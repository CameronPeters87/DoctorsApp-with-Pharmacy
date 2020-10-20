using PagedList;
using Sprint33.Models;
using Sprint33.Models.ViewModel;
using Sprint33.PharmacyEntities;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Controllers
{
    public class Patients1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Patients1
        public ActionResult Homepage()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        // GET: Patients1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients1/Create
        public ActionResult Create()
        {
            var model = new PatientVM();
            return View(model);
        }

        // POST: Patients1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PatientVM model)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(new Address
                {
                    Street_Number = model.Address.Street_Number,
                    City = model.Address.City,
                    Country = model.Address.Country,
                    Province = model.Address.Province,
                    Route = model.Address.Route,
                    ZipCode = model.Address.ZipCode
                });

                await db.SaveChangesAsync();

                var patientAddress = await db.Addresses.OrderByDescending(a => a.Id)
                    .FirstOrDefaultAsync();

                db.Patients.Add(new Patient
                {
                    FirstName = model.FirstName,
                    Surname = model.Surname,
                    Address = patientAddress,
                    ContactNumber = model.ContactNumber,
                    AddressId = patientAddress.Id,
                    Age = model.Age,
                    Email = model.Email,
                    Password = model.Password,
                    isActive = true,
                    isLoyal = false
                });

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Patients1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,Surname,Age,ContactNumber,Email,Password,isActive")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
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
        //Code

        //Doctor Side
        public ActionResult DoctorIndex()
        {
            return View(db.Patients.ToList());
        }
        public ActionResult DoctorEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorEdit([Bind(Include = "UserID,FirstName,Surname,Age,ContactNumber,Email,Password,isActive")] Patient patient)
        {
            if (ModelState.IsValid)
            {

                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DoctorIndex");
            }
            return View(patient);
        }
        public ActionResult DoctorDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }
        //End of Doctor Side//

        //Admin Side
        public ActionResult AdminIndex()
        {
            return View(db.Patients.ToList());
        }
        public ActionResult AdminEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEdit(Patient patient)
        {
            if (ModelState.IsValid)
            {

                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            return View(patient);
        }
        public ActionResult AdminDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }
        //End of Admin side//

        public async Task<ActionResult> PharmacyOrders(int patientId, int? page)
        {
            var pageNumber = page ?? 1;

            var model = await db.CustomerOrders.Where(o => o.CustomerId == patientId)
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            var onePageOfProducts = model.ToPagedList(pageNumber, 10);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(model);
        }
    }
}
