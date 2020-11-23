using Sprint33.Models;
using Sprint33.Models.ViewModel;
using Sprint33.PharmacyEntities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult FrontPage()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Project Code From Here
        public ActionResult Login()
        {
            Session["UserName"] = "";
            Session["IsLoyal"] = false;
            Session["id"] = "";
            Session["Title"] = "";
            ViewBag.Message = "";
            return View(new Doctor());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserID,FirstName,Surname,ContactNumber,Email,Password")] Doctor doctors, [Bind(Include = "UserID,FirstName,Surname,Age,ContactNumber,Email,Password")] Patient patients, [Bind(Include = "UserID,FirstName,Surname,Email,Password")] Admin admins,
            [Bind(Include = "UserID,FirstName,Surname,Email,Password")] Pharmacist pharmacist)
        {


            ViewBag.Message = "Incorrect Username or Password";
            foreach (var item in db.Doctors.ToList())
            {
                if (doctors.Email == null || doctors.Password == null)
                {
                    return View(doctors);
                }
                else
                {
                    if (doctors.Email.Equals(item.Email) && doctors.Password.Equals(item.Password))
                    {
                        Session["id"] = item.UserID;
                        Session["UserName"] = item.FirstName + " " + item.Surname;
                        Session["Title"] = "Doctor";
                        return RedirectToAction("Homepage", "Doctors1");
                    }
                }
            }
            foreach (var item in db.Pharmacists.ToList())
            {
                if (pharmacist.Email == null || pharmacist.Password == null)
                {
                    return View(doctors);
                }
                else
                {
                    if (pharmacist.Email.Equals(item.Email) && pharmacist.Password.Equals(item.Password))
                    {
                        Session["id"] = item.UserID;
                        Session["UserName"] = item.FirstName + " " + item.Surname;
                        Session["Title"] = "Pharmacist";
                        return RedirectToAction("Index", "Dashboard", new { area = "pharmacist" });
                    }
                }
            }
            foreach (var item in db.Patients)
            {
                if (patients.Email.Equals(item.Email) && patients.Password.Equals(item.Password))
                {
                    Session["id"] = item.UserID;
                    Session["UserName"] = item.FirstName + " " + item.Surname;
                    Session["IsLoyal"] = item.isLoyal;
                    Session["Title"] = "Patient";
                    return RedirectToAction("Homepage", "Patients1");
                }
            }
            foreach (var item in db.Admins)
            {
                if (admins.Email.Equals(item.Email) && admins.Password.Equals(item.Password))
                {
                    Session["id"] = item.UserID;
                    Session["UserName"] = item.FirstName + " " + item.Surname;
                    Session["Title"] = "Admin";
                    return RedirectToAction("Homepage", "Admins1");
                }
            }

            return View(doctors);

        }
        public ActionResult ForgotPassword()
        {
            ViewBag.ID = "";
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword([Bind(Include = "FirstName , Surname , ContactNumber , Email")] Patient patient)
        {
            foreach (var item in db.Patients)
            {
                if (patient.FirstName == item.FirstName && patient.Surname == item.Surname && patient.ContactNumber == item.ContactNumber && patient.Email == item.Email)
                {

                    return RedirectToAction("ResetPassword", new { id = item.UserID });
                }
            }
            ViewBag.Message = "Recovery Details Incorrect Please try again";
            return View(patient);
        }
        public ActionResult ResetPassword(int? id)
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
        public ActionResult ResetPassword([Bind(Include = "UserID,FirstName,Surname,Age,ContactNumber,Email,Password")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(patient);
        }

        public ActionResult Register()
        {
            var model = new PatientVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(PatientVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

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
                isLoyal = false,
                DateRegistered = DateTime.Today
            });

            await db.SaveChangesAsync();
            return RedirectToAction("Login");

        }
    }
}