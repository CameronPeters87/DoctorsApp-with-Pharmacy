using System.Web.Mvc;

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
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}