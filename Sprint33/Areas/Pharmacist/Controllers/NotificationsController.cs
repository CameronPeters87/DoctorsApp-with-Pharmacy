using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class NotificationsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Pharmacist/Notifications
        public async Task<ActionResult> Index()
        {
            // Change all unread notifications to read
            var notifications = await db.Notifications.Where(n => n.isRead == false)
                .ToListAsync();
            notifications.ForEach(n => n.isRead = true);
            await db.SaveChangesAsync();

            var model = await (from n in db.Notifications
                               orderby n.CreatedDate descending
                               where n.IsPrescriptionNotification == false
                               select new SingleNotificationVM
                               {
                                   Id = n.Id,
                                   Message = n.Message,
                                   CreatedDate = n.CreatedDate,
                                   isRead = n.isRead,
                                   Icon = n.Icon,
                                   BackgroundColorIcon = n.BackgroundColorIcon
                               }).ToListAsync();
            return View(model);
        }

        public ActionResult _NotificationSection()
        {
            var model = new NotificationVM();

            model.SingleNotification = (from n in db.Notifications
                                        where n.isRead == false
                                        orderby n.CreatedDate descending
                                        select new SingleNotificationVM
                                        {
                                            Id = n.Id,
                                            IsPrescriptionNotification = n.IsPrescriptionNotification,
                                            PrescriptionLink = n.PrescriptionLink,
                                            Message = n.Message,
                                            CreatedDate = n.CreatedDate,
                                            isRead = n.isRead,
                                            Icon = n.Icon,
                                            BackgroundColorIcon = n.BackgroundColorIcon
                                        }).ToList().Take(5);

            //model.NumberOfUnreadNotifications = db.Notifications
            //    .Where(n => n.isRead == false).Count();

            model.NumberOfUnreadNotifications = model.SingleNotification.Count();

            ViewBag.PresLink = "#";

            if (model.SingleNotification.Any(n => n.IsPrescriptionNotification == true))
            {
                foreach (var item in model.SingleNotification)
                {
                    if (item.IsPrescriptionNotification == true)
                    {
                        ViewBag.PresLink = item.PrescriptionLink;
                    }
                }
            }

            return PartialView(model);
        }
        [HttpPost]
        public async Task<int> MarkRead(int notificationId)
        {
            Notification notification = await db.Notifications.FindAsync(notificationId);
            //int notificationId = notification.Id;

            notification.isRead = true;

            db.Entry(notification).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return notificationId;
        }

        public ActionResult Clear()
        {
            var notifications = db.Notifications.ToList();

            db.Notifications.RemoveRange(notifications);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}