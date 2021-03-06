﻿using Microsoft.Owin;
using Owin;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(Sprint33.Startup))]
namespace Sprint33
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<OrderStatus> statusList = new List<OrderStatus>();
                if (!db.Doctors.Any())
                {
                    db.Doctors.Add(new Doctor
                    {
                        FirstName = "James",
                        Surname = "Govender",
                        Password = "1234",
                        Email = "doctor@gmail.com",
                        ContactNumber = 815600000
                    });
                }

                if (!db.Pharmacists.Any())
                {
                    db.Pharmacists.Add(new Pharmacist
                    {
                        FirstName = "Jane",
                        Surname = "Govender",
                        Password = "1234",
                        Email = "pharmacist@gmail.com",
                        ContactNumber = "0815600000"
                    });
                }

                if (!db.Admins.Any())
                {
                    db.Admins.Add(new Admin
                    {
                        FirstName = "Cameron",
                        Surname = "Peters",
                        Password = "1234",
                        Email = "admin@gmail.com"
                    });
                }

                if (!db.OrderStatuses.Any())
                {
                    statusList.Add(new OrderStatus
                    {
                        Name = "Pending Payment",
                        Description = "You need to make payment for the order to be processed",
                        Color = "secondary",
                        Icon = "fa fa-clock-o",
                        isPaid = false,
                        ProcessNumber = 1
                    });

                    statusList.Add(new OrderStatus
                    {
                        Name = "Processing",
                        Description = "Order is being processed",
                        Color = "primary",
                        Icon = "fa fa-cog",
                        isPaid = true,
                        ProcessNumber = 3
                    });

                    statusList.Add(new OrderStatus
                    {
                        Name = "On Hold",
                        Description = "Your order is put on hold",
                        Color = "warning",
                        Icon = "fa fa-pause",
                        isPaid = true,
                        ProcessNumber = 0
                    });

                    statusList.Add(new OrderStatus
                    {
                        Name = "Completed",
                        Description = "Order recieved",
                        Color = "success",
                        Icon = "fa fa-check",
                        isPaid = true,
                        ProcessNumber = 5
                    });

                    statusList.Add(new OrderStatus
                    {
                        Name = "On it's way!",
                        Description = "Your Order is being delivered",
                        Color = "warning",
                        Icon = "fa fa-truck",
                        isPaid = true,
                        ProcessNumber = 4
                    });

                    statusList.Add(new OrderStatus
                    {
                        Name = "Waiting For Response",
                        Description = "",
                        Color = "info",
                        Icon = "fa fa-clock-o",
                        isPaid = true,
                        ProcessNumber = 2
                    });

                    db.OrderStatuses.AddRange(statusList);
                }

                if (!db.LoyaltyPreferences.Any())
                {
                    db.LoyaltyPreferences.Add(new LoyaltyPreference
                    {
                        CouponCode = "LTY",
                        CouponDiscountRate = 15,
                        MonthsToExpiry = 1,
                        Subject = "Dr J Governder Pharmacy: Loyalty Coupon Gift!",
                        Body = "You recieved a loyalty coupon. Thank you for being a member",
                        PointsLimit = 100
                    });
                }

                db.SaveChanges();
            }
        }
    }
}
