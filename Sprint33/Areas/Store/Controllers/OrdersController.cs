using Sprint33.Models;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class OrdersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Store/Orders
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("view-order")]
        public ActionResult ViewOrder(int id)
        {
            var order = db.CustomerOrders.Find(id);

            return View("ViewOrder", order);
        }
    }
}