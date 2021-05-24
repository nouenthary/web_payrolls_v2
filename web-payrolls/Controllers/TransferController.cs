using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;
using web_payrolls.Models.DTO;
using Constraint = web_payrolls.Models.Constraint;

namespace web_payrolls.Controllers
{
  public class TransferController : Controller
  {
    // Connection
    private readonly DB_Connection _db = new DB_Connection();

    // Context
    private readonly ClHelper _helper = new ClHelper();

    private string _sqlQuery = "";

    // GET
    public ActionResult Index()
    {
      return View();
    }

    // get table
    public PartialViewResult GetTable(SearchModel model)
    {

    //  var connection =  ConfigurationManager.ConnectionStrings["DB_db_Raw"].ConnectionString;
      var sizes = model.PageSize ?? 20;
      var pageIndex = 1;
      pageIndex = model.Page.HasValue ? Convert.ToInt32(model.Page) : 1;

      var data = new List<TransferModel>();

      _sqlQuery =
        $"Exec GetAllTransferLocation " +
        $"N'{model.BossId}'," +
        $"N'{model.CompanyId}'," +
        $"N'{model.LocationId}'," +
        $"N'{""}'," +
        $"N'{model.TypeName}'," +
        $"N'{model.ProductId}'," +
        $"N'{""}'," +
        $"N'{model.Barcode}'," +
        $"N'{model.QrCode}'," +
        $"N'{model.Grade}'," +
        $"N'{model.Color}'," +
        $"N'{model.Size}'," +
        $"N'{model.Status}'," +
        $"N'{model.StartDate}'," +
        $"N'{model.EndDate}'," +
        $"N'{model.No}'";

      //using (var con = new SqlConnection(connection))
      //{
      //  var cmd = new SqlCommand(_sqlQuery, con)
      //    {CommandType = CommandType.Text};

      //  con.Open();

      //  var rdr = cmd.ExecuteReader();
      //  while (rdr.Read())
      //  {
      //    var item = new TransferModel
      //    {
      //      Id = rdr["Id"] + "",
      //      Name = rdr["Boss"] + "",
      //      FromCompany = rdr["FromCompany"] + "",
      //      Location = rdr["Location"] + "",
      //      No = rdr["No"] + "",

      //      Qty = rdr["Qty"] + "",
      //      UserUpdate = rdr["UserUpdate"] + "",
      //      InDate = rdr["Date"] + "",
      //      InTime = rdr["Time"] + "",
      //      UserConfirm = rdr["UserConfirm"] + "",

      //      DateConfirm = rdr["ConfirmDate"] + "",
      //      TimeConfirm = rdr["ConfirmTime"] + "",
      //      Status = rdr["Status"] + "",
      //      ProductType = rdr["Type"] + "",
      //      TypeName = rdr["TypeName"] + "",

      //      Product = rdr["Product"] + "",
      //      Color = rdr["Color"] + "",
      //      Size = rdr["Size"] + "",
      //      Grade = rdr["Grade"] + "",
      //      Measure = rdr["Measure"] + "",

      //      Code = rdr["Code"] + "",
      //      Barcode = rdr["Barcode"] + "",
      //      QrCode = rdr["QrCode"] + ""
      //    };
      //    data.Add(item);
      //  }
      //}

      var entity = _db.GetAllTransferLocation(
          "","","","","","","","","","","","","","","",""
        )
        .ToList().ToPagedList(pageIndex, sizes);

      return PartialView(entity);
    }

