using System;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class TaxController : Controller
    {
        private static readonly DB_Connection Connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        // GET: Tax
        public ActionResult Index()
        {
            ViewBag.Manager = Connection.tblBosses.ToList();
            return View();
        }

        // get Table View
        public PartialViewResult GetTable(
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

           var data = Connection
                .GetAllTax(bid, cid)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

            return PartialView(data);
        }      

        // Create Tax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddTax(tblTax tax, FormCollection form)
        {
            try
            {
                var companyId = int.Parse(form["FK_Comp_Id"]);
                if (companyId < 0) 
                {
                    throw new Exception("create failed can`t identify number company.");
                }
                var company = Connection.tblCompanies.Any(c=> c.PK_Comp_Id == companyId);
                if (company == false) {
                    throw new Exception(companyId + " = company not found.");
                }

                tax.FK_Comp_Id = int.Parse(form["FK_Comp_Id"]);
                tax.Tax_Percent = int.Parse(form["txt_tax"]);
                tax.Start_Rank_Salary = int.Parse(form["txt_start"]);
                tax.End_Rank_Salary = double.Parse(form["txt_end"]);
                tax.Decrease_Tax = double.Parse(form["txt_decrease"]);
                tax.Decrease_Tax_From_Fammily = int.Parse(form["txt_family"]);
                tax.Decrease_Tax_Foreign = int.Parse(form["txt_foreign"]);
                tax.User_Update = _helper.GetUserLoginId();
                tax.Date_Update = Constraint.GetDate();
                tax.Time_Update = Constraint.GetTime();

                Connection.tblTaxes.Add(tax);
                Connection.SaveChanges();

                return new JsonResult() { Data = "Created successfully." };
            }
            catch (Exception e) {
                return Json(new { error = e});
            }           
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection form)
        {
            try {
                var taxId = int.Parse(form["tax_id_edit"]);

                var tax = Connection.tblTaxes.Single(T => T.PK_Tax_Id == taxId);

                if (tax == null) throw new Exception(taxId + " = id not found.");
              
                tax.Tax_Percent = int.Parse(form["txt_tax_edit"]);
                tax.Start_Rank_Salary = int.Parse(form["txt_start_edit"]);
                tax.End_Rank_Salary = double.Parse(form["txt_end_edit"]);
                tax.Decrease_Tax = double.Parse(form["txt_decrease_edit"]);
                tax.Decrease_Tax_From_Fammily = int.Parse(form["txt_family_edit"]);
                tax.Decrease_Tax_Foreign = int.Parse(form["txt_foreign_edit"]);
                tax.User_Update = _helper.GetUserLoginId();
                tax.Date_Update = Constraint.GetDate();
                tax.Time_Update = Constraint.GetTime();

                Connection.SaveChanges();

                return new JsonResult() { Data = "Updated successfully!" };
            }
            catch (Exception e)
            {
                return Json(new { error = e});
            }           
        }

        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteTax(int taxId)
        {
            try {
                var tax = Connection.tblTaxes.Single(T => T.PK_Tax_Id == taxId);

                if (tax == null) throw new Exception(taxId + " = id not found.");
               
                Connection.tblTaxes.Remove(tax);
                Connection.SaveChanges();

                return new JsonResult()
                {
                    Data = "Deleted successfully!"                    
                };
            }
            catch (Exception ex)
            {
                return Json(new {error = ex });
            }          
        }
    }
}