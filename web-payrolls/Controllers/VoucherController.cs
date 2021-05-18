using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using web_payrolls.Helpers;
using web_payrolls.Models;
using web_payrolls.Models.Voucher;
using Constraint = web_payrolls.Models.Constraint;

namespace web_payrolls.Controllers
{
    public class VoucherController : LanguageController
    {
        private readonly DB_Connection _connection = new DB_Connection();
        
        private readonly ClHelper _helper = new ClHelper();
        // GET
        public ActionResult Index()
        {
            ViewBag.Manager = _connection.tblBosses.ToList();
            return View();
        }

        // sup view
        public PartialViewResult GetTable(
            int? page,
            int? pageSize,
            // boss
            int? bid = null,
            //  company
            int? cid = null,
            // location
            int? lid = null,
            // staff
            int? sid = null,
            // status
            string status = "",
            // date
            string start = "",
            string end = "",
            // voucher no
            string voucherNo = ""
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;
            
            var data = _connection
                .GetAllVoucher(bid, cid,lid, sid, status, start, end, voucherNo)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);
            return PartialView(data);
        }

        // create voucher
        public ActionResult Create()
        {
            ViewBag.Manager = _connection.tblBosses.ToList();
            return View();
        }
        
        // store voucher
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public JsonResult Save(List<VoucherModel> voucherModels , VoucherList voucherList)
        {
            // get voucher no
            var voucherNo = GetVoucherNo(voucherList.PhotoVoucher);

            foreach (var row in voucherModels)
            {
                var voucher = new tblVoucher()
                {
                    FK_Voucher_Rent_Other_Id =  voucherList.VoucherTypeId,
                    FK_Voucher_No_Id = voucherNo,
                    FK_Staff_Id =  _helper.GetUserLoginId(),
                    Rec_Date = Constraint.GetDate(),
                    Time_Voucher = Constraint.GetTime(),
                    Voucher_Name = row.Name,
                    Voucher_FK_Staff_Id = voucherList.StaffId,
                    Date_Voucher = voucherList.Date,
                    Rec_Time = voucherList.Time,
                    QTY = row.Qty,
                    Unit_Cost = row.Cost,
                    Amount = row.Amount,
                    Total =  row.Amount  + "",
                    Discount = row.Discount,
                    PCC = 0,
                    UoM = 0,
                    Voucher_Status = Status.Pending.ToString(),
                    Voucher_Picure = "",
                    Descr = voucherList.Desc
                };
                
                _connection.tblVouchers.Add(voucher);
                _connection.SaveChanges();
            }

            RemoveFile();
            return Json(new { message = "Created Successfully." , id = voucherNo});
        }
        
        // calculate discount
        public double GetDiscount(double amount = 0, double discount = 0)
        {
            return discount / 100 * amount;   
        }

        //get voucher no
        private int GetVoucherNo(string picture)
        {
            var voucherNo = new tblVoucher_No()
            {
                PK_Voucher_No_Id = 0,
                VoucherNo = 0,
                Picture = picture
            };
            _connection.tblVoucher_No.Add(voucherNo);
            _connection.SaveChanges();
            
            var id = voucherNo.PK_Voucher_No_Id;
            var update = _connection.tblVoucher_No.Single(no => no.PK_Voucher_No_Id == id);
            update.VoucherNo = id;
            _connection.SaveChanges();
            
            return id;
        }

        // get table voucher no 
        public ActionResult TableVoucherNo
        (
            int? page,
            int? pageSize,
            // boss
            int? bid = null,
            // company
            int? cid = null,
            // location
            int? lid = null,
            // staff id
            int? sid = null,
            // status
            string status = "",
            // date
            string start = "",
            string end = "",
            // voucher no
            int? voucherNo = null
        )
        {
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psizeNo = defaultPage;
            ViewBag.PageSizeNo = Constraint.PerPage;
            
            var voucherNos = _connection
                .GetAllVoucherNo(bid, cid, lid ,sid, status, start, end, voucherNo)
                .ToList()
                .ToPagedList(pageIndex, defaultPage);

            return PartialView(voucherNos);
        }

        // update status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateStatus(FormCollection collection)
        {
            var id = int.Parse(collection["voucherNo"]);
            var status = collection["status"];
            
            var voucher = _connection.tblVouchers.Where(v => v.FK_Voucher_No_Id == id).ToList();
            if (!voucher.Any()) return Json(new { error = "id not found."});

            if (!status.Equals("Done"))
            {
                foreach (var row in voucher)
                {
                    row.Confirm_PK_U_Id = _helper.GetUserLoginId();
                    row.Confirm_Date = Constraint.GetDate();
                    row.Confirm_time = Constraint.GetTime();
                    row.Voucher_Status = status;
                }
            }

            var voucherNo = _connection.tblVoucher_No.Single(no => no.PK_Voucher_No_Id == id);

            foreach (var item in voucher)
            {
                if (item.Confirm_PK_U_Id == null)
                {
                    return Json(new { error = "please checked this again."});
                }
            }
            
            if (status.Equals("Done"))
            {
                voucherNo.Aprove_PK_U_Id = _helper.GetUserLoginId();
                voucherNo.Aprove_Date = Constraint.GetDate();
                voucherNo.Aprove_Time = Constraint.GetTime();
                // set status done
                foreach (var row in voucher)
                {
                    row.Voucher_Status = status;
                }
            }
            
            _connection.SaveChanges();
            
            return Json(new { message = "Updated successfully."});
        }
        
