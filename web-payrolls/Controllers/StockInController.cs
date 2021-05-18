using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class StockInController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        private readonly ContextProvider _provider = new ContextProvider(new ClHelper(), new DB_Connection());
        // GET
        public ActionResult Index()
        {
            // List HOD
            ViewBag.Manager = _connection.tblBosses.ToList();

            // ManagerId
            ViewData["ManagerId"] = _provider.ManagerId;
            
            // List Company
            ViewBag.Company = _connection
                .tblCompanies
                .Where(company => company.FK_Boss_Id == _provider.ManagerId)
                .ToList();
            
            // CompanyId
            ViewBag.CompanyId = _provider.CompanyId;
            
            // Product Type
            ViewBag.ProductType = _connection
                .tblProduction_ProductType
                .Where(type => type.FK_Boss_Id == _provider.ManagerId)
                .ToList();
                   
            return View();
        }
        
        // sup view
        public PartialViewResult GetTable
            (
                int? page,
                int? pageSize,
                //
                int? bid = null,
                //
                int? cid = null,
                //
                string types = "",
                int? typeName = null,
                //
                int? product = null,
                //
                string codeProduct = "",
                string grade = "",
                string barcode = "",
                string qrCode = "",
                string color = "",
                string size = "",
                string status = "",
                string startDate = "",
                string endDate = "",
                string type = "" 
            )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var stockIn = _connection
                .GetAllProductStockIn
                (
                    bid,
                    cid,
                    typeName,
                    types,
                    product,
                    codeProduct,
                    barcode, 
                    qrCode,
                    grade,
                    color,
                    size,
                    status,
                    startDate,
                    endDate,
                    type
                )
                .Where(result => result.Pro_Type.Equals("Accessory") || result.Pro_Type.Equals("Fabric"))
                .ToList()
                .ToPagedList(pageIndex , defaultPage);
            
            return PartialView(stockIn);
        }

        // create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(FormCollection form)
        {
            var gradeId = int.Parse(form["gradeId"]);
            var companyId = int.Parse(form["company"]);
            // find grade id
            var gradeEntity = _connection.tblProduction_Grade.Any(grade => grade.PK_Grade_Id == gradeId);

            // product type 
            var stockInType = _connection
                .tblProduction_Grade
                .Single(grade => grade.PK_Grade_Id == gradeId)
                .tblProduction_Product
                .tblProduction_ProductType
                .Pro_Type;

            if (gradeEntity == false)
            {
                return Json(new{error = "product grade id not found."});
            }

            var qty = int.Parse(form["qty"]);
            var price = double.Parse(form["price"]);
            var total = double.Parse(form["total"]);
            
            var entity = new tblProduction_Stock_In_Product_Import()
            {
                FK_Grade_Id = gradeId,
                FK_Com_Id = companyId,
                Type_ = form["type"],
                In_QTY = qty,
                Price = price,
                Total_Price = total,
                Stock_In_Type = stockInType,
                U_Update = _helper.GetUserLoginId(),
                In_Date = Constraint.GetDate(),
                In_Time = Constraint.GetTime(),
                Remark = form["desc-stock"],
                Confirm_Status = Status.Pending.ToString(),
                InVoicePicture = form["picture-stock"]
            };

            _connection.tblProduction_Stock_In_Product_Import.Add(entity);
            _connection.SaveChanges();

            RemoveFile();

            return Json(new { Message = "Created successfully.", id = entity.PK_Stock_In_Product_Import_Id});
        }
        
        // update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection form)
        {
            var stockId = int.Parse(form["txtStockId"]);
            var companyId = int.Parse(form["txtCompany"]);

            var stockEntity = _connection
                .tblProduction_Stock_In_Product_Import
                .Single(stock => stock.PK_Stock_In_Product_Import_Id == stockId);
            
            if (stockEntity == null)
            {
                return Json(new{error = "stockId id not found."});
            }

            var qty = int.Parse(form["txtQty"]);
            var price = double.Parse(form["txtPrice"]);
            var total = double.Parse(form["txtTotal"]);
            
            //stockEntity.FK_Grade_Id = gradeId;
            stockEntity.FK_Com_Id = companyId;
            stockEntity.Type_ = form["txtType"];
            stockEntity.In_QTY = qty;
            stockEntity.Price = price;
            stockEntity.Total_Price = total;
            //stockEntity.Stock_In_Type = form["txtStockInType"];
            stockEntity.U_Update = _helper.GetUserLoginId();
            stockEntity.In_Date = Constraint.GetDate();
            stockEntity.In_Time = Constraint.GetTime();
            stockEntity.Remark = form["txtDescStock"];
            stockEntity.InVoicePicture = form["picture-stock-edit"];

            _connection.SaveChanges();

            RemoveFile();
            
            return new JsonResult(){Data = "Updated successfully."};   
        }
        
        // confirm stock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ConfirmStock(FormCollection form)
        {
            var stockId = int.Parse(form["stockId"]);
            var status = form["status"];

            var stockEntity = _connection
                .tblProduction_Stock_In_Product_Import
                .Single(import => import.PK_Stock_In_Product_Import_Id == stockId);
            // check
            if (stockEntity == null)
            {
                return Json(new{error = "Stock id not found."});
            }
            // done
            if (status == "Done")
            {
                var entity = _connection.tblProduction_PK_Stock_In_Product_Import_Balance;

                if (entity.Any(balance =>
                    balance.FK_Grade_Id == stockEntity.FK_Grade_Id &&
                    balance.FK_Com_Id == stockEntity.FK_Com_Id &&
                    balance.Stock_In_Type == stockEntity.Stock_In_Type &&
                    balance.Type == stockEntity.Type_
                ))
                {
                    var updateEntity = entity.Single(balance =>
                        balance.FK_Grade_Id == stockEntity.FK_Grade_Id &&
                        balance.FK_Com_Id == stockEntity.FK_Com_Id &&
                        balance.Stock_In_Type == stockEntity.Stock_In_Type &&
                        balance.Type == stockEntity.Type_
                    );

                    updateEntity.QTY_Balance += stockEntity.In_QTY;

                    _connection.SaveChangesAsync();
                }
                else
                {
                    // set to stock balance
                    var stockBalance = new tblProduction_PK_Stock_In_Product_Import_Balance()
                    {
                        FK_Grade_Id = stockEntity.FK_Grade_Id,
                        FK_Com_Id = stockEntity.FK_Com_Id,
                        Stock_In_Type = stockEntity.Stock_In_Type,
                        Type = stockEntity.Type_,
                        QTY_Balance = stockEntity.In_QTY
                    };
                
                    _connection.tblProduction_PK_Stock_In_Product_Import_Balance.Add(stockBalance);    
                }
                
                // done
                stockEntity.Confirm_Status = status;
            }
            // rejected
            else if (status == Status.Reject.ToString())
            {
                // rejected
                stockEntity.Confirm_Status = status;
            }
            
            stockEntity.Confirm_PK_U_Id = _helper.GetUserLoginId();
            stockEntity.Confirm_Date = Constraint.GetDate();
            stockEntity.Confirm_Time = Constraint.GetTime();
            
            _connection.SaveChanges();

            return new JsonResult(){Data = "You was confirm."};
        }


        // Remove File Name 
        public void RemoveFile()
        {
            var dir = HttpContext.Server.MapPath("~/Content/Uploads/Invoice/");

            var filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/Invoice/"));

            var fileInPath = new List<string>();

            foreach (var file in filePaths)
            {
                fileInPath.Add(Path.GetFileName(file));
            }
            // Remove
            foreach (var fileName in fileInPath)
            {

                var nameInDb = _connection.tblProduction_Stock_In_Product_Import.Where(s => s.InVoicePicture == fileName);

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

        // get table product grade
        public PartialViewResult GetTableGrade
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
            ViewBag.psizeGrade = defaultPage;

            ViewBag.PageSizeGrade = Constraint.PerPage;
            
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
                .Where(result => result.Pro_Type.Equals("Accessory") || result.Pro_Type.Equals("Fabric"))
                .ToList()
                .ToPagedList(pageIndex, defaultPage);
            
            return PartialView(productGrade);
        }
    }
}