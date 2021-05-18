using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using System.Data.Entity;
using System.IO;
using web_payrolls.Helpers;
using System.Collections.Generic;

namespace web_payrolls.Controllers
{
    public class CompanyController : Controller
    {
        private DB_Connection db = new DB_Connection();
        private readonly ClHelper _clHelper = new ClHelper();
        private readonly ImgUploadHelper _imgUploadHelper = new ImgUploadHelper();

        // GET: Company
        public ActionResult Index()
        {
            ViewBag.Manager = db.tblBosses.ToList();

            return View();
        }
        public ActionResult GetTable(
            int? page,
            int? pageSize,
            int? bid = null,
            string textSearch = ""
        )
        {
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            int defaultPage = (pageSize ?? 10);
            ViewBag.psize = defaultPage;

            GetNumberToDisplayDataPerPage();

            var companies = db
               .GetAllCompanies(bid, textSearch)
               .ToList()
               .ToPagedList(pageIndex, defaultPage);
            return PartialView(companies); // when we return as PartialView we need to crate the function name the same as page in Views
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

        // GET: Company/Create
        [HttpPost]
        public ActionResult AddCompany(tblCompany companyEntity, FormCollection formHelper)
        {
            var boss_id = formHelper["boss_id"];
            var staff_transaction = formHelper["staff_transaction"];
            var comp_name = formHelper["comp_name"];
            var description = formHelper["description"];
            var photo = formHelper["photo"];

            var isCompanyExisting = db.tblCompanies.Any(x => x.Comp_Name == comp_name);
            if (isCompanyExisting)
            {
                return Json(new { msg_company = "Company is already" });
            }

            companyEntity.FK_Boss_Id = int.Parse(boss_id);
            companyEntity.Comp_Name = comp_name;
            companyEntity.Picture_Company = photo;
            companyEntity.Descr = description;
            companyEntity.User_Update = _clHelper.GetUserLoginId();
            companyEntity.Date_Update = DateTime.Now.ToString("yyyy-MM-dd");
            companyEntity.Time_Update = DateTime.Now.ToString("hh:mm:ss");
            companyEntity.Number_Staff_Transaction = int.Parse(staff_transaction);

            db.tblCompanies.Add(companyEntity);
            db.SaveChanges();
            return Json(new { msg_success = "Saved successfully existing" });
        }

        // POST: Company/Edit/5
        [HttpPost]
        public ActionResult Update(tblCompany companyEntity, FormCollection formHelper)
        {
            int boss_id = int.Parse(formHelper["boss_id"]);
            var staff_transaction = int.Parse(formHelper["staff_transaction"]);
            var comp_name = formHelper["comp_name"];
            var description = formHelper["description"];
            var photo = formHelper["photo"];
            int comp_id = int.Parse(formHelper["comp_id"]);
            var photo_to_delete = formHelper["photo_to_delete"];

            var isCompanyExisting = db.tblCompanies.Any(x => (x.Comp_Name == comp_name && x.PK_Comp_Id != comp_id));
            if (isCompanyExisting)
            {
                return Json(new { msg_company = "Company is already existing" });
            }
            else
            {
                DeleteCompanyPhoto(photo_to_delete, photo);

                companyEntity.PK_Comp_Id = comp_id;
                companyEntity.FK_Boss_Id = boss_id;
                companyEntity.Comp_Name = comp_name;
                companyEntity.Picture_Company = photo;
                companyEntity.Descr = description;
                companyEntity.User_Update = _clHelper.GetUserLoginId();
                companyEntity.Date_Update = DateTime.Now.ToString("yyyy-MM-dd");
                companyEntity.Time_Update = DateTime.Now.ToString("hh:mm:ss");
                companyEntity.Number_Staff_Transaction = staff_transaction;

                db.Entry(companyEntity).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { msg_success = "Updated Successfully", JsonRequestBehavior.AllowGet });
            }

            
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase f = files[0];

                    string FileName = Path.GetFileNameWithoutExtension(f.FileName);

                    if (!string.IsNullOrEmpty(FileName))
                    {
                        string NameFileSave = DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim();
                        var folderPath = Path.Combine(Server.MapPath("~/Content/Uploads/Comp_Photoes/"));
                        var imgToDb = _imgUploadHelper.UploadImage(files, NameFileSave, folderPath, 200, 200, 200);

                        return Json(new { message = imgToDb });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message });
                }
            }

            return Json(new { error = "No files selected." });
        }

        private void DeleteCompanyPhoto(string oldPic, string currentPic)
        {
            if (oldPic != currentPic)
            {
                string photoPath = Url.Content("~/Content/Uploads/Comp_Photoes/");
                _imgUploadHelper.DeleteFileImage(string.Format("{0}{1}", photoPath, currentPic));
            }
        }

    }
}
