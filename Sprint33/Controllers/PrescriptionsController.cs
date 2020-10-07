using Sprint33.Models;
using System.Linq;
using System.Web.Mvc;

namespace Sprint33.Controllers
{
    public class PrescriptionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Create(int patientId)
        {
            var patient = db.Patients.Find(patientId);

            var model = new CreatePrescriptionModel
            {
                PatientID = patientId,
                Patient = patient,
                PrescriptionDetails = db.PrescriptionDetails.Where(d => d.PatientId == patientId &&
                    d.PrescriptionId == null),
                ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name")
            };

            return View(model);
        }





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
