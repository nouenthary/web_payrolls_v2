using System;
using System.Web;
using System.Web.Mvc;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class BaseController : Controller
    {
        // GET
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;    
            HttpCookie langCookie = Request.Cookies["culture"];    
            if (langCookie != null) {    
                lang = langCookie.Value;    
            } else {    
                var userLanguage = Request.UserLanguages;    
                var userLang = userLanguage != null ? userLanguage[0] : "";    
                if (userLang != "") {    
                    lang = userLang;    
                } else {    
                    lang = Language.GetDefaultLanguage();    
                }    
            }    
            new Language().SetLanguage(lang);    
            return base.BeginExecuteCore(callback, state);   
        }
    }
}