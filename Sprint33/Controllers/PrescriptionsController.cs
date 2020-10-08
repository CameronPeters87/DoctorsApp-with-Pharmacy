using Sprint33.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<ActionResult> AddDetail(CreatePrescriptionModel model)
        {
            if (!ModelState.IsValid)
            {
                model.PatientID = model.PatientID;
                model.Patient = db.Patients.Where(p => p.UserID == model.PatientID).FirstOrDefault();
                model.PrescriptionDetails = db.PrescriptionDetails.Where(d => d.PatientId == model.PatientID &&
                    d.PrescriptionId == null);
                model.ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name");

                return View("Create", model);
            }

            db.PrescriptionDetails.Add(new PrescriptionDetail
            {
                Instructions = model.Instructions,
                MedicineName = model.MedicineName,
                PackSize = model.PackSize,
                Patient = db.Patients.Where(p => p.UserID == model.PatientID).FirstOrDefault(),
                PatientId = model.PatientID
            });

            await db.SaveChangesAsync();

            model.PatientID = model.PatientID;
            model.Patient = db.Patients.Where(p => p.UserID == model.PatientID).FirstOrDefault();
            model.PrescriptionDetails = db.PrescriptionDetails.Where(d => d.PatientId == model.PatientID &&
                d.PrescriptionId == null);
            model.ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name");


            return View("Create", model);
        }

        [HttpPost]
        public async Task<ActionResult> Complete(CreatePrescriptionModel model)
        {
            if (!ModelState.IsValid)
            {
                model.PatientID = model.PatientID;
                model.Patient = db.Patients.Where(p => p.UserID == model.PatientID).FirstOrDefault();
                model.PrescriptionDetails = db.PrescriptionDetails.Where(d => d.PatientId == model.PatientID &&
                    d.PrescriptionId == null);
                model.ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name");

                return View("Create", model);
            }

            await db.SaveChangesAsync();

            var lastPrescription = await db.Prescriptions.OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            return Redirect("/");

            //return RedirectToAction("PrescriptionToPdf", new { id = lastPrescription.Id });
        }

        public async Task<ActionResult> Prescription(int id)
        {

            var model = await db.Prescriptions.FindAsync(id);

            return View(model);
        }


        public ActionResult PrescriptionToPdf(int id)
        {
            var result = new Rotativa.ActionAsPdf("Prescription", new { id = id });
            return result;
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
