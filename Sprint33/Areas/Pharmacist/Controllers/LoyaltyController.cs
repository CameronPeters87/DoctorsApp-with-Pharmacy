using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class LoyaltyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Pharmacist/Loyalty
        [ActionName("prefs")]
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

            return View("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(LoyaltyModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var obj = db.LoyaltyPreferences.Find(model.Id);

            obj.MonthsToExpiry = model.MonthsToExpiry;
            obj.Subject = model.Subject;
            obj.Body = model.Body;
            obj.CouponCode = model.CouponCode;
            obj.CouponDiscountRate = model.CouponDiscountRate;
            obj.PointsLimit = model.PointsLimit;

            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;

            db.Notifications.PushNotificaiton(string.Format("You updated loyalty preferences"));

            await db.SaveChangesAsync();


            return View("Index", model);
        }
    }
}