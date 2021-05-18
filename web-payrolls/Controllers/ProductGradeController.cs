using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class ProductGradeController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        private readonly ContextProvider _provider = new ContextProvider(new ClHelper(), new DB_Connection());
        // GET
        public ActionResult Index()
        {
            // Get Manager
            var hod = _connection
                .tblBosses
                .ToList();

            ViewBag.Manager = hod;

            // For select manager id
            ViewData["ManagerId"] = _provider.ManagerId;

            // get measure
            ViewBag.Measure = _connection
                .tblProduction_Measur
                .Where(b=> b.FK_Boss_Id == _provider.ManagerId)
                .ToList();
            // get Company
            var companies = _connection
                .tblCompanies
                .Where(company => company.FK_Boss_Id == _provider.ManagerId)
                .ToList();
            ViewBag.Company = companies;
            return View();
        }

        //sub view
        public PartialViewResult GetTable
            (
            int? page,
            int? pageSize,
            // HOD
            int? bid = null,
            // Product Type
            string type = "",
            int? typeName = null,
            // Product Name
            int? productId = null,
            string productName = "",
            // Product Grade
            string codeProduct = "",
            string grade = "",
            string barcode = "",
            string qrCode = "",
            string color = "",
            string size = "",
            string status = ""
            )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var productGrade = _connection
                .GetAllProductGrade(
                    bid,
                    typeName,
                    type,
                    productId,
                    codeProduct,
                    barcode,
                    qrCode,
                    grade,
                    color,
                    size,
                    status
                )
                .ToList()
                .ToPagedList(pageIndex, defaultPage);
            return PartialView(productGrade);
        }

        // Create Grade
        public object Create(int? productId, string productType)
        {
          // check product id
            var products = _connection.tblProduction_Product.Any(product => product.PK_Pro_Id == productId);

            if (products == false)
            {
                TempData["message"] = "Product Id find not found.";
                return RedirectToAction("Index","Product");
            }

            string []type = {"Accessory","Fabric","Processing","Recycling"};

            var existType = false;

            foreach (var row in type)
            {
              if (row == productType)
              {
                existType = true;
              }
            }

            if (existType == false)
            {
              return RedirectToAction("Index","Product");
            }

            var productMeasure = _connection
                .tblProduction_Measur
                .Where(measure => measure.FK_Boss_Id == _provider.ManagerId)
                .ToList();
            // List Measure
            ViewBag.Measure = productMeasure;

            // List Product Type
            ViewBag.ProductType = new List<SelectListItem>()
            {
                new SelectListItem(){Text = @"Accessory",Value = "Accessory"},
                new SelectListItem(){Text = @"Fabric",Value = "Fabric"},
                new SelectListItem(){Text = @"Processing",Value = "Processing"},
                new SelectListItem(){Text = @"Recycling",Value = "Recycling"}
            };

            return View();
        }


        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(FormCollection form)
        {
            var productId = int.Parse(form["productId"]);
            var qrCode = form["qrCode"];
            var barcode = form["barcode"];
            var grade = form["grade"];
            var color = form["color"];
            var size = form["size"];

            var doubleWholePrice = double.Parse(form["doubleWholePrice"]);
            var wholePrice = double.Parse(form["wholePrice"]);
            var unitPrice = double.Parse(form["unitPrice"]);
            var cost = double.Parse(form["cost"]);
            var normalPrice = double.Parse(form["normalPrice"]);
            var discount = double.Parse(form["discount"]);

            var entityGrade = _connection.tblProduction_Grade;

            if (qrCode != "" && entityGrade.Any(pg=> pg.QR_Code == qrCode))
            {
                return Json(new {qrCode = "QR Code already exist."});
            }

            if (barcode != "" && entityGrade.Any(pg=> pg.BarCode == barcode))
            {
                return Json(new {barcode =  "Barcode already exist."});
            }

            if (entityGrade.Any(pg=> pg.FK_Product_Id == productId && pg.Grade_Name == grade && pg.Color == color && pg.Size == size))
            {
                return Json(new {size = "Size already exist."});
            }

            var entity = new tblProduction_Grade()
            {
                FK_Product_Id = productId,
                QR_Code = qrCode,
                BarCode = barcode,
                Measu_Name = form["productMeasure"],
                Grade_Name = grade,
                Double_Whole_Price = doubleWholePrice,
                Whole_Price = wholePrice,
                Unit_Price = unitPrice,
                Cost = cost,
                Normal_Price = normalPrice,
                Picture_Path = form["picturePath"],
                Discount = discount,
                Start_Dis = form["startDiscount"],
                End_Dis = form["endDiscount"],
                Status_Dis = form["statusDiscount"],
                Pro_Status = form["ProductStatus"],
                User_Update = _helper.GetUserLoginId(),
                Date_Update = Constraint.GetDate(),
                Time_Update = Constraint.GetTime(),
                Descr = form["desc"],
                Color = color,
                Size = size,
                Code_Product = form["productCode"]
            };

            _connection.tblProduction_Grade.Add(entity);
            _connection.SaveChanges();

            RemoveFile();

            return new JsonResult(){Data = "Created successfully."};
        }

        // Edit
        public ActionResult Edit(int? id, string productType)
        {
            var productGrade = _connection.tblProduction_Grade.Single(grade => grade.PK_Grade_Id == id);
            // check product grade id
            if (productGrade == null)
            {
                TempData["Message"] = "Product Grade find not found.";
                return RedirectToAction("Index","ProductGrade");
            }

            string []type = {"Accessory","Fabric","Processing","Recycling"};

            var existType = false;

            foreach (var row in type)
            {
              if (row == productType)
              {
                existType = true;
              }
            }

            if (existType == false)
            {
              return RedirectToAction("Index","Product");
            }

            // List Product type
            ViewBag.ProductType = new List<SelectListItem>()
            {
                new SelectListItem(){Text = @"Accessory",Value = "Accessory"},
                new SelectListItem(){Text = @"Fabric",Value = "Fabric"},
                new SelectListItem(){Text = @"Processing",Value = "Processing"},
                new SelectListItem(){Text = @"Recycling",Value = "Recycling"}
            };

            // List Measure
            ViewBag.Measure = _connection
                .tblProduction_Measur
                .Where(measure => measure.FK_Boss_Id == _provider.ManagerId)
                .ToList();

            // List Status
            ViewBag.Status = new[] { "Enable", "Disabled"};

            return View(productGrade);
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection form)
        {
            var gradeId = int.Parse(form["gradeId"]);
            var productId = int.Parse(form["productId"]);
            var productCode = form["productCode"];
            var qrCode = form["qrCode"];
            var barcode = form["barcode"];
            var grade = form["grade"];
            var color = form["color"];
            var size = form["size"];

            var doubleWholePrice = double.Parse(form["doubleWholePrice"]);
            var wholePrice = double.Parse(form["wholePrice"]);
            var unitPrice = double.Parse(form["unitPrice"]);
            var cost = double.Parse(form["cost"]);
            var normalPrice = double.Parse(form["normalPrice"]);
            var discount = double.Parse(form["discount"]);

            var entityGrade = _connection.tblProduction_Grade;

            // Qr Code
            if (qrCode != "" && entityGrade.Any(pg=> pg.QR_Code == qrCode && pg.PK_Grade_Id != gradeId))
            {
                return Json(new {qrCode = "QR Code already exist."});
            }
            // Barcode
            if (barcode != "" && entityGrade.Any(pg=> pg.BarCode == barcode && pg.PK_Grade_Id != gradeId))
            {
                return Json(new {barcode =  "Barcode already exist."});
            }
            // Grade
            if (entityGrade.Any(pg=> pg.FK_Product_Id == productId && pg.Grade_Name == grade && pg.Color == color && pg.Size == size && pg.PK_Grade_Id != gradeId))
            {
                return Json(new {size = "Size already exist."});
            }

            var entity = entityGrade.Single(pg=> pg.PK_Grade_Id == gradeId);

            if (entity == null)
            {
                return Json(new{error = gradeId + " = id not found."});
            }

            entity.QR_Code = qrCode;
            entity.BarCode = barcode;
            entity.Measu_Name = form["productMeasure"];
            entity.Grade_Name = grade;
            entity.Double_Whole_Price = doubleWholePrice;
            entity.Whole_Price = wholePrice;
            entity.Unit_Price = unitPrice;
            entity.Cost = cost;
            entity.Normal_Price = normalPrice;
            entity.Picture_Path = form["picturePath"];
            entity.Discount = discount;
            entity.Start_Dis = form["startDiscount"];
            entity.End_Dis = form["endDiscount"];
            entity.Status_Dis = form["statusDiscount"];
            entity.Pro_Status = form["ProductStatus"];
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();
            entity.Descr = form["desc"];
            entity.Picture_Detail = form["pictureDetail"];
            entity.Code_Product = productCode;
            entity.Color = color;
            entity.Size = size;

            _connection.SaveChanges();

            RemoveFile();

            return new JsonResult(){Data = "Updated successfully."};
        }

        // Remove File Name
        public void RemoveFile()
        {
            var dir = HttpContext.Server.MapPath("~/Content/Uploads/Grade/");

            var filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/Grade/"));

            var fileInPath = new List<string>();

            foreach (var file in filePaths)
            {
                fileInPath.Add(Path.GetFileName(file));
            }
            // Remove
            foreach (var fileName in fileInPath)
            {

                var nameInDb = _connection.tblProduction_Grade.Where(s => s.Picture_Path == fileName);

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

        // get product grade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductGrade(FormCollection form)
        {
            if (form["value"] == null)
            {
                return Json(null);
            }

            var value = form["value"];

            var grade = GetGrade()
                .Where(productionGrade => productionGrade.Grade_Name.Contains(value))
                .Select(productionGrade =>  productionGrade.Grade_Name);

            return Json(grade);
        }


        // get barcode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetBarcode(FormCollection form)
        {
            if (form["value"] == null)
            {
                return Json(null);
            }

            var managerId = int.Parse(form["bid"]);

            var value = form["value"];

            if (managerId == 0)
            {
                var barcodes = _connection
                    .tblProduction_Grade
                    .Where(grade => grade.BarCode.Contains(value))
                    .Select(grade => grade.BarCode)
                    .ToList();

                return Json(barcodes);
            }

            var barcode = _connection.GetBarcode(managerId, value);

            return Json(barcode);
        }

        // get qr code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetQrCode(FormCollection form)
        {
            if (form["value"] == null)
            {
                return Json(null);
            }

            var managerId = int.Parse(form["bid"]);

            var value = form["value"];

            if (managerId == 0)
            {
                var qrCodes = _connection
                    .tblProduction_Grade
                    .Where(grade => grade.QR_Code.Contains(value))
                    .Select(grade => grade.QR_Code)
                    .ToList();

                return Json(qrCodes);
            }

            var qrCode = _connection.GetQtCode(managerId, value);

            return Json(qrCode);
        }

        // Get List Grade
        public List<tblProduction_Grade> GetGrade()
        {
            var grade = (from manager in _connection.tblBosses
                    // product type
                    join type in _connection.tblProduction_ProductType on manager.PK_Boss_Id equals type.FK_Boss_Id
                    // product
                    join product in _connection.tblProduction_Product on type.PK_ProType_Id equals product.FK_ProType_Id
                    // color
                    join color in _connection.tblProduction_Color on product.PK_Pro_Id equals color.FK_Pro_Id
                    // size
                    join size in _connection.tblProduction_Size on color.PK_Color_Id equals size.FK_Color_Id
                    // grade
                    join grades in _connection.tblProduction_Grade on size.PK_Size_Id equals grades.FK_Product_Id
                    // select
                    where manager.PK_Boss_Id == _provider.ManagerId

                    select grades
                );

            return grade.ToList();
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductTypeNameByProductType(FormCollection form)
        {
            var productType = form["value"];
            var productTypeName = _connection
                .tblProduction_ProductType
                .Where(type => type.Pro_Type == productType)
                .Select(type => new {key = type.PK_ProType_Id, value = type.ProType_Name})
                .Distinct()
                .ToList();
            return Json(productTypeName);
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductGradeByProduct(FormCollection form)
        {
            var productId = int.Parse(form["id"]);

            var productGrade = _connection
                .tblProduction_Grade
                .Where(productionGrade => productionGrade.FK_Product_Id == productId);

            var grade = productGrade.Select(productionGrade => new
                {
                    key = productionGrade.PK_Grade_Id,
                    value = productionGrade.Grade_Name
                })
                .DistinctBy(arg => arg.value)
                .ToList();

            // color
            var color = productGrade
                .Select(c => c.Color)
                .Distinct()
                .ToList();
            // size
            var size = productGrade
                .Select(s => s.Size)
                .Distinct()
                .ToList();

            return Json(new
            {
                grade, color, size
            });
        }

        // modify product grade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ModifyProductsGrade(FormCollection form)
        {
            var productId = int.Parse(form["product_type_name_txt"]);

            var doubleWhole = double.Parse(form["double_whole_txt"]);
            var wholePrice = double.Parse(form["whole_price_txt"]);
            var unit = double.Parse(form["unit_txt"]);
            var cost = double.Parse(form["cost_txt"]);
            var normal = double.Parse(form["normal_txt"]);


            var productGrade = _connection
                .tblProduction_Grade
                .Where(grade => grade.FK_Product_Id == productId).ToList();

            if (productGrade.Count < 1)
            {
                return Json(new{error = "Product grade not found."});
            }

            foreach (var item in productGrade)
            {
                item.Double_Whole_Price = doubleWhole;
                item.Whole_Price = wholePrice;
                item.Unit_Price = unit;
                item.Cost = cost;
                item.Normal_Price = normal;
            }

            _connection.SaveChanges();

            return new JsonResult(){Data = "Updated successfully."};
        }
    }
}
