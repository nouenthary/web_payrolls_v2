using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;
using web_payrolls.Models.DTO;
using web_payrolls.Views.Transfer;

namespace web_payrolls.Controllers
{
  public class TransferController : Controller
  {
    // Connection
    private readonly DB_Connection _connection = new DB_Connection();

    // Context
    private readonly ClHelper _helper = new ClHelper();

    private string _sqlQuery = "";

    // GET
    //page=2&bid=1&company=1&type=&type_name=&product_name=&color=&size=&grade=&barcode=&qrCode=&status=&invoice=&pageSize=1
    public ActionResult Index(
      int? page,
      int? pageSize,
      string bid,
      string company,
      string type,
      string type_name,
      string product_name,
      string color,
      string size,
      string grade,
      string barcode,
      string qrCode,
      string status,
      string invoice,
      string location,
      string start,
      string end
    )
    {
      var sizes = pageSize ?? 20;
      var pageIndex = 1;
      pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

      //_sqlQuery = ;

      var data = _connection.GetAllTransferLocation(
          bid,
          company,
          location,
          "",
          type_name,
          product_name,
          "",
          barcode,
          qrCode,
          grade,
          color,
          size,
          status,
          start,
          end,
          invoice
        )
        .ToList()
        .ToPagedList(pageIndex, sizes);

      return View(data);
    }

    // GET
    public ActionResult Create(int stockInCutNoLocationId)
    {
      var exist = _connection.tblProduction_StockIn_Cut_On_LocNo.Find(stockInCutNoLocationId);

      if (exist == null)
      {
        return RedirectToAction("Index");
      }

      var data = _connection
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

      _connection.tblProduction_StockIn_Cut_On_LocNo.Add(entity);
      _connection.SaveChanges();

      entity.StockIn_Cut_On_LocNo = entity.PK_StockIn_Cut_On_LocNo_Id;
      _connection.SaveChanges();

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

      var exitItem = _connection.tblProduction_StockIn_Cut_On_Loc
        .Any(x => x.FK_Grade_Id == gradeId && x.FK_Loc_Id == locationId && x.StockIn_Cut_On_LocNo_Id == cutLocationId);

      if (exitItem)
      {
        var item = _connection.tblProduction_StockIn_Cut_On_Loc
          .FirstOrDefault(x =>
            x.FK_Grade_Id == gradeId && x.FK_Loc_Id == locationId && x.StockIn_Cut_On_LocNo_Id == cutLocationId);

        item.In_QTY += qty;

        _connection.SaveChanges();

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

      _connection.tblProduction_StockIn_Cut_On_Loc.Add(entity);
      _connection.SaveChanges();

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

      var item = _connection.tblProduction_StockIn_Cut_On_Loc.Find(id);

      if (item == null)
      {
        return Json(new {error = "id not found."});
      }

      item.In_QTY = qty;
      _connection.SaveChanges();

      return Json(new {message = "Item was updated."});
    }


    // remove
    [HttpDelete]
    [ValidateAntiForgeryToken]
    public JsonResult RemoveItem(int id)
    {
      var item = _connection.tblProduction_StockIn_Cut_On_Loc.Find(id);

      if (item == null)
      {
        return Json(new {error = "id not found."});
      }

      _connection.tblProduction_StockIn_Cut_On_Loc.Remove(item);
      _connection.SaveChanges();

      return Json(new {message = "Item was remove."});
    }

    //update status
    [HttpPut]
    [ValidateAntiForgeryToken]
    public JsonResult UpdateStatus(int no, string status)
    {
      // import to balance

      if (status == "Done")
      {
        var items = _connection.tblProduction_StockIn_Cut_On_Loc
          .Where(x => x.StockIn_Cut_On_LocNo_Id == no && x.Confirm_Status == "Pending")
          .ToList();

        foreach (var row in items)
        {
          var exist = _connection.tblProduction_StockIn_Cut_On_LocBalance;

          if (exist.Any(x => x.FK_Grade_Id == row.FK_Grade_Id && x.FK_Loc_Id == row.FK_Loc_Id))
          {
            var update = exist.FirstOrDefault(x => x.FK_Grade_Id == row.FK_Grade_Id && x.FK_Loc_Id == row.FK_Loc_Id);
            if (update != null) update.In_QTY += row.In_QTY;
            _connection.SaveChanges();
          }
          else
          {
            var entity = new tblProduction_StockIn_Cut_On_LocBalance()
            {
              FK_Grade_Id = row.FK_Grade_Id,
              FK_Loc_Id = row.FK_Loc_Id,
              In_QTY = row.In_QTY,
              Stock_In_Type = "Processing"
            };
            _connection.tblProduction_StockIn_Cut_On_LocBalance.Add(entity);
            _connection.SaveChanges();
          }
        }
      }

      // update status
      _sqlQuery =
        $"UPDATE [tblProduction-StockIn_Cut_On_Loc] " +
        $"SET [Confirm_Status] = '{status}', " +
        $"[Confirm_PK_U_Id] = '{_helper.GetUserLoginId()}'," +
        $" [Confirm_Date] = '{Constraint.GetDate()}', " +
        $"[Confirm_Time] = '{Constraint.GetTime()}'" +
        $" WHERE [StockIn_Cut_On_LocNo_Id] = {no}";

      _connection.Database.ExecuteSqlCommand(_sqlQuery);
      _connection.SaveChanges();

      return Json(new {message = "Updated successfully."});
    }

    //    setParams('barcode');
    //    setParams('qrCode');
    //    setParams('status');
    //    setParams('grade');
    //    setParams('size');
    //    setParams('color');
    //    setParams('product_name');
    //    setParams('type_name');
    //    setParams('type');
    //    setParams('company');
    //    setParams('location');
    //    setParams('bid');
    //   setParams('pageSize');

    // Transfer Balance View
    public ActionResult TransferBalance
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

      var data = _connection.GetAllTransferBalance
          (bid, company, location, "", type_name, product_name, barcode, qrCode, grade, color, size, status)
        .ToList()
        .ToPagedList(pageIndex, sizes);

      return View(data);
    }

    //get table
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

      var data = _connection.GetAllTransferBalance
          (bid, company, location, "", type_name, product_name, barcode, qrCode, grade, color, size, status)
        .ToList()
        .ToPagedList(pageIndex, sizes);
      return PartialView(data);
    }
  }
}
