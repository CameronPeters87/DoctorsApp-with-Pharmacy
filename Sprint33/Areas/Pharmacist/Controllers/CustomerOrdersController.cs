using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using System.Linq;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class CustomerOrdersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pharmacist/CustomerOrders
        public ActionResult Index()
        {
            var model = (from o in db.CustomerOrders
                         orderby o.Id descending
                         select new CustomerOrderModel
                         {
                             FirstName = o.FirstName,
                             Id = o.Id,
                             Surname = o.Surname,
                             Address = o.Address,
                             CartItems = db.CustomerCarts.Where(c => c.CustomerOrderId == o.Id),
                             City = o.City,
                             Country = o.Country,
                             CouponId = o.CouponId,
                             CustomerId = o.CustomerId,
                             Email = o.Email,
                             Notes = o.Notes,
                             OrderDate = o.OrderDate,
                             OrderStatusId = o.OrderStatusId,
                             PaymentMethod = o.PaymentMethod,
                             PhoneNumber = o.PhoneNumber,
                             TotalCost = o.TotalCost,
                             TotalTax = o.TotalTax,
                             ZipCode = o.ZipCode,
                             OrderStatus = o.OrderStatus,
                             Customer = o.Customer
                         }).ToList();


            ViewBag.OrderStatus = db.OrderStatuses.ToList();

            return View(model);
        }

        public ActionResult ChangeOrderStatus(int orderId, int statusId)
        {
            var order = db.CustomerOrders.Find(orderId);
            var status = db.OrderStatuses.Find(statusId);

            order.OrderStatusId = statusId;
            order.OrderStatus = status;

            db.Entry(order).State = System.Data.Entity.EntityState.Modified;

            db.Notifications.PushNotificaiton(string.Format("You changed Customer Order #{0} status", orderId));


            db.SaveChanges();

            EmailExtensions.SendMail(order.Email, "Doctor J Govender: Order #" + order.Id,
                "Order Status changed to " + status.Name);

            return RedirectToAction("Index");
        }
    }
}