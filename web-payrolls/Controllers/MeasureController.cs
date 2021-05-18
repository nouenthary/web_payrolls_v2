using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class MeasureController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        private readonly ContextProvider  _provider = new ContextProvider(new ClHelper(), new DB_Connection());
        // GET
        public ActionResult Index()
        {
            // List Manager
            ViewBag.Manager = _connection
                .tblBosses
                .ToList();
            // Manager Id
            ViewData["ManagerId"] = _provider.ManagerId;
            return View();
        }
        
        // sup view
        public PartialViewResult GetTable
            (
                int? page,
                int? pageSize,
                int? bid = null,
                string measure = ""
            )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;
            
            var measures = _connection
                .GetAllMeasure(bid, measure)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);;
            ;
            return PartialView(measures);
        }
        
        // create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(FormCollection form)
        {
            var hodId = int.Parse(form["hodId"]);
            var measure = form["measure"];

            var entityMeasure = _connection.tblProduction_Measur;

            if (entityMeasure.Any(m=>m.FK_Boss_Id == hodId && m.Measu_Name == measure))
            {
                return Json(new {error = "Product measure already exist."});
            }
            
            var entity = new tblProduction_Measur()
            {
                FK_Boss_Id = hodId,
                Measu_Name = measure,
                Descr = form["desc"],
                User_Update = _helper.GetUserLoginId(),
                Date_Update = Constraint.GetDate(),
                Time_Update = Constraint.GetTime()
            };

            _connection.tblProduction_Measur.Add(entity);
            _connection.SaveChanges();
            
            return new JsonResult(){Data = "Created successfully."};
        }
        
        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection form)
        {
            var hodId = int.Parse(form["txtHodId"]);
            var id = int.Parse(form["txtMeasureId"]);
            var measure = form["txtMeasure"];
            
            var entityMeasure = _connection.tblProduction_Measur;
            
            if (entityMeasure.Any(m=> m.FK_Boss_Id == hodId && m.Measu_Name == measure && m.Pk_Measu_Id != id))
            {
                return Json(new {error = "Product measure already exist."});
            }

            var entity = entityMeasure.Single(m => m.Pk_Measu_Id == id);

            entity.FK_Boss_Id = hodId;
            entity.Measu_Name = measure;
            entity.Descr = form["txtDesc"];
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();
            
            _connection.SaveChanges();
            
            return new JsonResult(){Data = "Updated successfully."};
        }
    }
}