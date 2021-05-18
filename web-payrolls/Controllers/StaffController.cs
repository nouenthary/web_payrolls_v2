using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class StaffController : Controller
    {
        private DB_Connection db = new DB_Connection();
        private readonly ClHelper _clHelper = new ClHelper();
        private readonly ImgUploadHelper _imgUploadHelper = new ImgUploadHelper();
        // GET: Staff
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
            int? pid = null,
            string salType = "",
            string status = "",
            string textSearch = ""
        )
        {
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            int defaultPage = (pageSize ?? 10);
            ViewBag.psize = defaultPage;

            GetNumberToDisplayDataPerPage();

            var staffs = db
                .GetAllStaffs(bid, cid, lid, did, pid, salType, status, textSearch)
               .ToList()
               .ToPagedList(pageIndex, defaultPage);
            return PartialView(staffs); // when we return as PartialView we need to crate the function name the same as page in Views
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

        // Get department by location Id
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

        // Get position by department Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPosition(FormCollection form)
        {
            int dept_id = int.Parse(form["dept_id"]);
            if (dept_id == 0)
            {
                return Json(null);
            }

            var positions = _clHelper.GetPositionByDeptId(dept_id);
            return Json(positions);
        }

        [HttpPost]
        public string AddStaff(tblStaff staffEntity, FormCollection formHelper)
        {
            // variable for tblStaff
            var FK_Pos_Id = Convert.ToInt32(formHelper["FK_Pos_Id"]);
            var Staff_Name = formHelper["Staff_Name"];
            var Sex = formHelper["Sex"];
            var Staff_DOB = formHelper["Staff_DOB"];
            var Marry_Status = formHelper["Marry_Status"];
            var Nationality = formHelper["Nationality"];
            var Wife = Convert.ToInt32(formHelper["Wife"]);
            var Son = Convert.ToInt32(formHelper["Son"]);
            var Daughter = Convert.ToInt32(formHelper["Daughter"]);
            var Tell = formHelper["Tell"];
            var Account_Name = formHelper["Account_Name"];
            var Account_Number = formHelper["Account_Number"];
            var Address = formHelper["Address"];
            var Identity_Card = formHelper["Identity_Card"];
            var Identify_Picture = formHelper["Identify_Picture"];
            var Staff_Id_Card = formHelper["Staff_Id_Card"];
            var Serail_Card = formHelper["Serail_Card"];
            var Staff_Status = formHelper["Staff_Status"];
            var Join_date = formHelper["Join_date"];
            var Photo = formHelper["Photo"];
            var UserName = formHelper["UserName"];
            var Password = formHelper["Password"];
            var No_Scan = formHelper["No_Scan"];
            var encryptPassword = _clHelper.EncryptPassword(Password);

            int User_Update = _clHelper.GetUserLoginId();
            var Date_Update = DateTime.Now.ToString("yyyy-MM-dd");
            var Time_Update = DateTime.Now.ToString("hh:mm:ss");

            // variable for tblRoster
            int Num_Year = Convert.ToInt32(formHelper["Num_Year"]);
            int Num_Month = Convert.ToInt32(formHelper["Num_Month"]);

            // variable for tblSalType
            var Sal_Type = formHelper["Sal_Type"];
            var Salary = !string.IsNullOrEmpty(formHelper["Salary"]) ? float.Parse(formHelper["Salary"]) : 0;
            var Card_Phone = !string.IsNullOrEmpty(formHelper["Card_Phone"]) ? float.Parse(formHelper["Card_Phone"]) : 0;
            var Gym = !string.IsNullOrEmpty(formHelper["Gym"]) ? float.Parse(formHelper["Gym"]) : 0;
            var Accommodation = !string.IsNullOrEmpty(formHelper["Accommodation"]) ? float.Parse(formHelper["Accommodation"]) : 0;
            var Food = !string.IsNullOrEmpty(formHelper["Food"]) ? float.Parse(formHelper["Food"]) : 0;
            var insurance = !string.IsNullOrEmpty(formHelper["insurance"]) ? float.Parse(formHelper["insurance"]) : 0;
            var Money_Prim = !string.IsNullOrEmpty(formHelper["Money_Prim"]) ? float.Parse(formHelper["Money_Prim"]) : 0;
            var Travel = !string.IsNullOrEmpty(formHelper["Travel"]) ? float.Parse(formHelper["Travel"]) : 0;
            var Gasoline = !string.IsNullOrEmpty(formHelper["Gasoline"]) ? float.Parse(formHelper["Gasoline"]) : 0;
            var OT_Unit_Price = !string.IsNullOrEmpty(formHelper["OT_Unit_Price"]) ? float.Parse(formHelper["OT_Unit_Price"]) : 0;
            var Work_Hour_Taget = !string.IsNullOrEmpty(formHelper["Work_Hour_Taget"]) ? float.Parse(formHelper["Work_Hour_Taget"]) : 0;
            var Relax_Hour = !string.IsNullOrEmpty(formHelper["Relax_Hour"]) ? float.Parse(formHelper["Relax_Hour"]) : 0;
            var Off_Day = !string.IsNullOrEmpty(formHelper["Off_Day"]) ? float.Parse(formHelper["Off_Day"]) : 0;
            var AL_Day = !string.IsNullOrEmpty(formHelper["AL_Day"]) ? float.Parse(formHelper["AL_Day"]) : 0;
            var PH_Day = !string.IsNullOrEmpty(formHelper["PH_Day"]) ? float.Parse(formHelper["PH_Day"]) : 0;
            var Sick = !string.IsNullOrEmpty(formHelper["Sick"]) ? float.Parse(formHelper["Sick"]) : 0;
            var UPL = !string.IsNullOrEmpty(formHelper["UPL"]) ? float.Parse(formHelper["UPL"]) : 0;
            var AB = !string.IsNullOrEmpty(formHelper["AB"]) ? float.Parse(formHelper["AB"]) : 0;
            var CM = !string.IsNullOrEmpty(formHelper["CM"]) ? float.Parse(formHelper["CM"]) : 0;

            var isStaffNameOrUserExisting = db.tblStaffs.Any(x => ((x.Staff_Name == Staff_Name) & (x.FK_Pos_Id == FK_Pos_Id)) | (x.UserName == UserName));
            if (isStaffNameOrUserExisting)
            {
                return "Staff Name or Username is already existing";
                //return Json(new { msg_existing = "Staff Name or Username is already existing" });
            } else
            {
                staffEntity.FK_Pos_Id = FK_Pos_Id;
                staffEntity.Staff_Name = Staff_Name;
                staffEntity.Sex = Sex;
                staffEntity.Staff_DOB = Staff_DOB;
                staffEntity.Marry_Status = Marry_Status;
                staffEntity.Nationality = Nationality;
                staffEntity.Wife = Wife;
                staffEntity.Son = Son;
                staffEntity.Daughter = Daughter;
                staffEntity.Tell = Tell;
                staffEntity.Account_Name = Account_Name;
                staffEntity.Account_Number = Account_Number;
                staffEntity.Address = Address;
                staffEntity.Identity_Card = Identity_Card;
                staffEntity.Identify_Picture = Identify_Picture;
                staffEntity.Staff_Id_Card = Staff_Id_Card;
                staffEntity.Serail_Card = Serail_Card;
                staffEntity.Staff_Status = Staff_Status;
                staffEntity.Join_date = Join_date;
                staffEntity.Photo = Photo;
                staffEntity.UserName = UserName;
                staffEntity.Password = encryptPassword;
                staffEntity.No_Scan = No_Scan;
                staffEntity.User_Update = User_Update;
                staffEntity.Date_Update = Date_Update;
                staffEntity.Time_Update = Time_Update;

                db.tblStaffs.Add(staffEntity);
                db.SaveChanges();

                var staff_id = Convert.ToInt32(staffEntity.PK_Staff_Id);

                AddPermission(staff_id);
                AddRoster(staff_id, Num_Year, Num_Month, User_Update, Date_Update, Time_Update);
                AddOrUpdateSalaryType(staff_id, Sal_Type, Salary, Card_Phone, Gym, Accommodation, Food, insurance, Money_Prim, Travel, Gasoline,
                            OT_Unit_Price, Work_Hour_Taget, Relax_Hour, Off_Day, AL_Day, PH_Day, Sick, UPL, AB, CM, User_Update,
                            Date_Update, Time_Update, 0);

                return "Saved successfully";
                //return Json(new { msg_success = "Saved successfully" });
            }
        }

        [HttpPost]
        public string UpdateStaff(tblStaff staffEntity, FormCollection formHelper)
        {
            // variable for tblStaff
            var FK_Pos_Id = Convert.ToInt32(formHelper["FK_Pos_Id"]);
            var Staff_Name = formHelper["Staff_Name"];
            var Sex = formHelper["Sex"];
            var Staff_DOB = formHelper["Staff_DOB"];
            var Marry_Status = formHelper["Marry_Status"];
            var Nationality = formHelper["Nationality"];
            var Wife = Convert.ToInt32(formHelper["Wife"]);
            var Son = Convert.ToInt32(formHelper["Son"]);
            var Daughter = Convert.ToInt32(formHelper["Daughter"]);
            var Tell = formHelper["Tell"];
            var Account_Name = formHelper["Account_Name"];
            var Account_Number = formHelper["Account_Number"];
            var Address = formHelper["Address"];
            var Identity_Card = formHelper["Identity_Card"];
            var Identify_Picture = formHelper["Identify_Picture"];
            var Staff_Id_Card = formHelper["Staff_Id_Card"];
            var Serail_Card = formHelper["Serail_Card"];
            var Staff_Status = formHelper["Staff_Status"];
            var Join_date = formHelper["Join_date"];
            var End_Date = formHelper["End_Date"];
            var Photo = formHelper["Photo"];
            var UserName = formHelper["UserName"];
            //var Password = formHelper["Password"];
            var No_Scan = formHelper["No_Scan"];
            //var encryptPassword = _clHelper.EncryptPassword(Password);

            int User_Update = _clHelper.GetUserLoginId();
            var Date_Update = DateTime.Now.ToString("yyyy-MM-dd");
            var Time_Update = DateTime.Now.ToString("hh:mm:ss");

            // variable for tblSalType
            var Sal_Type = formHelper["Sal_Type"];
            var Salary = !string.IsNullOrEmpty(formHelper["Salary"]) ? float.Parse(formHelper["Salary"]) : 0;
            var Card_Phone = !string.IsNullOrEmpty(formHelper["Card_Phone"]) ? float.Parse(formHelper["Card_Phone"]) : 0;
            var Gym = !string.IsNullOrEmpty(formHelper["Gym"]) ? float.Parse(formHelper["Gym"]) : 0;
            var Accommodation = !string.IsNullOrEmpty(formHelper["Accommodation"]) ? float.Parse(formHelper["Accommodation"]) : 0;
            var Food = !string.IsNullOrEmpty(formHelper["Food"]) ? float.Parse(formHelper["Food"]) : 0;
            var insurance = !string.IsNullOrEmpty(formHelper["insurance"]) ? float.Parse(formHelper["insurance"]) : 0;
            var Money_Prim = !string.IsNullOrEmpty(formHelper["Money_Prim"]) ? float.Parse(formHelper["Money_Prim"]) : 0;
            var Travel = !string.IsNullOrEmpty(formHelper["Travel"]) ? float.Parse(formHelper["Travel"]) : 0;
            var Gasoline = !string.IsNullOrEmpty(formHelper["Gasoline"]) ? float.Parse(formHelper["Gasoline"]) : 0;
            var OT_Unit_Price = !string.IsNullOrEmpty(formHelper["OT_Unit_Price"]) ? float.Parse(formHelper["OT_Unit_Price"]) : 0;
            var Work_Hour_Taget = !string.IsNullOrEmpty(formHelper["Work_Hour_Taget"]) ? float.Parse(formHelper["Work_Hour_Taget"]) : 0;
            var Relax_Hour = !string.IsNullOrEmpty(formHelper["Relax_Hour"]) ? float.Parse(formHelper["Relax_Hour"]) : 0;
            var Off_Day = !string.IsNullOrEmpty(formHelper["Off_Day"]) ? float.Parse(formHelper["Off_Day"]) : 0;
            var AL_Day = !string.IsNullOrEmpty(formHelper["AL_Day"]) ? float.Parse(formHelper["AL_Day"]) : 0;
            var PH_Day = !string.IsNullOrEmpty(formHelper["PH_Day"]) ? float.Parse(formHelper["PH_Day"]) : 0;
            var Sick = !string.IsNullOrEmpty(formHelper["Sick"]) ? float.Parse(formHelper["Sick"]) : 0;
            var UPL = !string.IsNullOrEmpty(formHelper["UPL"]) ? float.Parse(formHelper["UPL"]) : 0;
            var AB = !string.IsNullOrEmpty(formHelper["AB"]) ? float.Parse(formHelper["AB"]) : 0;
            var CM = !string.IsNullOrEmpty(formHelper["CM"]) ? float.Parse(formHelper["CM"]) : 0;
            var PK_Staff_Id = Convert.ToInt32(formHelper["staff_id"]);
            var PK_Sal_Type_Id = Convert.ToInt32(formHelper["sal_type_id"]);
            var previousStaffPhoto = formHelper["previousStaffPhoto"];
            var previousStaffIDCardPhoto = formHelper["previousStaffIDCardPhoto"];

            var isStaffNameAndUsernameExisting = db.tblStaffs.Any(x => (x.FK_Pos_Id == FK_Pos_Id) & (x.Staff_Name == Staff_Name) & (x.UserName == UserName) & (x.PK_Staff_Id != PK_Staff_Id));
            if (isStaffNameAndUsernameExisting)
            {
                return "Staff Name or Username is already existing";
                //return Json(new { msg_existing = "Staff Name or Username is already existing" });
            } else
            {
                DeletePicture(previousStaffPhoto); // delete staff photo
                DeletePicture(previousStaffIDCardPhoto); // delete staff ID Card

                AddOrUpdateSalaryType(PK_Staff_Id, Sal_Type, Salary, Card_Phone, Gym, Accommodation, Food, insurance, Money_Prim, Travel, Gasoline,
                            OT_Unit_Price, Work_Hour_Taget, Relax_Hour, Off_Day, AL_Day, PH_Day, Sick, UPL, AB, CM, User_Update,
                            Date_Update, Time_Update, PK_Sal_Type_Id);

                db.UpdateStaff(FK_Pos_Id, Staff_Name, Sex, Staff_DOB, Marry_Status, Nationality, Wife, Son, Daughter, Tell, Account_Name,
                    Account_Number, Address, Identity_Card, Identify_Picture, Staff_Id_Card, Serail_Card, Staff_Status, Join_date,
                    End_Date, Photo, UserName, No_Scan, User_Update, Date_Update, Time_Update, PK_Staff_Id);

                return "Updated Successfully";
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

                    string NameFileSave = DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim();
                    var folderPath = Path.Combine(Server.MapPath("~/Content/Uploads/Staff_Photoes/"));
                    var imgToDb = _imgUploadHelper.UploadImage(files, NameFileSave, folderPath, 200, 200, 200);


                    return Json(new { message = imgToDb });
                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message });
                }
            }

            return Json(new { error = "No files selected." });
        }

        private void DeletePicture(string picName)
        {
            string photoPath = "~/Content/Uploads/Staff_Photoes/";
            _imgUploadHelper.DeleteFileImage(string.Format("{0}{1}", photoPath, picName));
        }

        private void AddOrUpdateSalaryType(int FK_Sta_Id, string Sal_Type, float Salary, float Card_Phone, float Gym, float Accommodation,
            float Food, float insurance, float Money_Prim, float Travel, float Gasoline, float OT_Unit_Price, float Work_Hour_Taget,
            float Relax_Hour, float Off_Day, float AL_Day, float PH_Day, float Sick, float UPL, float AB, float CM,
            int User_Update, string Date_Update, string Time_Update, int PK_Sal_Type_Id)
        {
            if (PK_Sal_Type_Id == 0)
            {
                db.AddSalaryType(FK_Sta_Id, Sal_Type, Salary, Card_Phone, Gym, Accommodation, Food, insurance, Money_Prim, Travel, Gasoline,
                        OT_Unit_Price, Work_Hour_Taget, Relax_Hour, Off_Day, AL_Day, PH_Day, Sick, UPL, AB, CM, User_Update, Date_Update, 
                        Time_Update);
            } else
            {
                db.UpdateSalaryType(FK_Sta_Id, Sal_Type, Salary, Card_Phone, Gym, Accommodation, Food, insurance, Money_Prim, Travel, Gasoline,
                        OT_Unit_Price, Work_Hour_Taget, Relax_Hour, Off_Day, AL_Day, PH_Day, Sick, UPL, AB, CM, User_Update, Date_Update,
                        Time_Update, PK_Sal_Type_Id);
            }
            

        }

        private void AddRoster(int FK_Staff_Id, int Num_Year, int Num_Month, int User_Update, string Date_Update, string Time_Update)
        {
            db.AddRoster(FK_Staff_Id, Num_Year, Num_Month, User_Update, Date_Update, Time_Update);
        }
        private void AddPermission(int FK_Staff_Id)
        {
            Boolean isTrue = true;
            db.AddPermission(FK_Staff_Id, isTrue);
        }


    }
}