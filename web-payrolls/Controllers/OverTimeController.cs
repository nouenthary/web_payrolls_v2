using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class OverTimeController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        
        private readonly ClHelper _helper = new ClHelper();
        // GET: OverTime
        public ActionResult Index()
        {           
            ViewBag.GetBoss = _connection.tblBosses.ToArray();
            
            return View();
        }

        public ActionResult GetTable(
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null,
            int? lid = null,
            int? did = null,
            string sname = "",
            string idcard = "",
            string salaryType = ""
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var staff = _connection
                .GetAllStaffOverTime(bid, cid, lid, did, sname, idcard, salaryType)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

            return PartialView(staff);
        }       
             
        // Get Table OT
        public ActionResult GetTableOverTime(
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null,
            int? lid = null,
            int? did = null,
            int? pid = null,
            string status = ""
        ) {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psizeOT = defaultPage;

            ViewBag.PageSizeOT = Constraint.PerPage;

            var ot = _connection
                .GetAllOT(bid, cid, lid, did, pid, null, null, status)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

           var sum = ot.Sum(o => o.Total_Price);

            ViewData["sum_ot"] = $"{sum:#,##0.##}";

            return PartialView(ot);
        }

        // Add New OT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOverTime(tblOver_Time entity, FormCollection form) {
            try {
                entity.FK_Staff_Id = int.Parse(form["staff_id"]);
                entity.OT_Date = form["date"];
                entity.OT_From_Time = form["start_time"];
                entity.OT_To_Time = form["end_time"];
                entity.Picture = "";
                entity.Amount_Hour = double.Parse(form["amount_time"]);
                entity.Unit_Price_Hour = double.Parse(form["unit_price"]);
                entity.Total_Price = double.Parse(form["total_price"]);
                entity.Descr = form["desc"];
                entity.Status = Status.Pending.ToString();
                entity.User_Update = _helper.GetUserLoginId();
                entity.Date_Update = Constraint.GetDate();
                entity.Time_Update = Constraint.GetTime();

                _connection.tblOver_Time.Add(entity);
                _connection.SaveChanges();

                return Json(new { message = "Overtime has been added.", id = entity.PK_Over_Time_Id });

            } catch (Exception ex) {
                Console.Write(ex.Message);
            }

            return Json(new { error = "add over time failed."});
        }

        //Update OT to Done
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetStatus(FormCollection form) {           
            var id = int.Parse(form["ot_id"]);
            var status = form["status"];

            var entity = _connection
                .tblOver_Time
                .Single(o => o.PK_Over_Time_Id == id);

            if (entity == null) {
                return Json(new { error = "OverTime confirm failed." });
            }

            entity.User_Confirm = _helper.GetUserLoginId();            
            entity.Date_Confirm = Constraint.GetDate();
            entity.Time_Confirm = Constraint.GetTime();

            if (status == Status.Done.ToString())
            {
                entity.Status = Status.Done.ToString();
            }
            else if (status == Status.Reject.ToString())
            {
                entity.Status = Status.Reject.ToString();
            }

            _connection.SaveChanges();

            return new JsonResult() { Data = "OverTime update to done." };
        }

        //Update OT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateOT(FormCollection form) {
            var overtimeId = int.Parse(form["overtime_id"]);
            
            var entity = _connection
                .tblOver_Time
                .Single(ot => ot.PK_Over_Time_Id == overtimeId);
            if (entity == null)
            {
                return Json(new { error = "OverTime Confirm failed." });
            }
            
            entity.OT_Date = form["date"];
            entity.OT_From_Time = form["start_time"];
            entity.OT_To_Time = form["end_time"];
            entity.Descr = form["desc"];
            entity.Amount_Hour = double.Parse(form["amount_time"]);
            entity.Total_Price = double.Parse(form["total_price"]);
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();
            entity.User_Update = _helper.GetUserLoginId();
            
            _connection.SaveChanges();

            return Json(new { message = "OverTime updated" });
        }
        
        // show overtime of month
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetOverTimeOfMonth()
        {
            var date = DateTime.Now.ToString("yyyy-MM");
            var sum = _connection.tblOver_Time;
            
            var employees = sum.Where(o => o.OT_Date.StartsWith(date) && !o.Status.Contains("Reject")).LongCount();

            var hour = $"{sum.Where(o => o.OT_Date.StartsWith(date) && !o.Status.Contains("Reject")).Sum(o=>o.Amount_Hour):#,##0.##}";
            
            var padding = sum.Where(o => o.OT_Date.StartsWith(date) && o.Status.Contains("Pending")).LongCount(); 
            
            var rejected = sum.Where(o => o.OT_Date.StartsWith(date) && o.Status.Contains("Reject")).LongCount();

            return Json(new
            {
                employees,
                hour,
                padding,
                rejected
            });
        }
    }
}