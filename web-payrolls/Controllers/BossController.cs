using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using System.IO;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class BossController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();

        private readonly ClHelper _helper = new ClHelper();

        private readonly ContextProvider _provider = new ContextProvider(new ClHelper(), new DB_Connection());
        // GET: Boss
        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // Get Table 
        public ActionResult GetTable(
            int? page,
            int? pageSize,
            string code,
            string name,
            string phone,
            string status
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var boss = _connection
                       .GetManager(null, name, phone, code, status)
                       .ToList()
                       .ToPagedList(pageIndex, defaultPage);
            return PartialView(boss);
        }

        // Post Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Store(tblBoss entity, FormCollection form)
        {
            var code = form["Code"];
            var name = form["Name"];
            var phone = form["Phone"];
            var address = form["Address"];
            var status = form["Status"];
            var picture = form["Picture"];

            var existCode = _connection.tblBosses.Any(b => b.Code.Equals(code));
            if (existCode)
            {
                return Json(new { Code = "Code Already Exists" });
            }

            var existName = _connection.tblBosses.Any(b => b.Name.Equals(name));
            if (existName)
            {
                return Json(new { Name = "Name Already Exists" });
            }

            var existPhone = _connection.tblBosses.Any(b => b.Phone.Equals(phone));
            if (existPhone)
            {
                return Json(new { Phone = "Phone Already Exists" });
            }

            entity.Code = code;
            entity.Name = name;
            entity.Phone = phone;
            entity.Address = address;
            entity.Status = status;
            entity.Picture = picture;
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();

            _connection.tblBosses.Add(entity);
            _connection.SaveChanges();

            RemoveFile();

            return new JsonResult() { Data = "Created Successfully." };
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection form)
        {
            var code = form["Code_Edit"];
            var name = form["Name_Edit"];
            var phone = form["Phone_Edit"];
            var address = form["Address_Edit"];
            var status = form["Status_Edit"];
            var bossId = int.Parse(form["PK_Boss_Id"]);
            var photo = form["Picture_Edit"];

            var existCode = _connection.tblBosses.Any(b => b.Code.Equals(code) && !b.PK_Boss_Id.Equals(bossId));
            if (existCode)
            {
                return Json(new { Code = "Code Already Exists" });
            }

            var existName = _connection.tblBosses.Any(b => b.Name.Equals(name) && !b.PK_Boss_Id.Equals(bossId));
            if (existName)
            {
                return Json(new { Name = "Name Already Exists" });
            }

            var existPhone = _connection.tblBosses.Any(b => b.Phone.Equals(phone) && !b.PK_Boss_Id.Equals(bossId));
            if (existPhone)
            {
                return Json(new { Phone = "Phone Already Exists" });
            }

            var entity = _connection.tblBosses.Single(b => b.PK_Boss_Id == bossId);

            if (entity == null)
            {
                return Json(new { error = "Boss id not found." });
            }

            entity.Code = code;
            entity.Name = name;
            entity.Phone = phone;
            entity.Address = address;
            entity.Status = status;
            entity.Picture = photo;
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();

            _connection.SaveChanges();

            RemoveFile();

            return new JsonResult() { Data = "Updated Successfully." };
        }

        // filter code       
        public ActionResult AutoCode(string value)
        {
            var data = _connection
                .tblBosses
                .Where(b => b.Code.Contains(value))
                .Select(col => col.Code.Trim())
                .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // filter phone
        public ActionResult AutoPhone(string value)
        {
            var data = _connection
               .tblBosses
               .Where(b => b.Phone.Contains(value))
               .Select(col => col.Phone.Trim())
               .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // filter Name
        public ActionResult AutoName(string value)
        {
            var data = _connection
               .tblBosses
               .Where(b => b.Name.Contains(value))
               .Select(col => col.Name.Trim())
               .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // Remove File Name 
        public void RemoveFile()
        {
            var dir = HttpContext.Server.MapPath("~/Content/Uploads/Boss/");

            var filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/Boss/"));

            var fileInPath = new List<string>();

            foreach (var file in filePaths)
            {
                fileInPath.Add(Path.GetFileName(file));
            }
            // Remove
            foreach (var fileName in fileInPath)
            {

                var nameInDb = _connection.tblBosses.Where(s => s.Picture == fileName);

                if (nameInDb.Count() == 0)
                {
                    var fileInfo = new FileInfo(dir + fileName);

                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }
            }
        }
    }
}