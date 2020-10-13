using Sprint33.Models;
using System.Linq;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class DeliveriesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Pharmacist/Deliveries
        public ActionResult Index()
        {
            var orders = db.CustomerOrders.Where(o => o.OrderStatus.ProcessNumber == 4).OrderByDescending(o => o.Id).ToList();
            return View(orders);
        }

        public ActionResult Directions()
        {
            return View();
        }
    }
}