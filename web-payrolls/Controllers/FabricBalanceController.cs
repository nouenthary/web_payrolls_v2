using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class FabricBalanceController : Controller
    {
        // connection
        private readonly DB_Connection _connection = new DB_Connection();
        
        // GET
        public ActionResult Index()
        {
            return View();
        }
        
        // sub view
        public PartialViewResult GetTable
            (
            int? page,
            int? pageSize,
            // 1
            int? bid = null,
            // 2
            int? cid = null,
            // 3
            
            // 4
            string type = "",
            // 5        
            int? typeName = null,
            // 6
            int? product = null,
            // 7
            string codeProduct = "",
            // 8
            string barcode = "",
            // 9
            string qrCode = "",
            // 10
            string grade = "",
            // 11
            string color = "",
            // 12
            string size = "",
            // 13
            string status = ""
            )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var fabricBalance = _connection
                .GetAllUserFabircBalance(
                    bid,
                    cid,
                    typeName,
                    type,
                    product,
                    codeProduct,
                    barcode,
                    qrCode,
                    grade,
                    color,
                    size,
                    status
                ).ToList().ToPagedList(pageIndex, defaultPage);
            
            return PartialView(fabricBalance);
        }
    }
}