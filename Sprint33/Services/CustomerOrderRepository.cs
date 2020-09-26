using Sprint33.Areas.Store.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using Sprint33.Services.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sprint33.Services
{
    public class CustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public void ApplyCoupon(Coupon coupon, CustomerOrder order)
        {
            order.CouponId = coupon.Id;
            order.TotalCost -= (order.TotalCost * coupon.DiscountRate);

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
        }

        public string GetCheckoutUrl(int orderId, int patientId)
        {
            return (string.Format("/store/checkout/index/{0}/{1}",
                orderId, patientId));
        }

        public CustomerOrder GetOrder(int orderId)
        {
            return db.CustomerOrders.Find(orderId);
        }

        public int GetOrderId(HttpContextBase controller)
        {
            return Convert.ToInt32(controller.Session["CustomerOrderId"]);
        }

        public void InitializeOrder(int patientId, Controller controller)
        {
            var patient = db.Patients.Find(patientId);

            var currentCart = db.CustomerCarts.GetCurrentCartItems(patientId);
            var defaultStatus = db.OrderStatuses.Where(s => s.Name == "Waiting").FirstOrDefault();

            var subTotal = currentCart.GetTotalPrice();
            var tax = currentCart.GetTotalTax();
            var totalCost = subTotal + tax;

            db.CustomerOrders.Add(new CustomerOrder
            {
                Customer = patient,
                CustomerId = patientId,
                OrderStatus = defaultStatus,
                OrderStatusId = defaultStatus.Id,
                OrderDate = DateTime.Today,
                SubTotal = subTotal,
                TotalCost = totalCost,
                TotalTax = tax,
                FirstName = patient.FirstName,
                Surname = patient.Surname,
                Address = string.Format("{0} {1}", patient.Address.Street_Number, patient.Address.Route),
                City = patient.Address.City,
                Province = patient.Address.Province,
                Country = patient.Address.Country,
                ZipCode = patient.Address.ZipCode,
                Email = patient.Email,
                PhoneNumber = patient.ContactNumber
            });

            db.SaveChanges();

            var lastOrder = db.CustomerOrders.GetLastOrder();

            foreach (var item in currentCart)
            {
                item.CustomerOrderId = lastOrder.Id;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }

            lastOrder.CustomerCarItems = db.CustomerCarts.Where(c => c.CustomerId == patientId
                && c.CustomerOrderId == lastOrder.Id).ToList();

            db.Entry(lastOrder).State = EntityState.Modified;
            db.SaveChanges();

            controller.Session["CustomerOrderId"] = lastOrder.Id;
        }

        public string PaymentApproved(HttpContextBase controller)
        {
            int orderId = GetOrderId(controller);

            var order = GetOrder(orderId);

            var orderStatus = db.OrderStatuses.Where(o => o.Name == "Waiting").FirstOrDefault();

            order.OrderStatus = orderStatus;
            order.OrderStatusId = orderStatus.Id;

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            EmailExtensions.SendMail(order.Email, "Your order is being Processed!",
                "Your order has being successfully approved and is being processed.<br>" +
                "<strong>NB: Upon delivery, Click here to confirm delivery or download our app from the play store?" +
                "<a href='#'>Here</a> </strong>");

            return "Approved";
        }

        public string PaymentDeclined(HttpContextBase controller)
        {
            int orderId = GetOrderId(controller);

            var order = GetOrder(orderId);

            db.CustomerOrders.Remove(order);

            db.SaveChanges();


            return "Declined";
        }

        public void UpdateBillingInfo(BillingForm model)
        {
            var order = db.CustomerOrders.Find(model.OrderId);

            order.FirstName = model.FirstName;
            order.Surname = model.Surname;
            order.Email = model.Email;
            order.City = model.City;
            order.Province = model.Province;
            order.Country = model.Country;
            order.PhoneNumber = model.PhoneNumber;
            order.Address = model.Address;
            order.ZipCode = model.ZipCode;
            order.PaymentMethod = model.PaymentMethod;

            db.Entry(order).State = EntityState.Modified;

            db.SaveChanges();
        }
    }
}