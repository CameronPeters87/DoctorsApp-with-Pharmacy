using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using System.Linq;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class LoyaltyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Pharmacist/Loyalty
        public ActionResult Index()
        {
            var prefs = db.LoyaltyPreferences.FirstOrDefault();

            var model = new LoyaltyModel
            {
                CouponCode = prefs.CouponCode,
                CouponDiscountRate = prefs.CouponDiscountRate,
                MonthsToExpiry = prefs.MonthsToExpiry,
                Subject = prefs.Subject,
                Body = prefs.Body,
                Id = prefs.Id,
                PointsLimit = prefs.PointsLimit
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(LoyaltyModel model)
        {
            return View(model);
        }
    }
}