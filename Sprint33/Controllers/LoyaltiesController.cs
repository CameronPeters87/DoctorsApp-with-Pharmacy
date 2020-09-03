using Sprint33.Models;
using Sprint33.Models.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Controllers
{
    public class LoyaltiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Loyalties
        public async Task<ActionResult> Index()
        {
            var model = await (from l in db.Loyalties
                               join p in db.Patients on l.PatientId equals p.UserID
                               select new LoyaltyListVM
                               {
                                   Id = l.Id,
                                   PatientName = p.FirstName + " " + p.Surname,
                                   Loyalty_Points = l.Loyalty_Points,
                                   Registered = l.Registered,
                                   ContactNumber = p.ContactNumber,
                                   PatientId = p.UserID
                               }).ToListAsync();
            return View(model);
        }
        // Post: Loyalties/RemovePatient?Loyalty
        public async Task<ActionResult> RemovePatient(int id)
        {
            Loyalty dto = await db.Loyalties.FindAsync(id);
            UpdatePatientRemove(dto.PatientId);
            db.Loyalties.Remove(dto);
            await db.SaveChangesAsync();
            TempData["SM"] = "You have removed a patient from loyalty list";
            return RedirectToAction("Index");
        }

        // GET: Loyalties/Subscribe?patientId
        public async Task<ActionResult> Subscribe(int patientId)
        {
            if (patientId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await (from p in db.Patients
                               where p.UserID.Equals(patientId)
                               select new LoyaltySubscribeVM
                               {
                                   PatientName = p.FirstName + " " + p.Surname,
                                   Registered = DateTime.Now
                               }).FirstOrDefaultAsync();

            return View(model);
        }
        // Post: Loyalties/Subscribe?patientId
        [HttpPost]
        public async Task<ActionResult> Subscribe(LoyaltySubscribeVM model)
        {
            if (ModelState.IsValid)
            {
                db.Loyalties.Add(new Loyalty
                {
                    Loyalty_Points = 0,
                    PatientId = model.PatientId,
                    Registered = DateTime.Now
                });

                UpdatePatient(model);

                await db.SaveChangesAsync();

                Session["UserName"] = "";
                Session["IsLoyal"] = false;
                Session["id"] = "";
                Session["Title"] = "";
                ViewBag.Message = "";

                return RedirectToAction("Congratulations");
            }
            return View(model);
        }
        public async Task<ActionResult> Congratulations()
        {
            return View();
        }

        public void UpdatePatient(LoyaltySubscribeVM model)
        {
            Patient patient = db.Patients.Find(model.PatientId);

            patient.isLoyal = true;

            db.Entry(patient).State = EntityState.Modified;
        }

        public void UpdatePatientRemove(int patientId)
        {
            Patient patient = db.Patients.Find(patientId);

            patient.isLoyal = false;

            db.Entry(patient).State = EntityState.Modified;
        }

    }
}