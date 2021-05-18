using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace web_payrolls.Controllers
{
    public class LanguageController : Controller
    {
        [HttpPost]
        public ActionResult Change(string lang) {
            if (lang != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                
                var cookie = new HttpCookie("lang");
                cookie.Value = lang;
                Response.Cookies.Add(cookie);
            }
            return Json("done");
        }    
    }
}