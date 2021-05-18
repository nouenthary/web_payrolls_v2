using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using QRCoder;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class TestController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();
        // GET
        public ActionResult Index()
        {
            var d = _connection.tblVouchers.ToList();

            for (var i = 1; i < 10000 ; i++ )
            {
                var v = new tblVoucher()
                {
                    Amount = 23,
                    Confirm_time = "",
                    Descr = "",
                    Discount = 34,
                    Total = "234",
                    Confirm_Date = "",
                    Date_Voucher = "",
                    Rec_Date = "",
                    Rec_Time = "",
                    Time_Voucher = "",
                    Unit_Cost = 2342,
                    UoM =34,
                    Voucher_Name = "dd",
                    Voucher_Picure = "",
                    Voucher_Status = "Done",
                    PCC = 34,
                    QTY = 34,
                    FK_Staff_Id = 34,
                    Confirm_PK_U_Id = 334
                };
               // _connection.tblVouchers.Add(v);
              // _connection.SaveChanges();
            }

            return View(d);
        }

        [HttpPost]
        public ActionResult Generate(string qrCode)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator generator = new QRCodeGenerator();
                QRCodeData data = generator.CreateQrCode(qrCode, QRCodeGenerator.ECCLevel.Q);
                QRCode code = new QRCode(data);

                using (Bitmap bitmap = code.GetGraphic(20))
                {
                    bitmap.Save(ms,ImageFormat.Png);
                    TempData["QrCodeImage"] = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
            
            return RedirectToAction("Index");
        }
    }
}