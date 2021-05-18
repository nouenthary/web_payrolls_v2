using System;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using System.Data.Entity;
using System.Collections.Generic;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class DepartmentController : Controller
    {
        private DB_Connection db = new DB_Connection();
        private readonly ClHelper _clHelper = new ClHelper();
        // GET: Department
        public ActionResult Index()
        {
            ViewBag.Manager = db.tblBosses.ToList();

            return View();
        }
        public ActionResult GetTable(
            int? page, 
            int? pageSize, 
            int? bid = null, 
            int? cid = null, 
            int? lid = null, 
            string textSearch = ""
        ){
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            int defaultPage = (pageSize ?? 10);
            ViewBag.psize = defaultPage;

            GetNumberToDisplayDataPerPage();

            var department = db
               .GetAllDepartments(bid, cid, lid, textSearch)
               .ToList()
               .ToPagedList(pageIndex, defaultPage);
            return PartialView(department); // when we return as PartialView we need to crate the function name the same as page in Views
        }
        // Get number to display record per page
        public void GetNumberToDisplayDataPerPage()
        {
            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() { Value="10", Text= "10" },
                new SelectListItem() { Value="20", Text= "20" },
                new SelectListItem() { Value="50", Text= "50" },
                new SelectListItem() { Value="100", Text= "100" },
                new SelectListItem() { Value="5000", Text= "ALL" },
            };
        }

        [HttpPost]
        public ActionResult AddDepartment(tblDepartment deptEntity, FormCollection formHelper)
        {
            var location_id = int.Parse(formHelper["location_id"]);
            var deptName = formHelper["deptName"];
            int staff_transaction = int.Parse(formHelper["staff_transaction"]);
            var description = formHelper["description"];

            var isDeptNameExisting = db.tblDepartments.Any(x => (x.FK_Loc_Id == location_id) & (x.Deppart_Name == deptName));
            if (isDeptNameExisting)
            {
                return Json(new { msg_existing = "Department is already existing" });
            }

            deptEntity.FK_Loc_Id = location_id;
            deptEntity.Deppart_Name = deptName;
            deptEntity.Descr = description;
            deptEntity.User_Update = _clHelper.GetUserLoginId();
            deptEntity.Date_Update = DateTime.Now.ToString("yyyy-MM-dd");
            deptEntity.Time_Update = DateTime.Now.ToString("hh:mm:ss");
            deptEntity.Number_Staff_Transaction = staff_transaction;

            db.tblDepartments.Add(deptEntity);
            db.SaveChanges();

            return Json(new { msg_success = "Saved successfully", JsonRequestBehavior.AllowGet });
        }

        // POST
        [HttpPost]
        public ActionResult Update(tblDepartment deptEntity, FormCollection formHelper)
        {
            var location_id = int.Parse(formHelper["location_id"]);
            var staff_transaction = int.Parse(formHelper["staff_transaction"]);
            var deptName = formHelper["deptName"];
            var description = formHelper["description"];
            var dept_id = int.Parse(formHelper["dept_id"]);

            var isDeptNameExisting = db.tblDepartments.Any(x => (x.FK_Loc_Id == location_id) & (x.Deppart_Name == deptName) & (x.PK_Depart_Id != dept_id));
            if (isDeptNameExisting)
            {
                return Json(new { msg_existing = "Department is already existing" });
            }

            deptEntity.PK_Depart_Id = dept_id;
            deptEntity.FK_Loc_Id = location_id;
            deptEntity.Number_Staff_Transaction = staff_transaction;
            deptEntity.Deppart_Name = deptName;
            deptEntity.Descr = description;
            deptEntity.User_Update = _clHelper.GetUserLoginId();
            deptEntity.Date_Update = DateTime.Now.ToString("yyyy-MM-dd");
            deptEntity.Time_Update = DateTime.Now.ToString("hh:mm:ss");

            db.Entry(deptEntity).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { msg_success = "Updated Successfully", JsonRequestBehavior.AllowGet });
        }
        // Get Company
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCompany(FormCollection form)
        {
            int boss_id = int.Parse(form["boss_id"]);
            if (boss_id == 0)
            {
                return Json(null);
            }

            var companies = _clHelper.GetCompanyByBossId(boss_id);
            
            return Json(companies);
        }

        // Get Location
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetLocation(FormCollection form)
        {
            int comp_id = int.Parse(form["comp_id"]);
            if (comp_id == 0)
            {
                return Json(null);
            }

            var locations = _clHelper.GetLocationByCompanyId(comp_id);
            return Json(locations);
        }
    }
}