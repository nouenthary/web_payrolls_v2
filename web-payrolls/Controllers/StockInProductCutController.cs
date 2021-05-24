using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;
using web_payrolls.Models.Product;

namespace web_payrolls.Controllers
{
    public class StockInProductCutController : Controller
    {
        // Connection
        private readonly DB_Connection _connection = new DB_Connection();
        // Context
        private readonly ClHelper _helper = new ClHelper();

        //private string _sqlQuery = "";
        // GET
        public ActionResult Index()
        {
            return View();
        }

        // sub view
        public PartialViewResult GetTable
            (
            int? page,
            int? pageSize,
            // 1
            int? bid = null,
            // 2
            int? cid = null,
            // 3
            int? productTypeId = null,
            // 4
            string type = "",
            // 5
            int? typeName = null,
            // 6
            int? product = null,
            // 7
            string codeProduct = "",
            // 8
            string barcode = "",
            // 9
            string qrCode = "",
            // 10
            string grade = "",
            // 11
            string color = "",
            // 12
            string size = "",
            // 13
            string status = "",
            // 14
            string start = "",
            // 15
            string end = "",
            // 16
            string productCutNo = "null"
            )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var productCuts = _connection
                .GetStockInProductCuts
                    (
                    bid,
                    cid,
                    typeName,
                    type,
                    product,
                    codeProduct,
                    barcode,
                    qrCode,
                    grade,
                    color,
                    size,
                    status,
                    start,
                    end,
                    productCutNo
                    )
                .Where(result => result.Stock_In_Type == "Processing")
                .DistinctBy(x=>x.Stock_In_Product_Cut_No)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

            return PartialView(productCuts);
        }

        //Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Save(List<ProductCut> productCuts)
        {
            var stockInProductCutNoId = GetProductCutNo();

            foreach (var item in productCuts)
            {
                var entity = new tblProduction_PK_Stock_In_Product_Cut()
                {
                    FK_Grade_Id = item.GradeId,
                    FK_Com_Id = item.CompanyId,
                    FK_Stock_In_Product_Cut_No_Id = stockInProductCutNoId,
                    In_QTY = item.Qty,
                    Price = item.Price,
                    Total_Price = item.Total,
                    Stock_In_Type = item.ProductType,
                    In_Date = item.Date,
                    In_Time = item.Time,
                    Confirm_Status = "Pending",
                    U_Update = _helper.GetUserLoginId(),
                    Descr = item.Desc
                };

                _connection.tblProduction_PK_Stock_In_Product_Cut.Add(entity);
                _connection.SaveChanges();
            }

            // return Json(productCuts);
            return Json(new {message = "Created successfully.", id = stockInProductCutNoId});
        }

        // Create
        public ActionResult Create(int PK_Stock_In_Product_Cut_No_Id)
        {
          var entity = _connection
            .tblProduction_Stock_In_Product_Cut_No.Find(PK_Stock_In_Product_Cut_No_Id);

          if (entity == null)
          {
            return RedirectToAction("Index");
          }

          var data = _connection
            .tblProduction_PK_Stock_In_Product_Cut
            .Where(x => x.FK_Stock_In_Product_Cut_No_Id == PK_Stock_In_Product_Cut_No_Id && x.Stock_In_Type != "Reject")
            .ToList();

          return View(data);
        }

