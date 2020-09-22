using Sprint33.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Controllers
{
    public class Referral_v2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Referral_v2
        public ActionResult Index(string search)
        {
            // return View(await db.Referral_V2s.ToListAsync());
            return View(db.Referral_V2s.Where(x => x.PatientName.StartsWith(search) || search == null).ToList());
        }
        public async Task<ActionResult> List()
        {
            return View(await db.Referral_V2s.ToListAsync());
        }

        // GET: Referral_v2/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referral_v2 referral_v2 = await db.Referral_V2s.FindAsync(id);
            if (referral_v2 == null)
            {
                return HttpNotFound();
            }
            return View(referral_v2);
        }

        // GET: Referral_v2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Referral_v2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "refferal_ID,referral_Doctors_Name,referral_doctor_Add,referral_doctor_num,referral_doctor_Email,refferal_Date,referral_ValidDate,refferal_Location,referral_Reasoning,PatientName,PatientID")] Referral_v2 referral_v2)
        {
            if ((ModelState.IsValid) && (referral_v2.referral_ValidDate > DateTime.Now))
            {
                referral_v2.refferal_Date = DateTime.Now;
                referral_v2.referral_Doctors_Name = "Dr. J Govender";
                referral_v2.referral_doctor_Add = "37 Magaliesberg Street, Shallcross, Durban, 4079";
                referral_v2.referral_doctor_num = "0314099088";
                referral_v2.referral_doctor_Email = "drjgovender@medis.co.za";
                db.Referral_V2s.Add(referral_v2);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(referral_v2);
        }

        // GET: Referral_v2/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referral_v2 referral_v2 = await db.Referral_V2s.FindAsync(id);
            if (referral_v2 == null)
            {
                return HttpNotFound();
            }
            return View(referral_v2);
        }

        // POST: Referral_v2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "refferal_ID,referral_Doctors_Name,referral_doctor_Add,referral_doctor_num,referral_doctor_Email,refferal_Date,referral_ValidDate,refferal_Location,referral_Reasoning,PatientName,PatientID")] Referral_v2 referral_v2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(referral_v2).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(referral_v2);
        }

        // GET: Referral_v2/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referral_v2 referral_v2 = await db.Referral_V2s.FindAsync(id);
            if (referral_v2 == null)
            {
                return HttpNotFound();
            }
            return View(referral_v2);
        }

        // POST: Referral_v2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Referral_v2 referral_v2 = await db.Referral_V2s.FindAsync(id);
            db.Referral_V2s.Remove(referral_v2);
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

        public async Task<ActionResult> AddReferral(int patientId)
        {
            var model = await (from p in db.Patients
                               where p.UserID == patientId
                               select new ReferralViewModel
                               {
                                   refferal_Date = DateTime.Now,
                                   referral_Doctors_Name = "Dr. J Govender",
                                   referral_doctor_Add = "37 Magaliesberg Street, Shallcross, Durban, 4079",
                                   referral_doctor_num = "0314099088",
                                   referral_doctor_Email = "drjgovender@medis.co.za",
                                   PatientID = p.UserID,
                                   PatientName = p.FirstName + " " + p.Surname
                               }).FirstOrDefaultAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReferral(ReferralViewModel referralViewModel)
        {
            if (ModelState.IsValid)
            {

                db.Referral_V2s.Add(new Referral_v2
                {
                    referral_Doctors_Name = referralViewModel.referral_Doctors_Name,
                    referral_doctor_Add = referralViewModel.referral_doctor_Add,
                    referral_doctor_num = referralViewModel.referral_doctor_num,
                    referral_doctor_Email = referralViewModel.referral_doctor_Email,
                    PatientName = referralViewModel.PatientName,
                    PatientID = referralViewModel.PatientID,
                    refferal_Date = referralViewModel.refferal_Date,
                    referral_ValidDate = referralViewModel.referral_ValidDate,
                    refferal_Location = referralViewModel.refferal_Location,
                    referral_Reasoning = referralViewModel.referral_Reasoning
                });
                db.SaveChanges();
                return RedirectToAction("Index", "Referral_v2");
            }
            return View(referralViewModel);
        }

    }
}