    // GET
    public ActionResult Create(int stockInCutNoLocationId)
    {
      var exist = _db.tblProduction_StockIn_Cut_On_LocNo.Find(stockInCutNoLocationId);

      if (exist == null)
      {
        return RedirectToAction("Index");
      }

      var data = _db
        .tblProduction_StockIn_Cut_On_Loc
        .Where(x => x.StockIn_Cut_On_LocNo_Id == stockInCutNoLocationId)
        .ToList();

      return View(data);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public JsonResult GenerateInvoice()
    {
      var entity = new tblProduction_StockIn_Cut_On_LocNo();

      _db.tblProduction_StockIn_Cut_On_LocNo.Add(entity);
      _db.SaveChanges();

      entity.StockIn_Cut_On_LocNo = entity.PK_StockIn_Cut_On_LocNo_Id;
      _db.SaveChanges();

      return Json(new {id = entity.PK_StockIn_Cut_On_LocNo_Id});
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public JsonResult AddItem(FormCollection form)
    {
      var gradeId = int.Parse(form["product_grade"]);
      var locationId = int.Parse(form["location_id"]);
      var qty = int.Parse(form["qty"]);
      var cutLocationId = int.Parse(form["cut_location_id"]);

      if (qty < 1)
      {
        return Json(new {error = "qty must be greater then 0"});
      }

      var exitItem = _db.tblProduction_StockIn_Cut_On_Loc
        .Any(x => x.FK_Grade_Id == gradeId && x.FK_Loc_Id == locationId && x.StockIn_Cut_On_LocNo_Id == cutLocationId);

      if (exitItem)
      {
        var item = _db.tblProduction_StockIn_Cut_On_Loc
          .FirstOrDefault(x =>
            x.FK_Grade_Id == gradeId && x.FK_Loc_Id == locationId && x.StockIn_Cut_On_LocNo_Id == cutLocationId);

        item.In_QTY += qty;

        _db.SaveChanges();

        return Json(new {message = "Item add successfully."});
      }


      var entity = new tblProduction_StockIn_Cut_On_Loc()
      {
        FK_Grade_Id = gradeId,
        FK_Loc_Id = locationId,
        In_QTY = qty,
        StockIn_Cut_On_LocNo_Id = cutLocationId,
        Stock_In_Type = "Processing",
        U_Update = _helper.GetUserLoginId(),
        In_Date = Constraint.GetDate(),
        In_Time = Constraint.GetTime(),
        Confirm_Status = "Pending"
      };

      _db.tblProduction_StockIn_Cut_On_Loc.Add(entity);
      _db.SaveChanges();

      return Json(new {message = "Item add successfully."});
    }

    // update
    [HttpPut]
    [ValidateAntiForgeryToken]
    public JsonResult UpdateItem(int id, int qty)
    {
      if (qty < 1)
      {
        return Json(new {error = "qty must be greater then 0"});
      }

      var item = _db.tblProduction_StockIn_Cut_On_Loc.Find(id);

      if (item == null)
      {
        return Json(new {error = "id not found."});
      }

      item.In_QTY = qty;
      _db.SaveChanges();

      return Json(new {message = "Item was updated."});
    }


    // remove
    [HttpDelete]
    [ValidateAntiForgeryToken]
    public JsonResult RemoveItem(int id)
    {
      var item = _db.tblProduction_StockIn_Cut_On_Loc.Find(id);

      if (item == null)
      {
        return Json(new {error = "id not found."});
      }

      _db.tblProduction_StockIn_Cut_On_Loc.Remove(item);
      _db.SaveChanges();

      return Json(new {message = "Item was remove."});
    }

    //update status
    [HttpPut]
    [ValidateAntiForgeryToken]
    public JsonResult UpdateStatus(int id, string status)
    {
      //var entity = _con

      _db.Database.ExecuteSqlCommand(_sqlQuery);
      _db.SaveChanges();

      return Json(new {message = "Updated successfully."});
    }

    // Transfer Balance View
    public ActionResult TransferBalance()
    {
      return View();
    }

    // get sum view
    public PartialViewResult GetTableBalance
    (
      int? page,
      int? pageSize,
      string bid,
      string company,
      string location,
      string barcode,
      string qrCode,
      string status,
      string grade,
      string size,
      string color,
      string product_name,
      string type_name,
      string type
    )
    {
      var sizes = pageSize ?? 20;
      var pageIndex = 1;
      pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

      var data = _db.GetAllTransferBalance
          (bid, company, location, "", type_name, product_name, barcode, qrCode, grade, color, size, status)
        .ToList()
        .ToPagedList(pageIndex, sizes);

      return PartialView(data);
    }
  }
}
