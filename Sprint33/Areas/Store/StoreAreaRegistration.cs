﻿using System.Web.Mvc;

namespace Sprint33.Areas.Store
{
    public class StoreAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Store";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Store_Pay",
            //    "Store/{controller}/{action}/{id}",
            //    new { action = "Pay", id = UrlParameter.Optional },
            //    new[] { "Sprint33.Areas.Store.Controllers" }
            //);


            //context.MapRoute("Orders",
            //"Store/Orders/{action}/{name}/{id}",
            //new
            //{
            //    controller = "Orders",
            //    action = "view-order"
            //});


            context.MapRoute("Checkout",
            "Store/Checkout/{action}/{orderId}/{customerId}",
            new
            {
                controller = "Checkout",
                action = "Index"
            });

            context.MapRoute("Shop",
            "Store/Shop/{action}/{slug}",
            new
            {
                controller = "Shop",
                action = "Index",
                slug = UrlParameter.Optional
            });

            context.MapRoute(
                "Store_default",
                "Store/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Sprint33.Areas.Store.Controllers" }
            );
        }
    }
}