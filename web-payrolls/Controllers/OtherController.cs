using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using web_payrolls.Helpers;
using web_payrolls.Models;

namespace web_payrolls.Controllers
{
    public class OtherController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();

        private readonly ImgUploadHelper _uploadHelper = new ImgUploadHelper();

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult GetStaffName(string value)
        {
            var data = _connection
                .tblStaffs
                .Where(s => s.Staff_Name.Contains(value))
                .Select(s => s.Staff_Name.Trim())
                .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCardID(string value)
        {
            var data = _connection
                .tblStaffs
                .Where(s => s.Staff_Id_Card.Contains(value))
                .Select(s => s.Staff_Id_Card.Trim())
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Upload(FormCollection form)
        {
            var path = form["path"];

            const int width = 400;
            const int height = 400;

            if (Request.Files.Count > 0)
            {
                try
                {
                    var collection = Request.Files;
                    var file = collection[0];

                    var fileExtension = Path.GetExtension(file.FileName);

                    var nameFileForSave = DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + fileExtension;

                    var resizeImage = _uploadHelper.ResizeImage(Image.FromStream(file.InputStream), width, height);

                    var saveImage = Path.Combine(Server.MapPath("~/Content/Uploads/" + path), nameFileForSave);
                    //file.SaveAs(fname);
                    resizeImage.Save(saveImage);

                    return Json(new { message = nameFileForSave });
                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message });
                }
            }

            return Json(new { error = "No files selected." });
        }

