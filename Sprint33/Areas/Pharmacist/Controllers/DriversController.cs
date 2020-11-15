using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class DriversController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pharmacist/Drivers
        public ActionResult Index()
        {
            var model = new DriverModel
            {
                Drivers = db.Drivers.Where(d => d.Status != "Archived").ToList(),
                Name = "",
                ContactNumber = "",
                Email = ""
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(DriverModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Drivers = db.Drivers.Where(d => d.Status != "Archived").ToList();

                return View("Index", model);
            }

            db.Drivers.Add(new PharmacyEntities.Driver
            {
                Name = model.Name,
                Email = model.Email,
                ContactNumber = model.ContactNumber,
                Status = "Active"
            });

            await db.SaveChangesAsync();

            model.Drivers = db.Drivers.Where(d => d.Status != "Archived").ToList();

            return View("Index", model);
        }

        public async Task<ActionResult> ArchiveDriver(int id)
        {
            var driver = db.Drivers.Find(id);

            driver.Status = "Archived";
            db.Entry(driver).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public ActionResult Archives()
        {
            return View(db.Drivers.Where(d => d.Status == "Archived").ToList());
        }

        public async Task<ActionResult> Delete(int id)
        {
            var driver = db.Drivers.Find(id);

            db.Drivers.Remove(driver);
            await db.SaveChangesAsync();

            return RedirectToAction("Archives");
        }

        public async Task<ActionResult> Recover(int id)
        {
            var driver = db.Drivers.Find(id);

            driver.Status = "Active";
            db.Entry(driver).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Archives");
        }

    }
}