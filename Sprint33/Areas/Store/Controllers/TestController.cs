using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class TestController : Controller
    {
        // GET: Store/Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult CheckoutSecond()
        {
            return View();
        }
    }
}