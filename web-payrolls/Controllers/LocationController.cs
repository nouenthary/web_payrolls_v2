using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Models;
using System.Data.Entity;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class LocationController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        // GET: Location
        public ActionResult Index()
        {                       
            ViewBag.Manager = _connection.tblBosses.ToList();
            ViewBag.Company = _connection.tblCompanies.ToList();
         
            return View();
        }

        public ActionResult GetTable(
            int? page,
            int? pageSize, 
            int? bid = null, 
            int? cid = null
        ){

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var location = _connection
               .GetAllLocation(bid,cid, "")
               .ToList()
               .ToPagedList(pageIndex, defaultPage);          
            return PartialView(location);
        }
      
        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StoreOrEdit(tblLocation entity, FormCollection form)
        {           
            var locName = form["Loc_Name"];
            var desc = form["Descr"];
            var compId = int.Parse(form["FK_Comp_Id"]);

            var existLocation = _connection.tblLocations.Any(l => l.Loc_Name.Equals(locName) && l.FK_Comp_Id == compId);
            if (existLocation)
            {
                return Json(new { Loc_Name = "Location is already" });
            }

            entity.FK_Comp_Id = compId;
            entity.Loc_Name = locName;
            entity.Descr = desc;
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();

            _connection.tblLocations.Add(entity);
            _connection.SaveChanges();

            return Json(new { message = "Created Successfully." });
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(tblLocation entity, FormCollection form)
        {           
            var locName = form["Loc_Name_Edit"];
            var desc = form["Descr_Edit"];
            var compId = int.Parse(form["FK_Comp_Id_Edit"]);
            var locationId = int.Parse(form["PK_Location_Id"]);

            var existName = _connection
                .tblLocations
                .Any(l => l.Loc_Name.Equals(locName) && l.FK_Comp_Id == compId && !l.PK_Location_Id.Equals(locationId));

            if (existName) {
                return Json(new { Loc_Name = "Location is already" });
            }         

            entity.PK_Location_Id = locationId;
            entity.FK_Comp_Id = compId;
            entity.Loc_Name = locName;
            entity.Descr = desc;
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();

            _connection.Entry(entity).State = EntityState.Modified;
            _connection.SaveChanges();

            return Json(new { message = "Update Successfully." });
        }

        // Get Company
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCompany(FormCollection form) {
            if (form["boss_id"] == "")
            {
                return Json(null);
            }
            var bossId = int.Parse(form["boss_id"]);
            var company = _connection
                .tblCompanies
                .Where(c => c.FK_Boss_Id == bossId)
                .Select(c => new {c.PK_Comp_Id , c.Comp_Name})
                .ToList();
            return Json(company);
        }

        // Get Location
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetLocation(FormCollection form)
        {
            if (form["com_id"] == "") {
                return Json(null);
            }
            var comId = int.Parse(form["com_id"]);                       
            var location = _connection
                .tblLocations
                .Where(l => l.FK_Comp_Id == comId)
                .Select(l => new { l.PK_Location_Id, l.Loc_Name })
                .ToList();
            return Json(location);
        }

        // Get Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDepartment(FormCollection form) {

            if (form["loc_id"] == "") {
                return Json(null);
            }

            var locId = int.Parse(form["loc_id"]);
            var department = _connection
                .tblDepartments
                .Where(d => d.FK_Loc_Id == locId)
                .Select(d => new {d.PK_Depart_Id, d.Deppart_Name })
                .ToList();

            return Json(department);
        }

        // Get Position
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPosition(FormCollection form) {

            if (form["dept_id"] == "") {
                return Json(null);
            }

            var deptId = int.Parse(form["dept_id"]);
            var position = _connection
                .tblPositions
                .Where(p => p.FK_Depart_Id == deptId)
                .Select(p => new { p.PK_Pos_Id, p.Pos_Name })
                .ToList();

            return Json(position);
        }
        
        // get staff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStaff(FormCollection form)
        {
            if (form["post_id"] == "") {
                return Json(null);
            }
            
            var positionId = int.Parse(form["post_id"]);
            var staffs = _connection
                .tblStaffs   
                .Where(s => s.FK_Pos_Id == positionId)
                .Select(s=> new{s.PK_Staff_Id, s.Staff_Name})
                .ToList();
            
            return Json(staffs ,JsonRequestBehavior.AllowGet);
        }
        
    }
}