using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;
using web_payrolls.Models.Fabric;

namespace web_payrolls.Controllers
{
    public class FabricController : Controller
    {
        // connection
        private readonly DB_Connection _connection = new DB_Connection();

        private readonly ClHelper _helper = new ClHelper();

        // GET
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetTable
            (
                int? page,
                int? pageSize,
                int? bid = null,
                int? cid = null,
                string type = "",
                int? typeName = null,
                int? product = null,
                string codeProduct = "",
                string barcode = "",
                string qrCode = "",
                string grade = "",
                string color = "",
                string size = "",
                string start = "",
                string end = "",
                string productCutNo = ""
            )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var useFabric = _connection
                .GetAllUseFabric
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
                       start,
                       end,
                       productCutNo
                )
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

            return PartialView(useFabric);
        }

        //Create
        public ActionResult Create(int Stock_In_Product_Cut_No)
        {
          var exist = _connection.tblProduction_Stock_In_Product_Cut_No.Any(x=>x.Stock_In_Product_Cut_No == Stock_In_Product_Cut_No);

          if (exist == false)
          {
            return RedirectToAction("Index");
          }

          var data = _connection
            .tblProduction_Use_Fabrik_On_Cut
            .Where(x => x.Stock_In_Product_Cut_No == Stock_In_Product_Cut_No)
            .ToList();

          return View(data);
        }

        // Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(List<Fabric> fabrics, int stockInProductCutNoId)
        {
            // get first stock in product cut
            var stockInProductCut = _connection
                .tblProduction_PK_Stock_In_Product_Cut
                .SingleOrDefault(db => db.FK_Stock_In_Product_Cut_No_Id == stockInProductCutNoId);

            if (stockInProductCut == null)
            {
                return Json(new { error = "Stock in product cut id " + stockInProductCutNoId + " not found."});
            }

            var stockInProductCutNo = _connection.tblProduction_Stock_In_Product_Cut_No
                .Single(db => db.PK_Stock_In_Product_Cut_No_Id == stockInProductCutNoId);

            // get first stock in product cut

            foreach (var item in fabrics)
            {
                var gradeEntity = _connection
                    .tblProduction_Grade
                    .Any(grade => grade.PK_Grade_Id == item.GradeId);

                if (gradeEntity)
                {
                    var entity = new tblProduction_Use_Fabrik_On_Cut()
                    {
                        FK_Grade_Id = item.GradeId,
                        FK_Com_Id = stockInProductCut.FK_Com_Id,
                        Stock_In_Product_Cut_No = stockInProductCutNo.Stock_In_Product_Cut_No,
                        In_QTY = item.Qty,
                        Stock_In_Type = ProductType.Fabric.ToString(),
                        U_Update = _helper.GetUserLoginId(),
                        In_Date = item.Date,
                        In_Time = item.Time
                    };

                    _connection.tblProduction_Use_Fabrik_On_Cut.Add(entity);


                    // check to fabric balance
                    var findFabricBalance = _connection
                        .tblProduction_Use_Fabrik_On_Cut_Balance
                        .Any(balance =>
                            balance.FK_Grade_Id == item.GradeId &&
                            balance.FK_Com_Id == stockInProductCut.FK_Com_Id
                        );

                    // check if have
                    if (findFabricBalance == false)
                    {
                        var fabricBalance = new tblProduction_Use_Fabrik_On_Cut_Balance()
                        {
                            FK_Grade_Id = item.GradeId,
                            FK_Com_Id = stockInProductCut.FK_Com_Id,
                            In_QTY = 0,
                            Stock_In_Type = ProductType.Fabric.ToString()
                        };

                        _connection.tblProduction_Use_Fabrik_On_Cut_Balance.Add(fabricBalance);
                    }
                    else
                    {
                        var findFabricBalanceForAddMore = _connection
                            .tblProduction_Use_Fabrik_On_Cut_Balance
                            .Single(balance =>
                                balance.FK_Grade_Id == item.GradeId &&
                                balance.FK_Com_Id == stockInProductCut.FK_Com_Id
                            );

                        findFabricBalanceForAddMore.In_QTY += item.Qty;
                    }

                    _connection.SaveChanges();
                }
                else
                {
                    return Json(new {error = "Product Grade Id not found."});
                }
            }
            return Json(new {message = "Created successfully."});
        }

        // get product type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductByType(FormCollection form)
        {
            var type = form["productType"];

            if (type == "")
            {
                return Json(null);
            }

            var products = _connection
                .tblProduction_Product
                .Where(product => product.tblProduction_ProductType.Pro_Type == type)
                .Select(product => new
                {
                    key = product.PK_Pro_Id,
                    value = product.Pro_Name
                })
                .ToList();

            return Json(products);
        }

        // get product grade by product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductGradeByProduct(FormCollection form)
        {
            var id = int.Parse(form["id"]);

            if (id < 0)
            {
                return Json(null);
            }

            var productGrade = _connection
                .GetStockBalanceByProductId(id,"Fabric");

            return Json(productGrade);
        }

        // add item fabric
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddItemFabric(
          int stockInProductCutNo,
          int companyId,
          int gradeId,
          int qty
        )
        {
          // check stock import balance
          var stockBalance = _connection.tblProduction_PK_Stock_In_Product_Import_Balance
            .FirstOrDefault(x => x.FK_Com_Id == companyId && x.FK_Grade_Id == gradeId);

          if (stockBalance == null)
          {
            return Json(new { error = "stock balance id not found."});
          }

          if (qty > stockBalance.QTY_Balance)
          {
            return Json(new { error = "stock balance available " + stockBalance.QTY_Balance});
          }

          UpdateStockInImportBalance(companyId,gradeId,qty,"minus");

          // check stock import balance

          var tableUseFabric = _connection.tblProduction_Use_Fabrik_On_Cut;

          // exist
          if (tableUseFabric.Any(x => x.FK_Com_Id == companyId && x.FK_Grade_Id == gradeId && x.Stock_In_Product_Cut_No == stockInProductCutNo))
          {
            var updateEntity = tableUseFabric.FirstOrDefault(x => x.FK_Com_Id == companyId && x.FK_Grade_Id == gradeId && x.Stock_In_Product_Cut_No == stockInProductCutNo);

            if (updateEntity != null)
            {
              updateEntity.In_QTY += qty;
              updateEntity.U_Update = _helper.GetUserLoginId();
              updateEntity.In_Date = Constraint.GetDate();
              updateEntity.In_Time = Constraint.GetTime();
            }

            _connection.SaveChanges();

            UpdateFabricBalance(companyId,gradeId,qty);

            return Json(new {message = "Item add successfully."});
          }

          // add new
          var entity = new tblProduction_Use_Fabrik_On_Cut()
          {
              FK_Grade_Id = gradeId,
              FK_Com_Id = companyId,
              Stock_In_Product_Cut_No = stockInProductCutNo,
              In_QTY = qty,
              Stock_In_Type = "Fabric",
              U_Update = _helper.GetUserLoginId(),
              In_Date = Constraint.GetDate(),
              In_Time = Constraint.GetTime()
          };

          _connection.tblProduction_Use_Fabrik_On_Cut.Add(entity);
          _connection.SaveChanges();
          // add new

          UpdateFabricBalance(companyId,gradeId,qty);

          return Json(new {message = "Item add successfully."});
        }

        // update stock in import balance
        private void UpdateStockInImportBalance(int companyId, int gradeId , int qty, string status)
        {
          var stockInImport = _connection.tblProduction_PK_Stock_In_Product_Import_Balance
            .FirstOrDefault(x => x.FK_Grade_Id == gradeId && x.FK_Com_Id == companyId);

          switch (status)
          {
            case "minus":
              if (stockInImport != null) stockInImport.QTY_Balance -= qty;
              _connection.SaveChanges();
              break;
            case "add":
              if (stockInImport != null) stockInImport.QTY_Balance += qty;
              _connection.SaveChanges();
              break;
          }
        }

        // update fabric balance
        private void UpdateFabricBalance(int companyId, int gradeId, int qty)
        {
          var entity = _connection
            .tblProduction_Use_Fabrik_On_Cut_Balance;

          if (entity.Any(x => x.FK_Com_Id == companyId && x.FK_Grade_Id == gradeId))
          {
            var updateEntity = entity
              .FirstOrDefault(x=> x.FK_Com_Id== companyId && x.FK_Grade_Id == gradeId);

            if (updateEntity != null) updateEntity.In_QTY += qty;

            _connection.SaveChanges();

            return;
          }

          var newEntity = new tblProduction_Use_Fabrik_On_Cut_Balance()
          {
              FK_Com_Id = companyId,
              FK_Grade_Id = gradeId,
              In_QTY = qty,
              Stock_In_Type = "Fabric"
          };

          _connection.tblProduction_Use_Fabrik_On_Cut_Balance.Add(newEntity);
          _connection.SaveChanges();
        }

        // remove item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RemoveItem(int id)
        {
          var entity = _connection.tblProduction_Use_Fabrik_On_Cut.Find(id);

          if (entity == null)
          {
            return Json(new { error = "id not found."});
          }

          var fabricBalance = _connection.tblProduction_Use_Fabrik_On_Cut_Balance
            .FirstOrDefault(x=>x.FK_Com_Id == entity.FK_Com_Id && x.FK_Grade_Id == entity.FK_Grade_Id);

          if (fabricBalance != null)
          {
            fabricBalance.In_QTY -= entity.In_QTY;
            _connection.SaveChanges();

            var companyId = Convert.ToInt32(entity.FK_Com_Id);

            var gradeId = Convert.ToInt32(entity.FK_Grade_Id);

            var qty = Convert.ToInt32(entity.In_QTY);

            UpdateStockInImportBalance(companyId, gradeId, qty, "add");
          }

          _connection.tblProduction_Use_Fabrik_On_Cut.Remove(entity);
          _connection.SaveChanges();

          return Json(new { message = "Item remove successfully."});
        }

        // update item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateItem(int id, int qty)
        {
          var entity = _connection.tblProduction_Use_Fabrik_On_Cut.Find(id);

          if (entity == null)
          {
            return Json(new {error = "id not found."});
          }

          var fabricBalance = _connection.tblProduction_Use_Fabrik_On_Cut_Balance
            .FirstOrDefault(x => x.FK_Grade_Id == entity.FK_Grade_Id && x.FK_Com_Id == entity.FK_Com_Id);

          var oldQty = Convert.ToInt32(entity.In_QTY);

          var sumQty = 0;

          var companyId = Convert.ToInt32(entity.FK_Com_Id);

          var gradeId = Convert.ToInt32(entity.FK_Grade_Id);
          // check stock in import
          var stockInImportBalance = _connection.tblProduction_PK_Stock_In_Product_Import_Balance
            .FirstOrDefault(x => x.FK_Com_Id == companyId && x.FK_Grade_Id == gradeId);

          if (qty > oldQty)
          {
            sumQty = qty - oldQty;

            if (stockInImportBalance != null && sumQty > stockInImportBalance.QTY_Balance)
            {
              return Json(new {error = "stock in import available " + stockInImportBalance.QTY_Balance});
            }

            fabricBalance.In_QTY += sumQty;
            _connection.SaveChanges();

            UpdateStockInImportBalance(companyId,gradeId,sumQty,"minus");
          }
          else
          {
            sumQty = oldQty - qty;
            fabricBalance.In_QTY -= sumQty;
            _connection.SaveChanges();

            UpdateStockInImportBalance(companyId,gradeId,sumQty,"add");
          }

          entity.In_QTY = qty;

          _connection.SaveChanges();

          return Json(new {message = "Item update successfully."});
        }
    }
}
