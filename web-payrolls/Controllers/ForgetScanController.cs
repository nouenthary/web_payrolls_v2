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
    public class ForgetScanController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();

        private readonly ClHelper _helper = new ClHelper();
        // GET: ForgetScan
        public ActionResult Index()
        {
            ViewBag.Manager = _connection.tblBosses.ToList();
            return View();
        }

        // Get Table View
        public ActionResult GetTable(
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null,
            int? lid = null,
            int? did = null,
            int? pid = null,
            string name = "",
            string id = "",
            string status = ""
        )
        {           
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.size = defaultPage;
           
            ViewBag.PageSize = Constraint.PerPage;

           var data = _connection
                .GetAllForgetScan(bid, cid, lid, did, pid, name, id, status)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

            var sum = data.Sum(s => s.Total_Price);

            ViewData["Sum_Total"] = $"{sum:#,##0.##}";

            return PartialView(data);
        }
       
        // Add Forget Scan
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public JsonResult AddForget(FormCollection form, tblForget_Scan entity)
        {
            try {
                var staffId = int.Parse(form["txt_sid_forget"]);
                var date = form["txt_date_forget"];
                var desc = form["desc_forget"];
                var price = double.Parse(form["txt_price_forget"]);
                var photo = form["txt_photo_forget"];

                var splitDate = date.Split('-');
                int.TryParse(splitDate[0], out var year);
                int.TryParse(splitDate[1], out var month);
                var dayOfMonth = DateTime.DaysInMonth(year, month);
                    
                var data = _connection.tblSalary_Type.Single(s => s.FK_Sta_Id == staffId);

                if (data == null) throw new Exception("Add forget error!");
                
                var salary = data.Salary;
                var type = data.Sal_Type.Trim();

                if (type == SalaryType.Monthly.ToString())
                {
                    var day = salary / dayOfMonth;
                    entity.Total_Price = day;
                }
                else if (type == SalaryType.Daily.ToString())
                {
                    entity.Total_Price = salary;
                }
                else if (type == SalaryType.Bills.ToString())
                {
                    entity.Total_Price = price;
                }

                entity.FK_Staff_Id = staffId;
                entity.Forget_Date = date;
                entity.Descr = desc;
                entity.Status = Status.Pending.ToString();
                entity.Picture = photo;
                entity.User_Update = _helper.GetUserLoginId();
                entity.Date_Update = Constraint.Date;
                entity.Time_Update = Constraint.Time;

                _connection.tblForget_Scan.Add(entity);
                _connection.SaveChanges();

                RemoveFile();

                return Json(new { message = "Created Successfully!", id = entity.PK_Forget_Scan_Id });
            } catch (Exception ex) {
                return Json(new { error = ex });
            }           
        }

        // Confirm 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ConfirmForget(FormCollection form)
        {
            try {
                var idForgetScan = int.Parse(form["id_forget_scan"]);
                var status = form["status"];

                var entity = _connection.tblForget_Scan.Single(f => f.PK_Forget_Scan_Id == idForgetScan);

                if (entity == null) throw new Exception(idForgetScan + " = not found.");

                entity.Status = status;            
            
                entity.User_Confirm = _helper.GetUserLoginId();
                entity.Date_Confirm = Constraint.GetDate();
                entity.Time_Confirm = Constraint.GetTime();
                
                _connection.SaveChanges();

                return new JsonResult() { Data = "Confirm Successfully!" };

            } catch (Exception ex) {
                return Json(new { error = ex });
            }           
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection form) {
            try {
                var forgetId = int.Parse(form["txt_forget_id"]);

                var entity = _connection.tblForget_Scan.Single(s => s.PK_Forget_Scan_Id == forgetId);
                
                if (entity == null) {
                    throw new Exception(forgetId + " = id forget not found.");
                }

                entity.Forget_Date = form["txt_date_forget_edit"];
                entity.Total_Price = double.Parse(form["txt_price_forget_edit"]);
                entity.Picture = form["txt_photo_forget_edit"];
                entity.Descr = form["desc_forget_eidt"];
                entity.User_Update = 2;
                entity.Date_Update = Constraint.GetDate();
                entity.Time_Update = Constraint.GetTime();

                _connection.SaveChanges();

                RemoveFile();

                return new JsonResult() { Data = "Update Successfully!" };
            }
            catch (Exception ex) {
                return Json( new { error = ex } );
            }
        }

        // Remove File Name 
        public void RemoveFile()
        {
            var dir = HttpContext.Server.MapPath("~/Content/Uploads/ForgetScan/");

            var filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/ForgetScan/"));

            var fileInPath = new List<string>();

            foreach (var file in filePaths)
            {
                fileInPath.Add(Path.GetFileName(file));
            }
            // Remove
            foreach (var fileName in fileInPath)
            {
                var nameInDb = _connection.tblForget_Scan.Where(s => s.Picture == fileName);

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
    }
}