using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;
using web_payrolls.Models.DTO;

namespace web_payrolls.Controllers
{
    public class ProductController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        private readonly ContextProvider _provider = new ContextProvider(new ClHelper(), new DB_Connection());
        
        // GET
        public ActionResult Index()
        {
            // Manager
            ViewBag.Manager = _connection
                .tblBosses
                .ToList();
            // Measure
            ViewBag.Measure = _connection
                .tblProduction_Measur
                .Where(m => m.FK_Boss_Id == _provider.ManagerId);
        
            // Selected Manager
            ViewData["ManagerId"] = _provider.ManagerId;
                           
            return View();
        }
        
        // sup view
        public PartialViewResult GetTable
        (
            int? page,
            int? pageSize,
            int? bid = null,
            string type = "",
            string typeName = "", 
            string product = ""
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var productType = _connection
                .GetAllProducts(bid , type, typeName, product)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);
            
            return PartialView(productType);
        }
        
        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(FormCollection form)
        {
            var productTypeId = int.Parse(form["productTypeId"]);
            var product = form["product"];

            var productEntity = _connection.tblProduction_Product;
            if (productEntity.Any(p=> p.FK_ProType_Id == productTypeId && p.Pro_Name == product))
            {
                return Json(new {error = "Product already exist."});
            }

            var entity = new tblProduction_Product()
            {
                FK_ProType_Id = productTypeId,
                Pro_Name = product,
                Picture_Path = form["picture"],
                Descr = form["desc"],
                User_Update = _helper.GetUserLoginId(),
                Date_Update = Constraint.GetDate(),
                Time_Update = Constraint.GetTime()
            };

            _connection.tblProduction_Product.Add(entity);
            _connection.SaveChanges();

            RemoveFile();
            
            return new JsonResult(){Data = "Created successfully."};
        }
        
        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection form)
        {
            var productTypeId = int.Parse(form["txtProductTypeId"]);
            var id = int.Parse(form["txtProductId"]);
            var product = form["txtProduct"];
            
            var productEntity = _connection.tblProduction_Product;
            if (productEntity.Any(p=> p.FK_ProType_Id == productTypeId && p.Pro_Name == product && p.PK_Pro_Id != id))
            {
                return Json(new {error = "Product already exist."});
            }

            var entity = productEntity.Single(p => p.PK_Pro_Id == id);
            if (entity == null)  return Json(new {error = id + " = id not found."});

            entity.FK_ProType_Id = productTypeId;
            entity.Pro_Name = product;
            entity.Picture_Path = form["txtPicture"];
            entity.Descr = form["txtDesc"];
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();
            
            _connection.SaveChanges();

            RemoveFile();
            
            return new JsonResult(){Data = "Updated successfully."};
        }

        public ActionResult ProductAttribute(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var data = _connection.tblProduction_Product.Find(id);

            if (data == null)
            {
                return RedirectToAction("Index");   
            }

            return View(data);
        }
        
        // Get Product type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductTypeByBoss(FormCollection form)
        {
            var id = int.Parse(form["id"]);
            if (id < 0)
            {
                return Json(null);
            }
            // product type
            var productType = _connection
                .tblProduction_ProductType
                .Where(type => type.FK_Boss_Id == id);
            // type
            var types = productType
                .Select(type => type.Pro_Type)
                .Distinct()
                .ToList();
            // type name
            var typeName = productType
                .Select(type => type.ProType_Name)
                .Distinct()
                .ToList();
            
            return Json(new
            {
                types,
                typeName
            });
        }
        
        // get product by product type
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult GetProductByProductType(FormCollection form)
        {
            var productTypeId = int.Parse(form["id"]);

            var products = _connection
                .tblProduction_Product
                .Where(product => product.FK_ProType_Id == productTypeId)
                .Select(product => new
                {
                    key = product.PK_Pro_Id,
                    value = product.Pro_Name
                })
                .ToList();
            
            return Json(products,JsonRequestBehavior.AllowGet);
        }
        
        // modal create product 
        public PartialViewResult ModalCreateProduct()
        {
            return PartialView();
        }

        // Remove File Name 
        public void RemoveFile()
        {
            var dir = HttpContext.Server.MapPath("~/Content/Uploads/Product/");

            var filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/Product/"));

            var fileInPath = new List<string>();

            foreach (var file in filePaths)
            {
                fileInPath.Add(Path.GetFileName(file));
            }
            // Remove
            foreach (var fileName in fileInPath)
            {

                var nameInDb = _connection.tblProduction_Product.Where(s => s.Picture_Path == fileName);

                if (nameInDb.Count() == 0)
                {
                    var fileInfo = new FileInfo(dir + fileName);

                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }
            }
        }

        // edit product by id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditProductById(FormCollection form)
        {
            var productId = int.Parse(form["productId"]);

            var entity = _connection
                .tblProduction_Product
                .Single(product => product.PK_Pro_Id == productId);

            // product type
            var productType = entity.tblProduction_ProductType.Pro_Type;
            
            // product type name
            var typeName= entity.tblProduction_ProductType.ProType_Name;
            
            // product type id
            var productTypeId = entity.tblProduction_ProductType.PK_ProType_Id;
            
            // hod id
            var hodId = entity.tblProduction_ProductType.FK_Boss_Id;

            // list product type
            var productTypes = _connection
                .tblProduction_ProductType
                .Where(type => type.Pro_Type == productType && type.FK_Boss_Id == hodId)
                .Select(type => new SelectListItem
                {
                    Value = type.PK_ProType_Id + "",
                    Text =  type.ProType_Name,
                    Selected = type.PK_ProType_Id == productTypeId
                });
            
            return Json(new
            {
                productTypeId,
                productType,
                typeName,
                productTypes
            });
        }
    }
}