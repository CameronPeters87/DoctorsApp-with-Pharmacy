using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist
{
    public class PharmacistAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Pharmacist";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Pharmacist_ChangeOrderStatus",
                "Pharmacist/{controller}/{action}/{stockOrderId}/{orderStatusId}",
                new
                {
                    controller = "StockOrders",
                    action = "ChangeOrderStatus",
                    id = UrlParameter.Optional
                }
            );

            context.MapRoute(
                "Pharmacist_default",
                "Pharmacist/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}