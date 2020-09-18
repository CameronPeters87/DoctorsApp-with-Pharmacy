﻿using Sprint33.Areas.Store.Models;
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
            var defaultStatus = db.OrderStatuses.Where(s => s.Name == "Pending Payment").FirstOrDefault();

            if (defaultStatus == null)
            {
                defaultStatus = OrderStatusExtensions.CreateOrderStatus(db, "Pending Payment",
                    "You need to make payment for the order to be processed",
                    "secondary", "fa fa-clock-o", false);
            }

            var totalCost = currentCart.GetTotalPrice();
            var tax = currentCart.GetTotalTax();

            db.CustomerOrders.Add(new CustomerOrder
            {
                Customer = patient,
                CustomerId = patientId,
                OrderStatus = defaultStatus,
                OrderStatusId = defaultStatus.Id,
                OrderDate = DateTime.Today,
                TotalCost = totalCost,
                TotalTax = tax,
                FirstName = patient.FirstName,
                Surname = patient.Surname,
                Address = string.Format("{0} {1}", patient.Address.Street_Number, patient.Address.Route),
                City = patient.Address.City,
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

            controller.Session["CustomerOrderId"] = lastOrder.Id;
        }

        public string PaymentApproved(HttpContextBase controller)
        {
            int orderId = GetOrderId(controller);

            var order = GetOrder(orderId);

            var orderStatus = db.OrderStatuses.Where(o => o.Name == "Processing").FirstOrDefault();

            if (orderStatus == null)
            {
                orderStatus = OrderStatusExtensions.CreateOrderStatus(db, "Processing",
                    "Order is being processed",
                    "info", "fa fa-cog", true);
            }

            order.OrderStatus = orderStatus;
            order.OrderStatusId = orderStatus.Id;

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            EmailExtensions.SendMail(order.Email, "Your order is being Processed!",
                "Your order has being successfully approved and is being processed.<br>" +
                "<strong>NB: Upon delivery, please open this link to scan the QR code to confirm delivery." +
                "<a href='#'>Here</a> </strong>");

            return "Approved";
        }

        public string PaymentDeclined(HttpContextBase controller)
        {
            int orderId = GetOrderId(controller);

            var order = GetOrder(orderId);

            var orderStatus = db.OrderStatuses.Where(o => o.Name == "Payment Pending").FirstOrDefault();

            if (orderStatus == null)
            {
                orderStatus = OrderStatusExtensions.CreateOrderStatus(db, "Paid: Processing",
                    "You need to make payment for the order to be processed",
                    "secondary", "fa fa-clock-o", true);
            }

            order.OrderStatus = orderStatus;
            order.OrderStatusId = order.Id;

            db.Entry(order).State = EntityState.Modified;
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
            order.Country = model.City;
            order.PhoneNumber = model.PhoneNumber;
            order.Address = model.Address;
            order.ZipCode = model.Address;
            order.PaymentMethod = model.PaymentMethod;

            db.Entry(order).State = EntityState.Modified;

            db.SaveChanges();
        }
    }
}