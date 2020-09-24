using Microsoft.Owin;
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

                    db.SaveChanges();
                }

                if (!db.OrderStatuses.Any())
                {
                    statusList.Add(new OrderStatus
                    {
                        Name = "Pending Payment",
                        Description = "You need to make payment for the order to be processed",
                        Color = "secondary",
                        Icon = "fa fa-clock-o",
                        isPaid = false
                    });

                    statusList.Add(new OrderStatus
                    {
                        Name = "Processing",
                        Description = "Order is being processed",
                        Color = "info",
                        Icon = "fa fa-cog",
                        isPaid = true
                    });

                    statusList.Add(new OrderStatus
                    {
                        Name = "On Hold",
                        Description = "Your order is put on hold",
                        Color = "warning",
                        Icon = "fa fa-pause",
                        isPaid = true
                    });

                    statusList.Add(new OrderStatus
                    {
                        Name = "Completed",
                        Description = "Order recieved",
                        Color = "success",
                        Icon = "fa fa-check",
                        isPaid = true
                    });

                    statusList.Add(new OrderStatus
                    {
                        Name = "On it's way!",
                        Description = "Your Order is being delivered",
                        Color = "warning",
                        Icon = "fa fa-truck",
                        isPaid = true
                    });
                }
            }
        }
    }
}
