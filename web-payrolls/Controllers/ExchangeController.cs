using System;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class ExchangeController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();

        private readonly ClHelper _helper = new ClHelper();
        // GET: Exchange
        public ActionResult Index()
        {         
            ViewBag.GetBoss = _connection.tblBosses.ToList();
            return View();
        }

        // View Partial
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

            var exchange = _connection
               .GetExchange(bid, cid)
               .ToList()
               .ToPagedList(pageIndex, defaultPage);

            return PartialView(exchange);
        }            

        // Store
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StoreOrEdit(tblExchange entity, FormCollection form)
        {
            var companyId = int.Parse(form["ex_company"]);
            var name = form["ex_name"];
            var rate = form["ex_rate"];

            var entityName = _connection.tblExchanges.Any(ex => ex.FK_Com_Id == companyId && ex.Name == name);
            if (entityName) {
                return Json(new { name = "Name Already Exist" });
            }

            entity.FK_Com_Id = companyId;          
            entity.Name = name;
            entity.Rate = Convert.ToDouble(rate);
            entity.User_Update = _helper.GetUserLoginId().ToString();
            entity.Time_Update = Constraint.GetTime();

            _connection.tblExchanges.Add(entity);
            _connection.SaveChanges();

            return new JsonResult() { Data = "Created Successfully." };
        }

        // Update
        [HttpPost]
        public JsonResult Update(FormCollection form)
        {
            var exchangeId = int.Parse(form["exchange_id"]);
            var companyId = int.Parse(form["ex_company_edit"]);
            var name = form["ex_name_edit"];
            var rate = form["ex_rate_edit"];

            var entityName = _connection.tblExchanges.Any(ex => ex.FK_Com_Id == companyId && ex.Name == name && ex.PK_Exchang_Id != exchangeId);

            if (entityName) {
                return Json(new { name = "Name Already Exist" });
            }

            var entity = _connection.tblExchanges.Single(ex => ex.PK_Exchang_Id == exchangeId);
            
            if (entity == null) {
                return Json(new { error = exchangeId + " = id not found." });
            }

            entity.Name = name;
            entity.Rate = double.Parse(rate);
            entity.User_Update = _helper.GetUserLoginId().ToString();
            entity.Time_Update = Constraint.GetTime();

            _connection.SaveChanges();

            return new JsonResult() { Data = "Updated Successfully." };
        }

        // convert 
        public string ConvertToRate(string rate)
        {
            var value = "";
            char[] regix = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };

            for (int i = 0; i < rate.Length; i++)
            {
                foreach (char ch in regix)
                {
                    if (rate[i] == ch)
                    {
                        value += rate[i];
                    }
                }
            }
            return value;
        }
    }
}