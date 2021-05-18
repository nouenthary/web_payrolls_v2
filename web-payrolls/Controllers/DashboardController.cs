using System.Linq;
using System.Web.Mvc;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ContextProvider _provider = new ContextProvider(new ClHelper(), new DB_Connection());
        // GET: Dashboard
        public ActionResult Index()
        {
            ViewBag.LocationId = _provider.LocationId;
            var data = _connection
                .tblAnnouncments
                .Single(l => l.FK_Loc_Id == _provider.LocationId);
            return View(data);          
        }
        
        public ActionResult Upload()
        {
            return View();
        }
    }
}