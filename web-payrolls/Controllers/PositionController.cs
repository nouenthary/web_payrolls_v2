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
    public class PositionController : Controller
    {
        private DB_Connection db = new DB_Connection();
        private readonly ClHelper _clHelper = new ClHelper();

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
            int? did = null,
            string textSearch = ""
        )
        {
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            int defaultPage = (pageSize ?? 10);
            ViewBag.psize = defaultPage;

            GetNumberToDisplayDataPerPage();

            var positions = db
                .GetAllPositions(bid, cid, lid, did, textSearch)
               .ToList()
               .ToPagedList(pageIndex, defaultPage);
            return PartialView(positions); // when we return as PartialView we need to crate the function name the same as page in Views
        }
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
        public ActionResult AddPosition(tblPosition positionEntity, FormCollection formHelper)
        {
            var dept_id = int.Parse(formHelper["dept_id"]);
            var positionName = formHelper["positionName"];

            var isPositionNameExisting = db.tblPositions.Any(x => (x.FK_Depart_Id == dept_id) & (x.Pos_Name == positionName));
            if (isPositionNameExisting)
            {
                return Json(new { msg_existing = "Position is already existing" });
            }

            positionEntity.FK_Depart_Id = dept_id;
            positionEntity.Pos_Name = positionName;
            positionEntity.User_Update = _clHelper.GetUserLoginId();
            positionEntity.Date_Update = DateTime.Now.ToString("yyyy-MM-dd");
            positionEntity.Time_Update = DateTime.Now.ToString("hh:mm:ss");

            db.tblPositions.Add(positionEntity);
            db.SaveChanges();
            return Json(new { msg_success = "Saved successfully" });
        }

        // POST
        [HttpPost]
        public ActionResult UpdatePosition(tblPosition positionEntity, FormCollection formHelper)
        {
            var dept_id = int.Parse(formHelper["dept_id"]);
            var positionName = formHelper["positionName"];
            var position_id = int.Parse(formHelper["position_id"]);

            var isDeptNameExisting = db.tblPositions.Any(x => (x.FK_Depart_Id == dept_id) & (x.Pos_Name == positionName) & (x.PK_Pos_Id != position_id));
            if (isDeptNameExisting)
            {
                return Json(new { msg_existing = "Department is already existing" });
            }

            positionEntity.PK_Pos_Id = position_id;
            positionEntity.FK_Depart_Id = dept_id;
            positionEntity.Pos_Name = positionName;
            positionEntity.User_Update = _clHelper.GetUserLoginId();
            positionEntity.Date_Update = DateTime.Now.ToString("yyyy-MM-dd");
            positionEntity.Time_Update = DateTime.Now.ToString("hh:mm:ss");

            db.Entry(positionEntity).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { msg_success = "Updated Successfully", JsonRequestBehavior.AllowGet });
        }

        // Get Company by Boss Id
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

        // Get Location by Company Id
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

        // Get department by Company Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDepartment(FormCollection form)
        {
            int loc_id = int.Parse(form["loc_id"]);
            if (loc_id == 0)
            {
                return Json(null);
            }

            var departments = _clHelper.GetDepartmentByLocationId(loc_id);
            return Json(departments);
        }
    }
}