using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class VoucherTypeController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        // GET: Voucher
        public ActionResult Index()
        {
            ViewBag.Manager = _connection.tblBosses.ToList();
            return View();
        }

        // Get Table 
        public PartialViewResult GetTable
        (
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null,
            int? lid = null,
            string type = "",
            string code = "",
            string name = ""
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

             var voucher = _connection
                .GetAllVoucherRent(bid, cid, lid, type, code, name)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

             var sum = voucher.Sum(s=>s.Amount_In_Month);

             ViewData["sumTotal"] = $"{sum:#,##0.##}";
             
            return PartialView(voucher);
        }
        
        // Store
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Store(FormCollection form)
        {
            var location = int.Parse(form["location"]);
            var code = form["code"];
            var name = form["name"];
            
            var findExist = _connection.tblVoucher_Rent_Invertory_Rent_And_Other_Voucher;
            
            var codeExist = findExist.Any(v=> v.Code == code && v.FK_loc_id == location);
            if (codeExist) return Json(new {code = "code already exist"});
            
            var nameExist = findExist.Any(v=> v.Name == name && v.FK_loc_id == location);
            if (nameExist) return Json(new {name = "name already exist"});
            
            var entity = new tblVoucher_Rent_Invertory_Rent_And_Other_Voucher()
            {
                FK_loc_id = location,
                Type = form["type"],
                Name = form["name"],
                Code = form["code"],
                QTY_in_Month = int.Parse(form["qtyOfMonth"]),
                Unit_Price_in_Month = double.Parse(form["priceOfMonth"]),
                Amount_In_Month =  double.Parse(form["amountOfMonth"]),
                Status = form["status"],
                Picture = form["photo_voucher"],
                Start_Rent = form["start"],
                End_Rent = form["ends"],
                Discount = double.Parse(form["discount"]),
                Amount_Month_Year_Rent = form["amountMonthYearRent"],
                User_Update = _helper.GetUserLoginId(),
                Date_Update = Constraint.GetDate(),
                Time_Update = Constraint.GetTime()
            };
            
            _connection.tblVoucher_Rent_Invertory_Rent_And_Other_Voucher.Add(entity);
            _connection.SaveChanges();

            RemoveFile();
            
            return new JsonResult() { Data = "Created Successfully."};
        }

        // update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection formCollection)
        {
            var voucherId = int.Parse(formCollection["voucherTypeId"]);
            var locationId = int.Parse(formCollection["locationId"]);
            var code = formCollection["code_edit"];
            var name = formCollection["name_edit"];
            
            var findExist = _connection.tblVoucher_Rent_Invertory_Rent_And_Other_Voucher;
            
            var codeExist = findExist
                .Any(v=> v.Code == code && v.FK_loc_id == locationId && v.PK_Voucher_Rent_Other_Id != voucherId);
            if (codeExist) return Json(new {code = "code already exist"});
            
            var nameExist = findExist
                .Any(v=> v.Name == name && v.FK_loc_id == locationId && v.PK_Voucher_Rent_Other_Id != voucherId);
            if (nameExist) return Json(new {name = "name already exist"});

            var entity = _connection
                .tblVoucher_Rent_Invertory_Rent_And_Other_Voucher
                .Single(v =>v.PK_Voucher_Rent_Other_Id == voucherId);

            if (entity == null) return Json(new {error = "id not found."});
                
            entity.Type = formCollection["type_edit"];
            entity.Code = formCollection["code_edit"];
            entity.Name = formCollection["name_edit"];
            entity.QTY_in_Month = int.Parse(formCollection["qtyOfMonth_edit"]);
            entity.Unit_Price_in_Month = double.Parse(formCollection["priceOfMonth_edit"]);
            entity.Amount_In_Month = double.Parse(formCollection["amountOfMonth_edit"]);
            entity.Status = formCollection["status_edit"];
            entity.Picture = formCollection["photo_voucher_edit"];
            entity.Start_Rent = formCollection["start_edit"];
            entity.End_Rent = formCollection["end_edits"];
            entity.Discount = double.Parse(formCollection["discount_edit"]);
            entity.Amount_Month_Year_Rent = formCollection["amountMonthYearRent_edit"];
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();

            _connection.SaveChanges();
            
            RemoveFile();
            
            return new JsonResult() { Data = "Updated Successfully."};
        }
        
        // Remove File Name 
        private void RemoveFile()
        {
            var dir = HttpContext.Server.MapPath("~/Content/Uploads/VoucherType/");

            var filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/VoucherType/"));

            var fileInPath = new List<string>();

            foreach (var file in filePaths)
            {
                fileInPath.Add(Path.GetFileName(file));
            }            
            // Remove
            foreach (var fileName in fileInPath) {

                var nameInDb = _connection
                    .tblVoucher_Rent_Invertory_Rent_And_Other_Voucher
                    .Where(s =>s.Picture == fileName);

                if (!nameInDb.Any())
                {
                    var fileInfo = new FileInfo(dir + fileName);

                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }
            }            
        }
        
        //drop down voucher 
        public JsonResult AutoCompleteVoucher(string value)
        {
            var voucher = _connection
                .tblVoucher_Rent_Invertory_Rent_And_Other_Voucher
                .Where(v=> v.Name.Contains(value))
                .Select(v => new
                {
                    v.PK_Voucher_Rent_Other_Id ,
                    v.Name,
                    v.Discount,
                    v.Code,
                    v.QTY_in_Month,
                    v.Unit_Price_in_Month,
                    v.Amount_In_Month,
                    v.Status,
                    v.User_Update
                }).ToList();
            return Json(voucher, JsonRequestBehavior.AllowGet);
        }
    }
}