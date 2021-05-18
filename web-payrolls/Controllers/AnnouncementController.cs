using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using System.IO;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ClHelper _helper = new ClHelper();
        private readonly ContextProvider _provider = new ContextProvider(new ClHelper(), new DB_Connection());
        // GET: Announcement
        public ActionResult Index()
        {
            ViewBag.Manager = _connection.tblBosses.ToList();
            return View();
        }

        public void Creates()
        {
            var find = _connection.tblAnnouncments.Count(s=> s.FK_Loc_Id == _provider.LocationId);
            if (find == 0)
            {
                var a = new tblAnnouncment()
                {
                    FK_Loc_Id = _provider.LocationId,
                    Title = "បុណ្យអ៊ុំទូក",
                    Picture = "20201214_103153.jpg",
                    Title_Booter = "1 Days",
                    Descr = "Some quick example text to build on the card title and make up the bulk of the card's content.",
                
                    Title1 = "បុណ្យអ៊ុំទូក",
                    Picture1 = "20201214_103153.jpg",
                    Title_Booter1 = "1 Days",
                    Descr1= "Some quick example text to build on the card title and make up the bulk of the card's content.",
                
                    Title2 = "បុណ្យអ៊ុំទូក",
                    Picture2 = "20201214_103153.jpg",
                    Title_Booter2 = "1 Days",
                    Descr2 = "Some quick example text to build on the card title and make up the bulk of the card's content.",
               
                    User_Update = _provider.StaffId,
                    Date_Update = Constraint.GetDate(),
                    Time_Update = Constraint.GetTime()
                };

                _connection.tblAnnouncments.Add(a);
                _connection.SaveChanges();
            }
        }

        // sub view
        public PartialViewResult GetTable
        (
            int? page,
            int? pageSize,
            int? bid = null,
            int? cid = null,
            int? lid = null
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;
            
            var data = _connection
                .GetALLAnnouncement(bid,cid,lid,null)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);
            
            return PartialView(data);
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

                    var fileName = Path.GetFileNameWithoutExtension(f.FileName);
                    var fileExtension = Path.GetExtension(f.FileName);

                    var nameFileSave = DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + fileExtension;

                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), nameFileSave);
                        file.SaveAs(fname);
                    }
                    return Json(new { message = nameFileSave });
                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message });
                }
            }

            return Json(new { error = "No files selected." });
        }
        
        // create 
        public ActionResult Create()
        {
            ViewBag.Manager = _connection.tblBosses.ToList();
            return View();
        }
        
        // store
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Store(FormCollection formCollection)
        {
            var locationId = int.Parse(formCollection["loc_id"]);

            var entity = _connection.tblAnnouncments.Any(a => a.FK_Loc_Id == locationId);
            if (entity)
            {
                return Json(new{error = "Announcement Exist"});
            }

            var announcment = new tblAnnouncment()
            {
                FK_Loc_Id = locationId,
                Title = formCollection["title"],
                Picture =  formCollection["photo"],
                Title_Booter = formCollection["title_booster"],
                Descr = formCollection["desc"],
                
                Title1 = formCollection["title1"],
                Picture1 = formCollection["photo1"],
                Title_Booter1 = formCollection["title_booster1"],
                Descr1 = formCollection["desc1"],
                
                Title2 = formCollection["title2"],
                Picture2 = formCollection["photo2"],
                Title_Booter2 = formCollection["title_booster2"],
                Descr2 = formCollection["desc2"],
                
                User_Update = _helper.GetUserLoginId(),
                Date_Update = Constraint.GetDate(),
                Time_Update = Constraint.GetTime()
            };

            _connection.tblAnnouncments.Add(announcment);
            _connection.SaveChanges();

            RemoveFile();
            
            return new JsonResult() { Data = "Created Successfully."};
        }
        
        // Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var data = _connection.tblAnnouncments.Single(s=> s.PK_Announment_Id == id);

            if (data == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(data);
        }
        
        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(FormCollection formCollection)
        {
            var announcementId = int.Parse(formCollection["announcement_id"]);
            if (announcementId < 0)
            {
                return Json(new {erorr = "id announcement not found."});
            }

            var entity = _connection.tblAnnouncments.Single(n => n.PK_Announment_Id == announcementId);
            if (entity == null)
            {
                return Json(new {erorr = "announcement not found."});
            }

            entity.Title = formCollection["title"];
            entity.Title_Booter = formCollection["title_booster"];
            entity.Picture = formCollection["photo"];
            entity.Descr = formCollection["desc"];
            
            entity.Title1 = formCollection["title1"];
            entity.Title_Booter1 = formCollection["title_booster1"];
            entity.Picture1 = formCollection["photo1"];
            entity.Descr1 = formCollection["desc1"];
            
            entity.Title2 = formCollection["title2"];
            entity.Title_Booter2 = formCollection["title_booster2"];
            entity.Picture2 = formCollection["photo2"];
            entity.Descr2 = formCollection["desc2"];
            entity.User_Update = _helper.GetUserLoginId();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();
            
            _connection.SaveChanges();

            RemoveFile();
            return new JsonResult() {Data = "Updated Successfully."};
        }
        
        // Remove File Name 
        private void RemoveFile()
        {
            var dir = HttpContext.Server.MapPath("~/Content/Uploads/Announcement/");

            var filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/Announcement/"));

            var fileInPath = new List<string>();

            foreach (var file in filePaths)
            {
                fileInPath.Add(Path.GetFileName(file));
            }            
            // Remove
            foreach (var fileName in fileInPath) {

                var nameInDb = _connection.tblAnnouncments.Where(s =>s.Picture == fileName || s.Picture1 == fileName || s.Picture2 == fileName);

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