        //Upload not cut size
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UploadFull(FormCollection form)
        {
            var path = form["path"];

            if (Request.Files.Count > 0)
            {
                try
                {
                    var collection = Request.Files;
                    var file = collection[0];

                    var fileExtension = Path.GetExtension(file.FileName);

                    var nameFileForSave = DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + fileExtension;

                    var saveImage = Path.Combine(Server.MapPath("~/Content/Uploads/" + path), nameFileForSave);
                    file.SaveAs(saveImage);

                    return Json(new { message = nameFileForSave });
                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message });
                }
            }

            return Json(new { error = "No files selected." });
        }


        // Get Company
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCompany(FormCollection form) {
            if (form["id"] == "")
            {
                return Json(null);
            }
            var bossId = int.Parse(form["id"]);
            var company = _connection
                .tblCompanies
                .Where(c => c.FK_Boss_Id == bossId)
                .Select(c => new {key = c.PK_Comp_Id ,value = c.Comp_Name})
                .ToList();
            return Json(company);
        }

        // Get Location
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetLocation(FormCollection form)
        {
            if (form["id"] == "") {
                return Json(null);
            }
            var comId = int.Parse(form["id"]);
            var location = _connection
                .tblLocations
                .Where(l => l.FK_Comp_Id == comId)
                .Select(l => new {key = l.PK_Location_Id, value = l.Loc_Name })
                .ToList();
            return Json(location);
        }

        // Get Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDepartment(FormCollection form) {

            if (form["id"] == "") {
                return Json(null);
            }

            var locId = int.Parse(form["id"]);
            var department = _connection
                .tblDepartments
                .Where(d => d.FK_Loc_Id == locId)
                .Select(d => new {key = d.PK_Depart_Id, value = d.Deppart_Name })
                .ToList();

            return Json(department);
        }

        // Get Position
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPosition(FormCollection form) {

            if (form["id"] == "") {
                return Json(null);
            }

            var deptId = int.Parse(form["id"]);
            var position = _connection
                .tblPositions
                .Where(p => p.FK_Depart_Id == deptId)
                .Select(p => new {key =  p.PK_Pos_Id, value = p.Pos_Name })
                .ToList();

            return Json(position);
        }

        // get staff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStaff(FormCollection form)
        {
            if (form["id"] == "") {
                return Json(null);
            }

            var positionId = int.Parse(form["id"]);
            var staffs = _connection
                .tblStaffs
                .Where(s => s.FK_Pos_Id == positionId)
                .Select(s=> new{key = s.PK_Staff_Id, value = s.Staff_Name})
                .ToList();

            return Json(staffs);
        }

        // voucher type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetVoucherType(FormCollection form)
        {
            if (form["id"] == "") {
                return Json(null);
            }
            var locationId = int.Parse(form["id"]);

            var voucherType = _connection
                .tblVoucher_Rent_Invertory_Rent_And_Other_Voucher
                .Where(v => v.FK_loc_id == locationId)
                .Select(v=> new
                {
                    key = v.PK_Voucher_Rent_Other_Id,
                    value = v.Name
                });

            return Json(voucherType);
        }

        // get staff voucher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStaffVoucher(FormCollection formCollection)
        {
            if (formCollection["id"] == "") {
                return Json(null);
            }

            var positionId = int.Parse(formCollection["id"]);
            var voucher = _connection
                .GetAllStaffVoucherByPosition(positionId)
                .Select(v=> new
                {
                    key = v.id , value= v.name
                })
                .ToList();
            return Json(voucher);
        }

        // get product type
       [HttpPost]
       [ValidateAntiForgeryToken]
        public JsonResult GetProductType(FormCollection formCollection)
        {
            if (formCollection["id"] == "") {
                return Json(null);
            }

            var hodId = int.Parse(formCollection["id"]);

            var productType = _connection
                .tblProduction_ProductType
                .Where(p => p.FK_Boss_Id == hodId)
                .Select(p=> new
                {
                    key = p.PK_ProType_Id,
                    value = p.ProType_Name
                })
                .ToList();

            return Json(productType);
        }

        // get product type by boss
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductTypeByBoss(FormCollection formCollection)
        {
            if (formCollection["id"] == "") {
                return Json(null);
            }

            var hodId = int.Parse(formCollection["id"]);

            var productType = _connection
                .tblProduction_ProductType
                .Where(p => p.FK_Boss_Id == hodId)
                .Select(p=> new
                {
                    key = p.Pro_Type,
                    value = p.Pro_Type
                })
                .Distinct()
                .ToList();

            return Json(productType);
        }

        //get product type name by product type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductTypeNameByType(FormCollection formCollection)
        {
            if (formCollection["id"] == "") {
                return Json(null);
            }

            var productTypes = formCollection["id"];

            var productType = _connection
                .tblProduction_ProductType
                .Where(p => p.Pro_Type == productTypes)
                .Select(p=> new
                {
                    key = p.ProType_Name,
                    value = p.ProType_Name
                })
                .Distinct()
                .ToList();

            return Json(productType);
        }


        // Get Product color by product id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductColorByProductId(FormCollection formCollection)
        {
            if (formCollection["id"] == "") {
                return Json(null);
            }

            var productId = int.Parse(formCollection["id"]);

            var productColor = _connection
                .tblProduction_Color
                .Where(pc => pc.FK_Pro_Id == productId)
                .Select(pc => new
                {
                    key = pc.PK_Color_Id,
                    value = pc.Color_Name
                })
                .ToList();
            return Json(productColor);
        }

        // Get Product Size By Product color
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductSizeByProductColorId(FormCollection formCollection)
        {
            if (formCollection["id"] == "") {
                return Json(null);
            }

            var productColorId = int.Parse(formCollection["id"]);

            var productSize = _connection
                .tblProduction_Size
                .Where(ps => ps.FK_Color_Id == productColorId)
                .Select(pc => new
                {
                    key = pc.PK_Size_Id,
                    value = pc.Size_Name
                })
                .ToList();
            return Json(productSize);
        }

        // Get product type name by hod
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductTypeNameByProductType(FormCollection form)
        {
            var id = int.Parse(form["id"]);
            var productType = form["productType"];

            if (id < 0)
            {
                return Json(null);
            }
            // product type name
            var productTypeName = (
                from type in _connection.tblProduction_ProductType
                where type.FK_Boss_Id == id && type.Pro_Type == productType
                select new
                {
                    key = type.PK_ProType_Id,
                    value = type.ProType_Name
                }
            ).Distinct().ToList();
            return Json(productTypeName);
        }

        // get Product by product type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductByProductType(FormCollection form)
        {
            var id = int.Parse(form["id"]);

            if (id < 0)
            {
                return Json(null);
            }

            var products =
            (
                from product in _connection.tblProduction_Product
                where product.FK_ProType_Id == id
                select new
                {
                    key = product.PK_Pro_Id,
                    value = product.Pro_Name
                }
            ).Distinct().ToList();
            return Json(products);
        }

        // get product grade by product id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetProductGradeByProductId(FormCollection form)
        {
            var id = int.Parse(form["id"]);
            if (id < 0)
            {
                return Json(null);
            }

            var productGrade = _connection
                .tblProduction_Grade
                .Where(grade => grade.FK_Product_Id == id);
            // Grade
            var grades = productGrade
                .Select(grade => new {key = grade.Grade_Name, value = grade.Grade_Name}).Distinct();
            // Color
            var colors = productGrade
                .Select(grade => new {key = grade.Color, value = grade.Color}).Distinct();
            // Size
            var sizes = productGrade.Select(grade => new {key = grade.Size, value = grade.Size}).Distinct();
            return Json(new
            {
                grades, colors, sizes
            });
        }


        // 2021-05-03
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetGradeByProductId(FormCollection form)
        {
          var id = int.Parse(form["id"]);

          if (id < 0)
          {
            return Json(null);
          }

          var grade = _connection.tblProduction_Grade
            .Where(x=>x.FK_Product_Id == id)
            .Select(x=> new
            {
              key = x.PK_Grade_Id,
              value = x.Grade_Name,
              x.Color,
              x.Size
            })
            .ToList();

          return Json(grade);
        }
    }
}
