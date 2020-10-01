using Sprint33.Models;
using System.Web.Mvc;

namespace Sprint33.Controllers
{
    public class PrescriptionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();







        //// GET: Prescriptions
        //public ActionResult Index(string search)
        //{

        //    return View(db.Prescriptions.Where(x => x.PatientName.StartsWith(search) || search == null).ToList());
        //}
        // GET: Prescriptions

        public ActionResult TestPrescriptionLayout()
        {
            return View();
        }

    }
}
