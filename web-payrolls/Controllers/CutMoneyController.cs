using System;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class CutMoneyController : Controller
    {        
        private readonly DB_Connection _connection = new DB_Connection();
        
        private readonly ClHelper _helper = new ClHelper();
        // GET: CutMoney
        public ActionResult Index()
        {        
            ViewBag.GetBoss = _connection.tblBosses.ToList();
            ViewBag.DeductMoney = Constraint.DeductMoneyType;
            return View();
        }

        // Partial View
        public ActionResult GetTable(
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null
        )
        {           
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            
            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var data = _connection
               .GetAllCutMoney(bid, cid)
               .ToList()
               .OrderByDescending(c => c.PK_Cut_Id)
               .ToPagedList(pageIndex, defaultPage);

            return PartialView(data);
        }
       
        // Add Cut Money
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(tblCut_Money entity, FormCollection form)
        {
            var companyId = int.Parse(form["company_id"]);
            var leaveType = form["leave_type"];
            var cutMoney = double.Parse(form["cut_money"]);
            var txtDesc = form["txt_desc"];

            var compare = _connection.tblCut_Money.Any(c => c.FK_Com_Id == companyId && c.Lev_Name == leaveType);

            if (compare) {
                return Json(new { error = "Leave Already Exist."});
            }

            entity.FK_Com_Id = companyId;
            entity.Lev_Name = leaveType;
            entity.Cut_Percent = cutMoney;
            entity.Descr = txtDesc;
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();

            _connection.tblCut_Money.Add(entity);
            _connection.SaveChanges();

            return new JsonResult() { Data  = "Created Successfully." };
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StoreOrEdit(FormCollection form)
        {
            var cutId = int.Parse(form["cut-money-id"]);
            var comId = int.Parse(form["company-id-edit"]);           
            var percent = double.Parse(form["cut-money-edit"]);
            var leave = form["leave-type-edit"];
            var desc = form["desc-edit"];

            var check = _connection.tblCut_Money.Any(c => c.FK_Com_Id == comId && c.Lev_Name == leave && c.PK_Cut_Id != cutId);

            if (check) {
                return Json(new { Leave = "Leave Already Exist." });
            }

            var entity = _connection.tblCut_Money.Single(c => c.PK_Cut_Id == cutId);               
                    
            entity.Lev_Name = leave;
            entity.Cut_Percent = percent;
            entity.Descr = desc;
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();

            _connection.SaveChanges();

            return new JsonResult() { Data = "Update Successfully!" };
        }
    }
}