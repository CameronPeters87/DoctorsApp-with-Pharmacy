using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sprint33.Models;
using System.Web.Mvc.Html;
using Sprint33.Models.ViewModel;

namespace Sprint33.Controllers
{
    public class OrdersController : Controller
    {
        public static int getOrderIDs = 0;

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        public ActionResult AdminIndex()
        {
            var model = (from o in db.Orders
                         orderby o.OrderID descending
                         select new OrderVM
                         {
                             PatientName = o.PatientName,
                             OrderDate = o.OrderDate,
                             LoyaltyPurchase = o.LoyaltyPurchase,
                             TotalPrice = o.TotalPrice,
                             OrderID = o.OrderID,
                             PatientId = o.PatientId,
                             AppointmentId = o.AppointmentId,
                             isApproved = o.isApproved,
                             isRefund = o.isRefund
                         }).ToList();
            return View(model);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            
            if (order == null)
            {
                return HttpNotFound();
            }
            getOrderIDs = order.OrderID;
            return View(order);
        }


        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,UserID,DeliveryName,DeliveryAddress,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                //This view will help to load order records to payment page.
                return RedirectToAction("Index","Pay", new {id = order.OrderID });
            }

            return View(order);
        }
       
        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);          
            if (order == null)
            {
                return HttpNotFound();
            }
            getOrderIDs = order.OrderID;
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,UserID,DeliveryName,DeliveryAddress,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CreateOrder(int? id)
        {
            var test = Session["id"];
            Appointment appointment = db.Appointments.Find(id);
            Patient patient = db.Patients.Where(p => p.UserID.Equals(appointment.PatientID))
                .FirstOrDefault();
            Loyalty loyalty;
            int? OrderId;

            if (patient.isLoyal)
            {
                loyalty = db.Loyalties.Where(l => l.PatientId.Equals(patient.UserID))
                    .FirstOrDefault();
            } else
            {
                loyalty = new Loyalty();
            }

            // Check if an order already exists with same appointment id
            if(db.Orders.Any(o => o.AppointmentId.Equals(appointment.AppointmentID)))
            {
                Order order = db.Orders
                    .Where(o => o.AppointmentId.Equals(appointment.AppointmentID)).FirstOrDefault();
                OrderId = order.OrderID;

                order.LoyaltyPurchase = patient.isLoyal;

                db.Entry(order).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index2", "Pay", new { id = OrderId });
            } else
            {
                if (patient.isLoyal && loyalty.Loyalty_Points >= 100)
                {
                    db.Orders.Add(new Order
                    {
                        AppointmentId = appointment.AppointmentID,
                        OrderDate = DateTime.Now,
                        TotalPrice = 360,
                        PatientName = appointment.PatientName,
                        LoyaltyPurchase = patient.isLoyal,
                        PatientId = appointment.PatientID,
                        isApproved = false
                    });
                }
                else
                {
                    db.Orders.Add(new Order
                    {
                        AppointmentId = appointment.AppointmentID,
                        OrderDate = DateTime.Now,
                        TotalPrice = 400,
                        PatientName = appointment.PatientName,
                        LoyaltyPurchase = patient.isLoyal,
                        PatientId = appointment.PatientID,
                        isApproved = false
                    });
                }
            }

            db.SaveChanges();

            OrderId = (from o in db.Orders
                       where o.AppointmentId.Equals(appointment.AppointmentID)
                        select o.OrderID).FirstOrDefault();

            Session["orderId"] = OrderId;
            Session["AppointmentId"] = appointment.AppointmentID;

            return Redirect("/Pay/Index2?id=" + OrderId);
        }

        public ActionResult RefundsIndex()
        {
            var model = (from o in db.Orders
                         where o.isRefund.Equals(true)
                         select new OrderVM
                         {
                             PatientName = o.PatientName,
                             OrderDate = o.OrderDate,
                             LoyaltyPurchase = o.LoyaltyPurchase,
                             TotalPrice = o.TotalPrice,
                             OrderID = o.OrderID,
                             PatientId = o.PatientId,
                             AppointmentId = o.AppointmentId,
                             isApproved = o.isApproved,
                             isRefund = o.isRefund
                         }).ToList();
            return View(model);
        }
    }
}
