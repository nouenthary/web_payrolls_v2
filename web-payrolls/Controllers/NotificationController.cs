using System;
using System.Linq;
using System.Web.Mvc;
using web_payrolls.Models;
using PagedList;
using web_payrolls.Helpers;

namespace web_payrolls.Controllers
{
    public class NotificationController : Controller
    {
        private readonly DB_Connection _connection = new DB_Connection();

        private readonly ClHelper _helper = new ClHelper();
        // Get: Notification
        public ActionResult Index() {
            return View();
        }

        // Get Table
        public ActionResult GetTable(int? page, int? pageSize) {

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var defaultPage = (pageSize ?? Constraint.PageSize);
            ViewBag.psize = defaultPage;

            ViewBag.PageSize = Constraint.PerPage;

            var data = _connection
                       .tblNotifications
                       .ToList()
                       .OrderByDescending(n => n.PK_Notification_ID)
                       .ToPagedList(pageIndex, defaultPage);

            return PartialView(data);
        }

        // Post: Notification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetAlert(
            tblNotification entity,
            FormCollection form
        )
        {
            var optionKey = form["optionKey"];
            var exist = _connection.tblNotifications.Any(x=> x.Option_Key == optionKey);

            if (exist)
            {
              return Json(new { message = "Update successfully."});
            }

            entity.FK_Staff_Id = _helper.GetUserLoginId();
            entity.Link = form["link"];
            entity.Notification = form["note"];
            entity.Notification_Pending = Status.Pending.ToString();
            entity.Date_Update = Constraint.GetDate();
            entity.Time_Update = Constraint.GetTime();
            entity.Option_Key = optionKey;
            _connection.tblNotifications.Add(entity);
            _connection.SaveChanges();

            return Json(new {message = "Created successfully." });
        }

        // Get
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetNotification()
        {
            var data = _connection
                .tblNotifications
                .Where(n => n.Notification_Pending.Contains(Status.Pending.ToString()))
                .Select(n => new
                {
                    n.PK_Notification_ID,
                    n.Link,
                    n.Notification,
                    n.Notification_Pending,
                    n.Date_Update,
                    n.Time_Update,
                    n.tblStaff.PK_Staff_Id,
                    n.tblStaff.Staff_Name,
                    n.tblStaff.Photo
                })
                .OrderByDescending(n => n.PK_Notification_ID)
                .ToList();
            return Json(data);
        }

        // Update Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(FormCollection form) {
            var optionKey = form["optionKey"];

            var notification = _connection
                .tblNotifications
                .Single(n => n.Option_Key.Equals(optionKey));

            if (notification == null) {
                return Json(new { message = "status update failed." });
            }
            notification.Notification_Pending = form["status"];

            _connection.SaveChanges();

            return Json(new {message = "Updated successfully." });
        }
    }
}
