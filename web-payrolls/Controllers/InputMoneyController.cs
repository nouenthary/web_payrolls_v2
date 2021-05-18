using System;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using System.Web;
using System.IO;
using System.Collections.Generic;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class InputMoneyController : Controller
    {
        private static readonly DB_Connection Connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        // GET
        public ActionResult Index()
        {
            ViewBag.Manager = Connection.tblBosses.ToList();
            return View();
        }

        public PartialViewResult GetTable(
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null,
            int? lid = null,
            int? did = null,
            int? pid = null,
            string name = "",
            string id = "",
            string status = ""
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;

            ViewBag.pageSize = Constraint.PerPage;

            var data = Connection
                .GetAllInputMoney(bid, cid, lid, did, pid, name, id, status)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

            var sum = data.Sum(s => s.Total_Price);

            ViewData["Sum_Input_Money"] = String.Format("{0:#,##0.##}", sum);

            return PartialView(data);
        }

        // Add Input Money
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateInputMoney(tblInput_Money entity, FormCollection form) {
            try {
                var staffId = int.Parse(form["txt_sid_input_money"]);

                if (staffId < 1)
                {
                    throw new Exception("staff id not found.");
                }

                entity.FK_Staff_Id = staffId;
                entity.Total_Price = double.Parse(form["txt_total_money"]);
                entity.Status = Status.Pending.ToString();
                entity.Picture = form["txt_photo"];
                entity.Descr = form["txt_desc"];
                entity.User_Update = _helper.GetUserLoginId();
                entity.Date_Update = Constraint.GetDate();
                entity.Time_Update = Constraint.GetTime();

                Connection.tblInput_Money.Add(entity);
                Connection.SaveChanges();

                RemoveFile();

                return Json( new { mesage = "Created successfully!", id = entity.PK_Input_Money_Id });
            } catch (Exception ex) {
                return Json(new { error = ex});
            }            
        }

        // Confirm 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ConfirmInputMoney(FormCollection form) {
            try {
                var inputMoneyId = int.Parse(form["inputMoneyId"]);
                var status = form["status"];

                var entity = Connection.tblInput_Money.Single(i => i.PK_Input_Money_Id == inputMoneyId);            
                if (entity == null) throw new Exception("input money id not found.");
                
                entity.User_Confirm = _helper.GetUserLoginId();
                entity.Date_Confirm = Constraint.GetDate();
                entity.Time_Confirm = Constraint.GetTime();
                entity.Status = status;

                Connection.SaveChanges();

                return new JsonResult() { Data = "Updated successfully!" };
            }
            catch (Exception ex) {
                return Json(new { error = ex });
            }
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateInputMoney(FormCollection form) {
            try {
                var inputId = int.Parse(form["txtInputId"]);
                var price = double.Parse(form["txtTotalMoney"]);
                var picture = form["txtPhoto"];
                var desc = form["txtDesc"];

                var entity = Connection.tblInput_Money.Single(i => i.PK_Input_Money_Id == inputId);

                if (entity == null) throw new Exception("input money id not found.");

                entity.Total_Price = price;
                entity.Picture = picture;
                entity.Descr = desc;
                entity.User_Update = _helper.GetUserLoginId();
                entity.Date_Update = Constraint.Date;
                entity.Time_Update = Constraint.Time;

                Connection.SaveChanges();

                RemoveFile();

                return new JsonResult() { Data = "Updated successfully!" };
            }
            catch (Exception ex) {
                return Json(new { error = ex});
            }
        }

        // Remove File Name 
        public void RemoveFile()
        {
            var dir = HttpContext.Server.MapPath("~/Content/Uploads/InputMoney/");

            var filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/InputMoney/"));

            var fileInPath = new List<string>();

            foreach (var file in filePaths)
            {
                fileInPath.Add(Path.GetFileName(file));
            }
            // Remove
            foreach (var fileName in fileInPath)
            {
                var nameInDb =Connection.tblInput_Money.Where(s => s.Picture == fileName);

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