        public ActionResult Edit(int? id)
        {
            ViewBag.Id = id;

            ViewBag.Manager = _connection.tblBosses.ToList();
            
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetVoucherById(int id)
        {
            if (id < 0)
            {
                return Json(new{error = "id not found."});
            }

            var voucherNo = _connection.tblVoucher_No.Single(s=>s.PK_Voucher_No_Id == id);
            
            var voucherEntity = _connection.tblVouchers.FirstOrDefault(voucher => voucher.FK_Voucher_No_Id == id);

            if (voucherEntity == null) return Json(new {error = "id find not found."});
            {
                // type voucher
                var type = voucherEntity.tblVoucher_Rent_Invertory_Rent_And_Other_Voucher.Type;
                
                // rent id
                var rentId = voucherEntity.FK_Voucher_Rent_Other_Id;
                // location id
                var locationId = _connection.tblVoucher_Rent_Invertory_Rent_And_Other_Voucher
                    .Single(r=> r.PK_Voucher_Rent_Other_Id == voucherEntity.FK_Voucher_Rent_Other_Id)
                    .FK_loc_id;
                // company id
                var companyId = _connection.tblLocations.Single(l => l.PK_Location_Id == locationId).FK_Comp_Id;
                // manager id
                var managerId = _connection.tblCompanies.Single(c=>c.PK_Comp_Id == locationId).FK_Boss_Id;
                
                // company entity
                var company = _connection
                    .tblCompanies
                    .Where(c => c.FK_Boss_Id == managerId)
                    .Select(c=> new{ key= c.PK_Comp_Id , value = c.Comp_Name})
                    .ToList();
                // location entity
                var location = _connection
                    .tblLocations
                    .Where(l => l.FK_Comp_Id == companyId)
                    .Select(l=> new { key = l.PK_Location_Id , value = l.Loc_Name})
                    .ToList();

                // department entity
                var department = _connection
                    .tblDepartments
                    .Where(d=> d.FK_Loc_Id == locationId)
                    .Select(d=> new{ key = d.PK_Depart_Id , value = d.Deppart_Name})
                    .ToList();
                // position entity
                var positionId = department[0].key;
                var position = _connection
                    .tblPositions
                    .Where(p=> p.FK_Depart_Id == positionId)
                    .Select(p=> new{key = p.PK_Pos_Id, value=p.Pos_Name})
                    .ToList();
                var departmentId = _connection.tblPositions.Single(d=>d.PK_Pos_Id == positionId).FK_Depart_Id;
                
                // staff entity
                var staffId = voucherEntity.Voucher_FK_Staff_Id;
                var staffPosId = _connection
                    .tblStaffs
                    .Single(s=>s.PK_Staff_Id == staffId).FK_Pos_Id;

                var staff = _connection
                    .tblStaffs
                    .Where(s => s.FK_Pos_Id == staffPosId)
                    .Select(s=> new
                    {
                        key = (int) s.PK_Staff_Id , 
                        value = s.Staff_Name.Trim()
                    })
                    .ToList();

                var voucherType = _connection
                    .tblVoucher_Rent_Invertory_Rent_And_Other_Voucher
                    .Where(l => l.FK_loc_id == locationId && l.Type == type)
                    .Select(l=> new
                    {
                        key = l.PK_Voucher_Rent_Other_Id,
                        value = l.Name
                    })
                    .ToList();

                var time = voucherEntity.Time_Voucher;
                var date = voucherEntity.Date_Voucher;
                // list voucher
                var voucherLists = _connection
                    .tblVouchers
                    .Where(v=>v.FK_Voucher_No_Id == id)
                    .Select(v => new
                    {
                        id = v.PK_Voucher_Id, 
                        name = v.Voucher_Name,
                        qty = v.QTY, 
                        cost = v.Unit_Cost, 
                        amount = v.Amount,
                        total = v.Total,
                        discount = v.Discount   
                    })
                    .ToList();
                
                var desc = _connection
                    .tblVouchers
                    .FirstOrDefault(v=>v.FK_Voucher_No_Id == id)
                    ?.Descr;

                return Json(new
                {
                    managerId, companyId,
                    company, locationId ,
                    location ,department , departmentId,
                    positionId = staffPosId , position,
                    staffId ,staff, 
                    voucherType, rentId,
                    date, time,
                    picture = voucherNo.Picture,
                    voucherLists,
                    desc,
                    type
                });
            }

        }
        
        // update voucher
        [HttpPost]
        public JsonResult Update
        (
            List<VoucherModel> voucherModels,
            VoucherList listVouchers,
            int voucherNo
        )
        {
            var staffId = listVouchers.StaffId;
            var date = listVouchers.Date;
            var time = listVouchers.Time;
            var photo = listVouchers.PhotoVoucher;
            var desc = listVouchers.Desc;
            var rentId = listVouchers.VoucherTypeId;
            var voucherNoId = voucherNo;
            
            // FIND 
            var voucherNoEntity = _connection
                .tblVoucher_No
                .Single(v=> v.PK_Voucher_No_Id == voucherNoId);

            if (voucherNoEntity == null) return Json(new{error = "id not found."});
            
            voucherNoEntity.Picture = photo;
            _connection.SaveChanges();

            var listVoucherId = new List<long>();

            var voucherFirstRow = _connection.tblVouchers.First(v=> v.FK_Voucher_No_Id == voucherNoId);
            
            foreach (var item in voucherModels)
            {
                if (item.Id > 0)
                {
                    var voucherEntity = _connection
                        .tblVouchers
                        .Single(voucher =>
                            voucher.PK_Voucher_Id == item.Id
                        );
                    
                    voucherEntity.FK_Voucher_Rent_Other_Id = rentId;
                    voucherEntity.FK_Staff_Id = _helper.GetUserLoginId();
                    voucherEntity.Voucher_Name = item.Name;
                    voucherEntity.Voucher_FK_Staff_Id = staffId;
                    voucherEntity.Date_Voucher = date;
                    voucherEntity.Time_Voucher = time;
                    voucherEntity.QTY = item.Qty;
                    voucherEntity.Unit_Cost = item.Cost;
                    voucherEntity.Amount = item.Amount;
                    voucherEntity.Total = item.Amount + "";
                    voucherEntity.Discount = item.Discount;
                    voucherEntity.Descr = desc;

                    _connection.SaveChanges();
                    listVoucherId.Add(item.Id);
                }
                else if (item.Id == 0)
                {
                    var voucher = new tblVoucher()
                    {
                        FK_Voucher_Rent_Other_Id = rentId,
                        FK_Voucher_No_Id = voucherNoId,
                        FK_Staff_Id = _helper.GetUserLoginId(),
                        Voucher_Name = item.Name,
                        Voucher_FK_Staff_Id = staffId,
                        Date_Voucher = date,
                        Time_Voucher = time,
                        QTY = item.Qty,
                        Unit_Cost = item.Cost,
                        Amount = item.Amount,
                        Total = item.Amount + "",
                        Discount = item.Discount,
                        UoM = 0,
                        PCC = 0,
                        Voucher_Status = voucherFirstRow.Voucher_Status,
                        Descr = desc,
                        Confirm_PK_U_Id = voucherFirstRow.Confirm_PK_U_Id,
                        Confirm_Date = voucherFirstRow.Confirm_Date,
                        Confirm_time = voucherFirstRow.Confirm_time,
                        Rec_Date = voucherFirstRow.Rec_Date,
                        Rec_Time = voucherFirstRow.Rec_Time,
                    };
                    
                    _connection.tblVouchers.Add(voucher);
                    _connection.SaveChanges();
                    listVoucherId.Add(voucher.PK_Voucher_Id);
                }
            }


            DeleteVoucher(listVoucherId, voucherNoId);
            
            return new JsonResult(){Data = "Updated successfully."};
        }

        // delete voucher 
        private void DeleteVoucher(List<long> list, int voucherNoId)
        {
            if (list.Count <= 0) return;
            var voucher = _connection
                .tblVouchers
                .Where(v => v.FK_Voucher_No_Id == voucherNoId)
                .Select(v=> v.PK_Voucher_Id)
                .ToList();

            foreach (var item in voucher)
            {
                var exist = false;
                foreach (var i in list)
                {
                    if (item == i)
                    {
                        exist = true;
                    }
                }

                if (exist == false)
                {
                    var delete = _connection
                        .tblVouchers
                        .Single(d=>d.PK_Voucher_Id == item);
                    _connection.tblVouchers.Remove(delete);
                    _connection.SaveChanges();
                }
            }
        }

        // Remove File Name 
        private void RemoveFile()
        {
            var dir = HttpContext.Server.MapPath("~/Content/Uploads/Voucher/");

            var filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/Voucher/"));

            var fileInPath = new List<string>();

            foreach (var file in filePaths)
            {
                fileInPath.Add(Path.GetFileName(file));
            }            
            // Remove
            foreach (var fileName in fileInPath) {

                var nameInDb = _connection
                    .tblVoucher_No
                    .Where(s =>s.Picture == fileName);

                if (!nameInDb.Any())
                {
                    var fileInfo = new FileInfo(dir + fileName);

                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }
            }            
        }
        
        // get voucher by location
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetVoucherRent(FormCollection form)
        {
            var locationId = int.Parse(form["locationId"]);
            var voucherType = form["voucherType"];
            
            if (locationId < 1 || voucherType == "")
            {
                return Json(null);
            }

            var voucherRent = _connection
                .tblVoucher_Rent_Invertory_Rent_And_Other_Voucher
                .Where(voucher => voucher.FK_loc_id == locationId && voucher.Type == voucherType)
                .Select(voucher => new
                {
                   key = voucher.PK_Voucher_Rent_Other_Id,
                   value = voucher.Name
                })
                .Distinct()
                .ToList();
            
            return Json(voucherRent);
        }
    }
}