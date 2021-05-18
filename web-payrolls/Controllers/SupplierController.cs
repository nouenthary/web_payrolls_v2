using System;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class SupplierController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        
        private readonly ClHelper _helper = new ClHelper();
        // GET: Supplier
        public ActionResult Index()
        {
            ViewBag.GetBoss = _connection.tblBosses.ToList();
            return View();
        }

        public ActionResult GetTable(
            string name, 
            string phone,
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null,
            int? lid = null
        ){
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var supplier = _connection
                .GetAllSupplier(bid, cid, lid, name, phone)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

            return PartialView(supplier);
        }    

        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(tblSupplyer entity, FormCollection form)
        {                  
            var supplier = form["supplier"];
            var phone = form["phone"];
            var address = form["address"];
            var locationId = int.Parse(form["sup_location"]);

            var supplierName = _connection.tblSupplyers.Any(s => s.FK_Loc_Id == locationId && s.Name == supplier);

            if (supplierName) {
                return Json(new { supplier = "Supplier Already Exists" });
            }

            var supplierPhone = _connection.tblSupplyers.Any(s => s.FK_Loc_Id == locationId && s.Phone == phone);

            if (supplierPhone) {
                return Json(new { phone = "Phone Already Exists" });
            }

            entity.FK_Loc_Id = locationId;
            entity.Name = supplier;
            entity.Phone = phone;
            entity.Address = address;
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();

            _connection.tblSupplyers.Add(entity);
            _connection.SaveChanges();

            return new JsonResult() { Data = "Created Successfully." };
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection form)
        {           
            var id = int.Parse(form["supp-id-edit"]);
            var supplier = form["supplier-edit"];
            var phone = form["phone-edit"];
            var address = form["address-edit"];
            var locId = int.Parse(form["sup_location_edit"]);
          
            var supplierName = _connection.tblSupplyers.Any(s => s.FK_Loc_Id == locId && s.Name == supplier && s.PK_Supp_Id != id);

            if (supplierName)
            {
                return Json(new { supplier = "Supplier Already Exists" });
            }

            var supplierPhone = _connection.tblSupplyers.Any(s => s.FK_Loc_Id == locId && s.Phone == phone && s.PK_Supp_Id != id);

            if (supplierPhone)
            {
                return Json(new { phone = "Phone  Already Exists" });
            }

            var entity = _connection.tblSupplyers.Single(s => s.PK_Supp_Id == id);

            entity.Name = supplier;
            entity.Phone = phone;
            entity.Address = address;
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();
            
            _connection.SaveChanges();

            return new JsonResult() { Data = "Update Successfully." };
        }

        // filter Name
        public ActionResult AutoName(string value)
        {
            var data = _connection
               .tblSupplyers
               .Where(s => s.Name.Contains(value))
               .Select(s => s.Name.Trim())
               .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // filter Phone
        public ActionResult AutoPhone(string value)
        {
            var data = _connection
               .tblSupplyers
               .Where(s => s.Phone.Contains(value))
               .Select(s => s.Phone.Trim())
               .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}