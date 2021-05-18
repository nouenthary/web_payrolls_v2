using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class StockBalanceController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        private readonly ContextProvider _provider = new ContextProvider(new ClHelper(), new DB_Connection());
        // GET
        public ActionResult Index()
        {
            // List HOD
            ViewBag.Manager = _connection.tblBosses.ToList();

            // ManagerId
            ViewData["ManagerId"] = _provider.ManagerId;
            
            // List Company
            ViewBag.Company = _connection.tblCompanies.ToList();
            
            // CompanyId
            ViewBag.CompanyId = _provider.CompanyId;
            
            // Product Type
            ViewBag.ProductType = _connection
                .tblProduction_ProductType
                .Where(type => type.FK_Boss_Id == _provider.ManagerId)
                .ToList();
            
            return View();
        }
        // sup view
        public PartialViewResult GetTable
            (
            int? page,
            int? pageSize,
            // HOD
            int? bid = null,
            // Company
            int? cid = null,
            // product type
            string types = "",
            int? typeName = null,
            // Product
            int? productId = null,
            // Grade
            string codeProduct = "",
            string grade = "",
            string barcode = "",
            string qrCode = "",
            string color = "",
            string size = "",
            string type = "",
            string status = ""
            )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaultPage = (pageSize ?? 20);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var stockBalance = _connection
                .GetAllStockBalance(
                    bid,
                    cid,
                    typeName,
                    types,
                    "",
                    productId,
                    codeProduct,
                    barcode,
                    qrCode,
                    grade,
                    color,
                    size,
                    type,
                    status != "" ? status : "Enable"
                    )
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

            return PartialView(stockBalance);
        }
    }
}