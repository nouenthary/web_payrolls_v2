using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class RosterController : Controller
    {        
        private readonly DB_Connection _connection = new DB_Connection();

        private readonly ClHelper _helper = new ClHelper();
        // GET: Roster
        public ActionResult Index()
        {                                   
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var userId = _helper.GetUserLoginId();
                                    
            var countRecord= _connection
                .GetRoster(null, null, null, null, null, "", "", "", year, month)
                .ToList()
                .Count;
           
            if (countRecord == 0)
            {
                _connection.AddNextRoster(Convert.ToInt32(year), Convert.ToInt32(month), userId);
                _connection.SaveChanges();
            }

            ViewBag.LeaveType = _connection.tblCut_Money.Where(c => c.FK_Com_Id == 1);
            ViewBag.Boss = _connection.tblBosses.ToArray();                                                     
            return View();
        }

        // Table view 
        public ActionResult GetTable(
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null,
            int? lid = null,
            int? did = null,
            int? pid = null,            
            int? year = null,
            int? month = null,
            string name = "",
            string id = "",
            string status = ""
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;           

            ViewBag.PageSize = Constraint.PerPage;

           var roster = _connection
                .GetRoster(
                    bid, cid, lid, did, pid, name, id, status, year, month
                ).ToList();

            if (roster.Count < 1 && status == "Enable") {
                AddNextRoster(year, month);
            }

            return PartialView(roster.ToPagedList(pageIndex, defaultPage));
        }
        
        // Add Next Roster
        public void AddNextRoster(int? year, int? month) {            
            var day = DateTime.Now.Day;
            var userId = 1;

            if (month == 2 && DateTime.Now.Day > 28)
            {
                day = 28;
            }

            var futureDate = DateTime.Parse(month + "/" + day + "/" + year);

            if (futureDate.Date <= DateTime.Now.Date) return;
            _connection.AddNextRoster(year, month, userId);
            _connection.SaveChanges();
        }

        // set roster
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetRoster(FormCollection form)
        {            
            var start = int.Parse(form["start_date"]);
            var end = int.Parse(form["end_date"]);
            var time = form["time"];
            var rosterId = int.Parse(form["roster_id"]);

            var roster = _connection
                .tblRosters
                .Where(r => r.PK_Ros_Id == rosterId)
                .ToArray()
                .FirstOrDefault();

            if (roster == null) {
                return Json(new {error = "Set Roster Time Field." });
            }

            var col = new Dictionary<string, string>
            {
                {"DAY1", roster.DAY1},
                {"DAY2", roster.DAY2},
                {"DAY3", roster.DAY3},
                {"DAY4", roster.DAY4},
                {"DAY5", roster.DAY5},
                {"DAY6", roster.DAY6},
                {"DAY7", roster.DAY7},
                {"DAY8", roster.DAY8},
                {"DAY9", roster.DAY9},
                {"DAY10", roster.DAY10},
                {"DAY11", roster.DAY11},
                {"DAY12", roster.DAY12},
                {"DAY13", roster.DAY13},
                {"DAY14", roster.DAY14},
                {"DAY15", roster.DAY15},
                {"DAY16", roster.DAY16},
                {"DAY17", roster.DAY17},
                {"DAY18", roster.DAY18},
                {"DAY19", roster.DAY19},
                {"DAY20", roster.DAY20},
                {"DAY21", roster.DAY21},
                {"DAY22", roster.DAY22},
                {"DAY23", roster.DAY23},
                {"DAY24", roster.DAY24},
                {"DAY25", roster.DAY25},
                {"DAY26", roster.DAY26},
                {"DAY27", roster.DAY27},
                {"DAY28", roster.DAY28},
                {"DAY29", roster.DAY29},
                {"DAY30", roster.DAY30},
                {"DAY31", roster.DAY31}
            };

            var off = 0;
            var ph = 0;
            var al = 0;
            var sic = 0;
            var upl = 0;
            var ab = 0;
            var cm = 0;

            for (var i = start; i <= end; i++)
            {
                string[] leave = { "OFF", "AL", "SIC", "PH", "UPL", "CM", "AB" };
                
                foreach (var item in leave)
                {
                    if (col["DAY" + i] != item) continue;
                    if (item == leave[0])
                        off++;
                    else if (item == leave[1])
                        al++;
                    else if (item == leave[2])
                        sic++;
                    else if (item == leave[3])
                        ph++;
                    else if (item == leave[4])
                        upl++;
                    else if (item == leave[5])
                        cm++;
                    else if (item == leave[6])
                        ab++;
                }
            }


            if (ModelState.IsValid)
            {
                for (var i = start; i <= end; i++)
                {
                    _connection.Database.ExecuteSqlCommand(
                            "UPDATE tblROSTER SET" 
                            + " DAY" + i + " = " + time 
                            + ", User_Update = " + 1 
                            + ", Date_Update = '" + Constraint.GetDate() + "'"
                            + ", Time_Update = '" + Constraint.GetTime() + "'"
                            + " WHERE PK_ROS_ID = " + rosterId
                        );
                }

                roster.OFF = (int.Parse(roster.OFF) - off).ToString();
                roster.PH = (int.Parse(roster.PH) - ph).ToString();
                roster.AL = (int.Parse(roster.AL) - al).ToString();
                roster.SIC = (int.Parse(roster.SIC) - sic).ToString();
                roster.AB = (int.Parse(roster.AB) - ab).ToString();
                roster.UPL = (int.Parse(roster.UPL) - upl).ToString();
                roster.CM = (int.Parse(roster.CM) - cm).ToString();
                roster.User_Update = _helper.GetUserLoginId();
                
                _connection.SaveChanges();
            }

            return new JsonResult() {Data = "Roster updated successfully."};
        }

        //set roster leave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetTimeLeave(FormCollection form)
        {            
            var start = int.Parse(form["start_date_leave"]);
            var end = int.Parse(form["end_date_leave"]);
            var leaveType = form["leave_type"];
            var rosterId = int.Parse(form["rosters_id"]);
            var staffId = int.Parse(form["staff_id"]);
            var years = int.Parse(form["year_roster"]);

            var leaveDay = end - start + 1;

            var sumLeave = _connection
                .GetSumTypeLeave(staffId, years, leaveType)
                .FirstOrDefault();

            var salaryType = _connection
                .tblSalary_Type
                .Single(s => s.FK_Sta_Id == staffId);
            
            var amountDay = sumLeave + leaveDay;
         

            switch (leaveType)
            {
                case "OFF" when amountDay > salaryType.Off_Day:
                    return Json(new { error = GetMessageLeaveType("OFF",salaryType.Off_Day) });
                case "PH" when amountDay > salaryType.PH_Day:
                    return Json(new { error = GetMessageLeaveType("PH",salaryType.PH_Day) });
                case "AL" when amountDay > salaryType.AL_Day:
                    return Json(new { error = GetMessageLeaveType("AL",salaryType.AL_Day)  });
                case "SIC" when amountDay > salaryType.Sick:
                    return Json(new { error = GetMessageLeaveType("SIC",salaryType.Sick) });
                case "AB" when amountDay > salaryType.AB:
                    return Json(new { error = GetMessageLeaveType("AB",salaryType.AB) });
                case "UPL" when amountDay > salaryType.UPL:
                    return Json(new { error = GetMessageLeaveType("UPL",salaryType.UPL)  });
                case "CM" when amountDay > salaryType.CM:
                    return Json(new { error = GetMessageLeaveType("CM",salaryType.CM) });
            }

            var roster = _connection
                .tblRosters
                .Where(r => r.PK_Ros_Id == rosterId)
                .ToArray()
                .FirstOrDefault();

            if (roster == null) {
                return Json(new {error = "Roster id not found." });
            }

            var col = new Dictionary<string, string>
            {
                {"DAY1", roster.DAY1},
                {"DAY2", roster.DAY2},
                {"DAY3", roster.DAY3},
                {"DAY4", roster.DAY4},
                {"DAY5", roster.DAY5},
                {"DAY6", roster.DAY6},
                {"DAY7", roster.DAY7},
                {"DAY8", roster.DAY8},
                {"DAY9", roster.DAY9},
                {"DAY10", roster.DAY10},
                {"DAY11", roster.DAY11},
                {"DAY12", roster.DAY12},
                {"DAY13", roster.DAY13},
                {"DAY14", roster.DAY14},
                {"DAY15", roster.DAY15},
                {"DAY16", roster.DAY16},
                {"DAY17", roster.DAY17},
                {"DAY18", roster.DAY18},
                {"DAY19", roster.DAY19},
                {"DAY20", roster.DAY20},
                {"DAY21", roster.DAY21},
                {"DAY22", roster.DAY22},
                {"DAY23", roster.DAY23},
                {"DAY24", roster.DAY24},
                {"DAY25", roster.DAY25},
                {"DAY26", roster.DAY26},
                {"DAY27", roster.DAY27},
                {"DAY28", roster.DAY28},
                {"DAY29", roster.DAY29},
                {"DAY30", roster.DAY30},
                {"DAY31", roster.DAY31}
            };

            var off = 0;
            var ph = 0;
            var al = 0;
            var sic = 0;
            var upl = 0;
            var ab = 0;
            var cm = 0;

            for (var i = start; i <= end; i++)
            {
                string[] leave = { "OFF", "AL", "SIC", "PH", "UPL", "CM", "AB" };
                
                foreach (var item in leave)
                {
                    if (col["DAY" + i] != item) continue;
                    
                    if (item == leave[0])
                        off++;
                    else if (item == leave[1])
                        al++;
                    else if (item == leave[2])
                        sic++;
                    else if (item == leave[3])
                        ph++;
                    else if (item == leave[4])
                        upl++;
                    else if (item == leave[5])
                        cm++;
                    else if (item == leave[6])
                        ab++;
                }                              
            }
           
            // old
            var countDay = 0;
            for (var i = start; i <= end; i++)
            {
                countDay++;
                _connection.Database.ExecuteSqlCommand("UPDATE tblROSTER SET DAY" + i + " = '" + leaveType + "' WHERE PK_Ros_Id =" + rosterId);
            }
            
            switch (leaveType)
            {
                case "OFF":
                    roster.OFF = (int.Parse(roster.OFF) + countDay).ToString();
                    break;
                case "PH":
                    roster.PH = (int.Parse(roster.PH) + countDay ).ToString();
                    break;
                case "AL":
                    roster.AL = (int.Parse(roster.AL) + countDay ).ToString();
                    break;
                case "SIC":
                    roster.SIC = (int.Parse(roster.SIC) +countDay ).ToString();
                    break;
                case "AB":
                    roster.AB = (int.Parse(roster.AB) + countDay).ToString();
                    break;
                case "UPL":
                    roster.UPL = (int.Parse(roster.UPL) + countDay).ToString();
                    break;
                case "CM":
                    roster.CM = (int.Parse(roster.CM) + countDay).ToString();
                    break;
            }

            roster.OFF = (int.Parse(roster.OFF) - off).ToString();
            roster.PH = (int.Parse(roster.PH) - ph).ToString();
            roster.AL = (int.Parse(roster.AL) - al).ToString();
            roster.SIC = (int.Parse(roster.SIC) - sic).ToString();
            roster.AB = (int.Parse(roster.AB) - ab).ToString();
            roster.UPL = (int.Parse(roster.UPL) - upl).ToString();
            roster.CM = (int.Parse(roster.CM) - cm).ToString();

            roster.User_Update = _helper.GetUserLoginId();
            roster.Date_Update = Constraint.GetDate();
            roster.Time_Update = Constraint.GetTime();

            _connection.SaveChanges();

            return new JsonResult() { Data = "Roster updated successfully."};
        }

        // Get List All Leave Type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAllLeave(FormCollection form)
        {
            var sid = int.Parse(form["staff_id"]);
            var year = int.Parse(form["year"]);

            var salaryType = _connection
                .GetCanLeave(sid)
                .FirstOrDefault();

            if (salaryType == null) {
                return Json(new { error = "salaryType not found." });
            }

            var roster = _connection
                .GetSumLeave(sid, year)
                .FirstOrDefault();
            if (roster  == null) {
                return Json(new { error = "roster not found." });
            }

            var off = Convert.ToInt32(salaryType.ST_OFF) - Convert.ToInt32(roster.R_OFF);
            var ph = Convert.ToInt32(salaryType.ST_PH) - Convert.ToInt32(roster.R_PH);
            var sic = Convert.ToInt32(salaryType.ST_SIC) - Convert.ToInt32(roster.R_SIC);
            var al = Convert.ToInt32(salaryType.ST_AL) - Convert.ToInt32(roster.R_AL);
            var cm = Convert.ToInt32(salaryType.ST_SM) - Convert.ToInt32(roster.R_CM);
            var upl = Convert.ToInt32(salaryType.ST_UPL) - Convert.ToInt32(roster.R_UPL);
            var ab = Convert.ToInt32(salaryType.ST_AB) - Convert.ToInt32(roster.R_AB);

            var rests = new Dictionary<string, object>
            {
                {"REST_OFF", off},
                {"REST_PH", ph},
                {"REST_SIC", sic},
                {"REST_AL", al},
                {"REST_CM", cm},
                {"REST_UPL", upl},
                {"REST_AB", ab}
            };

            return Json(new { all = salaryType, use  = roster , rest = rests });
        }

        public string GetMessageLeaveType(string leaveType, object day)
        {
            return "You can add more " + leaveType + " day bigger " + day;
        }
    }
} 