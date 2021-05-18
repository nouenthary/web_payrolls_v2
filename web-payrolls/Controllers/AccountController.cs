using System;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class AccountController : Controller
    {
        private DB_Connection db = new DB_Connection();
        private readonly ClHelper _clHelper = new ClHelper();

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection formHelper)
        {
            var username = formHelper["username"];
            var password = formHelper["password"];

            if (Request.Browser.Crawler)
            {
                //We have a web crawler.
                return Json(new { msg_error = "Crawler" });
            }

            string userFromDB = string.Empty;
            string pwdFromDB = string.Empty;

            userFromDB = db.tblStaffs.Where(x => x.UserName == username).Select(x => x.UserName).FirstOrDefault();

            if (userFromDB != string.Empty)
            {
                pwdFromDB = db.tblStaffs.Where(x => x.UserName == username).Select(x => x.Password).FirstOrDefault();
                pwdFromDB = _clHelper.DecryptPassword(pwdFromDB);
            }
            else
            {
                WriteLoginCount();
            }

            int PK_Staff_Id = 0;

            if (!string.IsNullOrEmpty(userFromDB))
            {
                if (username.ToLower() == userFromDB.ToLower() & password == pwdFromDB)
                {
                    PK_Staff_Id = Convert.ToInt32(db.tblStaffs.Where(x => x.UserName == username).Select(x => x.PK_Staff_Id).FirstOrDefault());

                    string Staff_Status = string.Empty;
                    Staff_Status = db.tblStaffs.Where(x => x.PK_Staff_Id == PK_Staff_Id).Select(x => x.Staff_Status).FirstOrDefault();

                    if (Staff_Status == "Disable")
                    {
                        return Json(new { msg_error = "Your account is disable." });
                    }
                    else
                    {
                        // check permission of login user
                        bool isUserExistingPermission = db.tblPermissions.Any(x => x.FK_Staff_Id == PK_Staff_Id);

                        if (isUserExistingPermission)
                        {
                            // Login success go home
                            if (HttpContext.Request.Cookies["opLCx"] != null)
                            {
                                HttpContext.Response.Cookies["opLCx"].Expires = DateTime.Now.AddMinutes(-1); // clear old cookies
                                HttpContext.Response.Cookies["cOLCx"].Expires = DateTime.Now.AddMinutes(-1); // clear company cookies
                                HttpContext.Response.Cookies["cOPLCx"].Expires = DateTime.Now.AddMinutes(-1); // clear company picture cookies
                                HttpContext.Response.Cookies["SnLCx"].Expires = DateTime.Now.AddMinutes(-1);
                                HttpContext.Response.Cookies["SpLCx"].Expires = DateTime.Now.AddMinutes(-1);
                            }

                            var LoginCode = "";
                            // check exist LoginCode Code in DB
                            string LoginCodeDB = string.Empty;
                            LoginCodeDB = db.tblStaffs.Where(x => x.PK_Staff_Id == PK_Staff_Id).Select(x => x.LoginCodeGeneration).FirstOrDefault();

                            if (string.IsNullOrEmpty(LoginCodeDB))
                            {
                                // Generate new Login Code
                                LoginCode = PK_Staff_Id + DateTime.Now.ToString("hh:mm:ss");
                                LoginCode = _clHelper.EncryptCookies(LoginCode);
                                db.AddLoginCodeGeneration(PK_Staff_Id, LoginCode);

                            }
                            else
                            {
                                LoginCode = LoginCodeDB;
                            }

                            var companyInfo = db.GetCompanyInfoByStaffId(PK_Staff_Id).ToArray();
                            var staffInfo = db.tblStaffs.Where(x => x.PK_Staff_Id == PK_Staff_Id).ToArray();

                            HttpContext.Response.Cookies["opLCx"]["kdBkLi"] = LoginCode;

                            HttpContext.Response.Cookies["cOLCx"]["kdBkLi"] = companyInfo.Select(x => x.Comp_Name).FirstOrDefault();
                            HttpContext.Response.Cookies["cOPLCx"]["kdBkLi"] = companyInfo.Select(x => x.Picture_Company).FirstOrDefault();

                            HttpContext.Response.Cookies["SnLCx"]["kdBkLi"] = staffInfo.Select(x => x.UserName).FirstOrDefault();
                            HttpContext.Response.Cookies["SpLCx"]["kdBkLi"] = staffInfo.Select(x => x.Photo).FirstOrDefault();

                            HttpContext.Response.Cookies["opLCx"].Expires = DateTime.Now.AddMinutes(80000);  // cookies expired in 30 minutes later
                            HttpContext.Response.Cookies["cOLCx"].Expires = DateTime.Now.AddMinutes(80000);
                            HttpContext.Response.Cookies["cOPLCx"].Expires = DateTime.Now.AddMinutes(80000);
                            HttpContext.Response.Cookies["SnLCx"].Expires = DateTime.Now.AddMinutes(80000);
                            HttpContext.Response.Cookies["SpLCx"].Expires = DateTime.Now.AddMinutes(80000);

                            _clHelper.WriteLoginCount(5); // write Login count left to 5

                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return Json(new { msg_error = "You don't have permission to access this system. Please contact to Admin." });
                        }

                    }
                }
                else
                {
                    WriteLoginCount();
                    return Json(new { msg_error = "Username or Password was wrong." });
                }
            }
            else
            {
                //return Json(new { msg_error = "Username or Password was wrong." });
                return RedirectToAction("Login", "Account");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ViewProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            if (HttpContext.Request.Cookies["opLCx"] != null)
            {
                HttpContext.Response.Cookies["opLCx"].Expires = DateTime.Now.AddMinutes(-1); // clear old cookies
                HttpContext.Response.Cookies["cOLCx"].Expires = DateTime.Now.AddMinutes(-1);
                HttpContext.Response.Cookies["cOPLCx"].Expires = DateTime.Now.AddMinutes(-1);
                HttpContext.Response.Cookies["SnLCx"].Expires = DateTime.Now.AddMinutes(-1);
                HttpContext.Response.Cookies["SpLCx"].Expires = DateTime.Now.AddMinutes(-1);
            }
            //_loginCode = string.Empty;

            return RedirectToAction("Login", "Account");
        }
        public ActionResult WriteLoginCount()
        {
            int LoginLeft = _clHelper.ReadLoginCount();
            LoginLeft -= 1;
            _clHelper.WriteLoginCount(LoginLeft);

            if (LoginLeft <= 0)
            {
                if (HttpContext.Request.Cookies["opLCx"] != null)
                {   // Logout System
                    HttpContext.Response.Cookies["opLCx"].Expires = DateTime.Now.AddMinutes(-1); // clear old cookies
                }

                return HttpNotFound();
            }

            return HttpNotFound();

            //return Json(new { msg_login_attemp = "Login attempt : " + LoginLeft.ToString(), JsonRequestBehavior.AllowGet });
        }
    }
}
