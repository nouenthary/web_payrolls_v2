using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        private readonly ContextProvider _provider = new ContextProvider(new ClHelper(), new DB_Connection());
        // GET
        public ActionResult Index()
        {
            // List Manager
            ViewBag.Manager = _connection
                .tblBosses
                .ToList();

            ViewData["ManagerId"] = _provider.ManagerId;
            
            // Product Type
            ViewBag.ProductType = new List<SelectListItem>()
            {
                new SelectListItem(){Text = @"Accessory",Value = "Accessory"},
                new SelectListItem(){Text = @"Fabric",Value = "Fabric"},
                new SelectListItem(){Text = @"Processing",Value = "Processing"},
                new SelectListItem(){Text = @"Recycling",Value = "Recycling"}
            };
            return View();
        }
        
        // sup view
        public PartialViewResult GetTable
        (
            int? page,
            int? pageSize,
            int? bid = null,
            string type = "", 
            string product = ""
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;
            
            var productType = _connection
                .GetAllProductType(bid , type, product)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);;
            ;
            return PartialView(productType);
        }
        
        // create product type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(FormCollection form)
        {
            var hodId = int.Parse(form["hodId"]);
            var type = form["ProductType"];
            var typeName = form["ProductTypeName"];

            var productEntity = _connection
                .tblProduction_ProductType
                .Any(p => p.FK_Boss_Id == hodId && p.Pro_Type == type && p.ProType_Name == typeName);

            if (productEntity)
            {
                return Json(new{error = "Product Type already exist."});
            }

            var entity = new tblProduction_ProductType()
            {
                FK_Boss_Id = hodId,
                Pro_Type = type,
                ProType_Name = typeName,
                Descr = form["desc"],
                User_Update = _helper.GetUserLoginId(),
                Date_Update = Constraint.GetDate(),
                Time_Update = Constraint.GetTime()
            };

            _connection.tblProduction_ProductType.Add(entity);
            _connection.SaveChanges();
            
            return new JsonResult(){Data = "Created successfully."};
        }
        
        // Update Product type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection form)
        {
            var hodId = int.Parse(form["txtHodId"]);
            var id = int.Parse(form["txtProductTypeId"]);
            var type = form["txtProductType"];
            var typeName = form["txtProductTypeName"];

            var entityProductType = _connection.tblProduction_ProductType;
            if (entityProductType.Any(p=>
                p.FK_Boss_Id == hodId && 
                p.Pro_Type == type && 
                p.ProType_Name == typeName &&
                p.PK_ProType_Id != id
            ))
            {
                return Json(new{ProductType = "Product type already exist."});
            }

            var entity = entityProductType.Single(p => p.PK_ProType_Id == id);
            if (entity == null) return Json(new{error = id + "= not found."});

            entity.FK_Boss_Id = hodId;
            entity.Pro_Type = type;
            entity.ProType_Name = typeName;
            entity.Descr = form["txtDesc"];
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();

            _connection.SaveChanges();
            
            return new JsonResult(){Data = "Updated successfully."};
        }
    }
}