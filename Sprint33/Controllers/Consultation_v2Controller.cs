using Sprint33.Models;
using Sprint33.Models.ViewModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Controllers
{
    public class Consultation_v2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Consultation_v2
        public ActionResult Index()
        {
            var model = db.Consultation_V2s
               .ToArray()
               .OrderBy(c => c.PatientName)
               .Select(c => new ConsultationListVM(c))
               .ToList();

            return View(model);
        }

        // GET: Consultation_v2/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation_v2 consultation_v2 = await db.Consultation_V2s.FindAsync(id);
            if (consultation_v2 == null)
            {
                return HttpNotFound();
            }
            return View(consultation_v2);
        }

        // GET: Consultation_v2/Create
        public ActionResult Create(int appointmentId)
        {
            var model = (from a in db.Appointments
                         where a.AppointmentID.Equals(appointmentId)
                         select new CreateCosulationModel
                         {
                             AppointmentID = appointmentId,
                             AppointmentDate = a.AppointmentTime,
                             PatientName = a.PatientName,
                             PatientId = a.PatientID
                         }).FirstOrDefault();
            return View(model);
        }

        // POST: Consultation_v2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCosulationModel model)
        {
            if (ModelState.IsValid)
            {
                Consultation_v2 dto = new Consultation_v2
                {
                    PatientName = model.PatientName,
                    Notes = model.Notes,
                    Symptoms = model.Symptoms,
                    Diagnosis = model.Diagnosis,
                    AppointmentDate = model.AppointmentDate,
                    AppointmentID = model.AppointmentID,
                    PatientId = model.PatientId
                };

                db.Consultation_V2s.Add(dto);

                Appointment appointment = db.Appointments.Where(a =>
                    a.AppointmentID.Equals(model.AppointmentID)).FirstOrDefault();

                appointment.Complete = true;

                db.Entry(appointment).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<string> CreateConsultation(CreateCosulationModel model)
        {
            if (ModelState.IsValid)
            {
                Consultation_v2 dto = new Consultation_v2
                {
                    PatientName = model.PatientName,
                    Notes = model.Notes,
                    Symptoms = model.Symptoms,
                    Diagnosis = model.Diagnosis,
                    AppointmentDate = model.AppointmentDate,
                    AppointmentID = model.AppointmentID,
                    PatientId = model.PatientId
                };

                db.Consultation_V2s.Add(dto);

                Appointment appointment = db.Appointments.Where(a =>
                    a.AppointmentID.Equals(model.AppointmentID)).FirstOrDefault();

                appointment.Complete = true;

                db.Entry(appointment).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return "Success";
            }

            TempData["Error"] = "An error has occured. Try again";

            return "Error";
        }

        // GET: Consultation_v2/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation_v2 consultation_v2 = await db.Consultation_V2s.FindAsync(id);
            if (consultation_v2 == null)
            {
                return HttpNotFound();
            }
            return View(consultation_v2);
        }

        // POST: Consultation_v2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PatientName,AppointmentDate,Symptoms,Diagnosis,Notes,AppointmentID")] Consultation_v2 consultation_v2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultation_v2).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(consultation_v2);
        }

        // GET: Consultation_v2/Delete/5
        public async Task<ActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation_v2 consultation_v2 = await db.Consultation_V2s.FindAsync(id);
            if (consultation_v2 == null)
            {
                return HttpNotFound();
            }
            TempData["SM"] = "You have removed a consultation for " + consultation_v2.PatientName;

            db.Consultation_V2s.Remove(consultation_v2);
            await db.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        // POST: Consultation_v2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Consultation_v2 consultation_v2 = await db.Consultation_V2s.FindAsync(id);
            db.Consultation_V2s.Remove(consultation_v2);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Consultation_v2/Delete/5
        public async Task<ActionResult> PreviewConsultation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Consultation_v2 consultation_ = await db.Consultation_V2s.FindAsync(id);

            if (consultation_ == null)
            {
                return HttpNotFound();
            }

            return View(consultation_);
        }

        public async Task<ActionResult> PatientHistory()
        {
            var consultations = db.Consultation_V2s.Include(c => c.Appointment);

            return View(await consultations.ToListAsync());
        }
    }
}