        // Edit
        public ActionResult Edit(int ? id)
        {
            var stockInProductCut = _connection
                .tblProduction_PK_Stock_In_Product_Cut
                .Where(cut => cut.FK_Stock_In_Product_Cut_No_Id == id);

            if (!stockInProductCut.Any())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        //update
        [HttpPost]
        public JsonResult Update(List<ProductCut> productCuts, int productCutNoId)
        {
           // var
           var stockInProductCut = _connection
               .tblProduction_PK_Stock_In_Product_Cut
               .Where(cut => cut.FK_Stock_In_Product_Cut_No_Id == productCutNoId)
               .ToList();

           if (!stockInProductCut.Any())
           {
               return Json(new {error = "Product cut not found."});
           }

           var listProductCutId = new List<long>();

           foreach (var item in productCuts)
           {
               // add new
               if (item.StockInProductCutId == 0)
               {
                   var entity = new tblProduction_PK_Stock_In_Product_Cut()
                   {
                       FK_Com_Id = item.CompanyId,
                       FK_Grade_Id = item.GradeId,
                       FK_Stock_In_Product_Cut_No_Id = productCutNoId,
                       In_QTY = item.Qty,
                       Price = item.Price,
                       Total_Price = item.Total,
                       Stock_In_Type = item.ProductType,
                       U_Update = _helper.GetUserLoginId(),
                       In_Date = item.Date,
                       In_Time = item.Time,
                       Confirm_Status = "Pending",
                       Descr = item.Desc
                   };

                   _connection.tblProduction_PK_Stock_In_Product_Cut.Add(entity);
                   _connection.SaveChanges();

                   listProductCutId.Add(entity.PK_Stock_In_Product_Cut_Id);
               }
               else
               {
                   var stockInCutId = item.StockInProductCutId;
                   var entity = _connection
                       .tblProduction_PK_Stock_In_Product_Cut
                       .Single(cut => cut.PK_Stock_In_Product_Cut_Id == stockInCutId);

                   entity.FK_Com_Id = item.CompanyId;
                   entity.FK_Grade_Id = item.GradeId;
                   entity.In_QTY = item.Qty;
                   entity.Price = item.Price;
                   entity.Total_Price = item.Total;
                   entity.Stock_In_Type = item.ProductType;
                   entity.U_Update = _helper.GetUserLoginId();
                   entity.In_Date = item.Date;
                   entity.In_Time = item.Time;
                   entity.Descr = item.Desc;

                   _connection.SaveChanges();

                   listProductCutId.Add(entity.PK_Stock_In_Product_Cut_Id);
               }
           }

           DeleteProductCut(listProductCutId, productCutNoId);

           //return Json(new { productCuts , productCutNoId});
           return new JsonResult(){Data = "Updated successfully."};
        }

        // remove
        private void DeleteProductCut(List<long> list, int productCutNoId)
        {
            if (list.Count <= 0) return;
            var productCut = _connection
                .tblProduction_PK_Stock_In_Product_Cut
                .Where(v => v.FK_Stock_In_Product_Cut_No_Id == productCutNoId)
                .Select(v=> v.PK_Stock_In_Product_Cut_Id)
                .ToList();

            foreach (var item in productCut)
            {
                var exist = false;
                foreach (var i in list)
                {
                    if (item == i)
                    {
                        exist = true;
                    }
                }

                if (exist == false)
                {
                    var delete = _connection
                        .tblProduction_PK_Stock_In_Product_Cut
                        .Single(d=> d.PK_Stock_In_Product_Cut_Id == item);
                    _connection.tblProduction_PK_Stock_In_Product_Cut.Remove(delete);
                    _connection.SaveChanges();
                }
            }
        }


        // get product cut no
        public int GetProductCutNo()
        {
            // number no
            var productCutNo = new tblProduction_Stock_In_Product_Cut_No
            {
                // get number last
                Stock_In_Product_Cut_No = GetNumberNoLast()
            };

            _connection.tblProduction_Stock_In_Product_Cut_No.Add(productCutNo);
            _connection.SaveChanges();
            // get product cut no id
            var productCutNoId = Convert.ToInt32(productCutNo.PK_Stock_In_Product_Cut_No_Id);

            return productCutNoId;
        }

        // get number no last
        public int GetNumberNoLast()
        {
            var productCutNo = _connection
                .tblProduction_Stock_In_Product_Cut_No.ToList();

            if (productCutNo.Count == 0)
            {
                return 1000;
            }
            //
            var productNumber = productCutNo.Last().Stock_In_Product_Cut_No + 1;

            return Convert.ToInt32(productNumber);
        }

        // confirm status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ConfirmStatus(FormCollection form)
        {
            var productCutNoId = int.Parse(form["stockCutId"]);
            var status = form["status"];

            var productCut = _connection.tblProduction_PK_Stock_In_Product_Cut;

            var entity = productCut
                .Where(cut => cut.FK_Stock_In_Product_Cut_No_Id == productCutNoId);
            // compare
            if (!entity.Any())
            {
                return Json(new { error = productCutNoId + " id  not found."});
            }
            // update data when confirm
            foreach (var item in entity)
            {
                item.Confirm_PK_U_Id = _helper.GetUserLoginId();
                item.Confirm_Date = Constraint.GetDate();
                item.Confirm_Time = Constraint.GetTime();
                item.Confirm_Status = status;
            }
            // save to stock in product cut balance id
            if (status == "Done")
            {
                foreach(var item in entity)
                {
                    var productCutBalance = _connection.tblProduction_StockIn_Product_Cut_Balance;

                    // check stock in balance
                    if (productCutBalance.Any(balance => balance.FK_Com_Id == item.FK_Com_Id && balance.FK_Grade_Id == item.FK_Grade_Id))
                    {
                        var updateEntity = productCutBalance
                            .Single(balance => balance.FK_Com_Id == item.FK_Com_Id && balance.FK_Grade_Id == item.FK_Grade_Id);

                        updateEntity.QTY_Balance += item.In_QTY;

                        _connection.SaveChangesAsync();
                    }
                    // not exist
                    else
                    {
                        var stockInProductCutBalance = new tblProduction_StockIn_Product_Cut_Balance()
                        {
                            FK_Grade_Id = item.FK_Grade_Id,
                            FK_Com_Id = item.FK_Com_Id,
                            Stock_In_Type = item.Stock_In_Type,
                            QTY_Balance = item.In_QTY
                        };
                        _connection.tblProduction_StockIn_Product_Cut_Balance.Add(stockInProductCutBalance);
                    }
                }
            }

            _connection.SaveChanges();

            return new JsonResult{ Data = "Confirm successfully." };
        }


        // Get Product Name
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProduct(FormCollection form)
        {
            // required bossId and product type
            var bossId = int.Parse(form["bossId"]);
            var productType = form["productType"];

            if (bossId < 0 || productType == "")
            {
                return Json(null);
            }

            var products = _connection
                .tblProduction_Product
                .Where(product =>
                    product.tblProduction_ProductType.Pro_Type == productType &&
                    product.tblProduction_ProductType.FK_Boss_Id == bossId
                )
                .Select(product => new
                {
                    key = product.PK_Pro_Id,
                    value = product.Pro_Name
                }).ToList();

            return Json(products);
        }

        // get product grade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductGrade(FormCollection form)
        {
            var productId = int.Parse(form["id"]);
            if (productId == 0)
            {
                return Json(null);
            }
            // list product grade
            var productGrade = _connection
                .tblProduction_Grade
                .Where(grade => grade.FK_Product_Id == productId)
                .Select(grade =>  new
                {
                    key = grade.PK_Grade_Id,
                    value = grade.Grade_Name,
                    color = grade.Color,
                    size = grade.Size,
                    img = grade.Picture_Path
                })
                .ToList();

            return Json(productGrade);
        }

        // get product cut no
        public JsonResult SearchProductCutNo(FormCollection form)
        {
            var productCutNo = _connection
                .tblProduction_PK_Stock_In_Product_Cut
                .Where(cut => cut.FK_Com_Id == 1 )
                .Select(cut => new
                {

                })
                .ToList();

            return Json(productCutNo, JsonRequestBehavior.AllowGet);
        }

        // get all product cut
        public PartialViewResult GetTableProductCut
            (
                int? page,
                int? pageSize,
                // 1
                int? bid = null,
                // 2
                int? cid = null,
                // 3
                int? productTypeId = null,
                // 4
                string type = "",
                // 5
                int? typeName = null,
                // 6
                int? product = null,
                // 7
                string codeProduct = "",
                // 8
                string barcode = "",
                // 9
                string qrCode = "",
                // 10
                string grade = "",
                // 11
                string color = "",
                // 12
                string size = "",
                // 13
                string status = "",
                // 14
                string start = "",
                // 15
                string end = "",
                // 16
                string productCutNo = ""
            )
        {

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var stockInProductCuts = _connection
                .GetAllStockProductCut(
                    bid,
                    cid,
                    typeName,
                    type,
                    product,
                    codeProduct,
                    barcode,
                    qrCode,
                    grade,
                    color,
                    size,
                    status,
                    start,
                    end,
                    productCutNo
                    )
                .Where(result => result.Stock_In_Type == "Processing")
                .ToList()
                .ToPagedList(pageIndex, defaultPage);;

            return PartialView(stockInProductCuts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductTypeByBoss(FormCollection form)
        {
            var bid = int.Parse(form["bossId"]);
            var productType = form["productType"];

            if (bid > 0 && productType == "" )
            {
                return Json(null);
            }

            var productTypes = _connection
                .tblProduction_ProductType
                .Where(type => type.FK_Boss_Id == bid && type.Pro_Type == productType)
                .Select(type => new
                {
                    key = type.PK_ProType_Id,
                    value = type.ProType_Name
                })
                .ToList();

            return Json(productTypes, JsonRequestBehavior.AllowGet);
        }

        // 2021-05-04
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GenerateInvoice()
        {
          var productionStockInProductCutNo = new tblProduction_Stock_In_Product_Cut_No();
          _connection.tblProduction_Stock_In_Product_Cut_No.Add(productionStockInProductCutNo);

          _connection.SaveChanges();

          var id = productionStockInProductCutNo.PK_Stock_In_Product_Cut_No_Id;

          var updateEntity = _connection.tblProduction_Stock_In_Product_Cut_No.Find(id);

          if (updateEntity != null)
            updateEntity.Stock_In_Product_Cut_No = Convert.ToInt32(id);

          _connection.SaveChanges();

          return Json(new {id});
        }

        // Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RemoveItem(int id)
        {
          var entity = _connection.tblProduction_PK_Stock_In_Product_Cut.Find(id);

          if (entity == null)
          {
            return Json(new { error = "id not found..."});
          }

          _connection.tblProduction_PK_Stock_In_Product_Cut.Remove(entity);
          _connection.SaveChanges();

          return Json(new { message = "Item was remove..."});
        }


        // add item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddItem(FormCollection form)
        {
          var gradeId = int.Parse(form["product_grade"]);
          var companyId = int.Parse(form["companyId"]);
          var fkStockIn = int.Parse(form["PK_Stock_In_Product_Cut_No_Id"]);
          var qty = int.Parse(form["qty_txt"]);
          var price = double.Parse(form["txt_price"]);
          var total = qty * price;

          var exist = _connection
            .tblProduction_PK_Stock_In_Product_Cut
            .FirstOrDefault(x => x.FK_Grade_Id == gradeId && x.FK_Stock_In_Product_Cut_No_Id == fkStockIn);
          if (exist != null)
          {
            var newQty = exist.In_QTY + qty;
            var newPrice = exist.Price + price;
            var newTotal = newQty * newPrice;

            exist.In_QTY = newQty;
            exist.Price = newPrice;
            exist.Total_Price = newTotal;

            _connection.SaveChanges();

            return Json(new { message = "Item was updated"});
          }

          var entity = new tblProduction_PK_Stock_In_Product_Cut()
          {
            FK_Grade_Id = gradeId,
            FK_Com_Id = companyId,
            FK_Stock_In_Product_Cut_No_Id = fkStockIn,
            In_QTY = qty,
            Price = price,
            Total_Price = total,
            Stock_In_Type = "Processing",
            U_Update = _helper.GetUserLoginId(),
            In_Date = Constraint.GetDate(),
            In_Time = Constraint.GetTime(),
            Confirm_Status = "Pending"
          };

          _connection.tblProduction_PK_Stock_In_Product_Cut.Add(entity);
          _connection.SaveChanges();

          return Json(new { message = "Item was created." });
        }

        // update item rows
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateRowItem(int id, int qty, double price)
        {
          var entity = _connection.tblProduction_PK_Stock_In_Product_Cut.Find(id);

          if (entity == null)
          {
            return Json(new { error = "id not found."});
          }

          entity.In_QTY = qty;
          entity.Price = price;
          entity.Total_Price = qty * price;

          _connection.SaveChanges();

          return Json(new {message = "Update successfully."});
        }
    }
}
