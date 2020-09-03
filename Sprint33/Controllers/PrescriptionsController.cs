using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Sprint33.Models;

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
        public ActionResult Index2(string search)
        {
            var model = (from p in db.Prescriptions
                         where p.PatientName.StartsWith(search) || search == null
                         select new PrescriptionListViewModel
                         {
                             PatientName = p.PatientName,
                             DateIssued = p.DateIssued,
                             Inventories = db.Inventorys.ToList(),
                             InventoryId = p.InventoryId,
                             PrescriptionValid = p.PrescriptionValid,
                             PrescriptionDetails = p.PrescriptionDetails,
                             Id = p.PrescriptionId,
                             itemQuantity = p.itemQuantity
                         }).ToList();

            return View(model);
        }

        // GET: Prescriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // GET: Prescriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prescription prescription = db.Prescriptions.Find(id);
            db.Prescriptions.Remove(prescription);
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

        [HttpGet]
        public ActionResult AddPrescription(int patientId)
        {
            if (patientId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //if (prescription == null)
            //{
            //    return HttpNotFound();
            //}
            var model = (from a in db.Appointments
                         join p in db.Patients on a.PatientID equals p.UserID
                         //join ps in db.Prescriptions on p.UserID equals ps.PatientID
                         where a.PatientID == patientId
                         select new PatientPrescriptionViewModel
                         {
                             PatientId = p.UserID,
                             AppointmentId = a.AppointmentID,
                             PatientName = p.FirstName,
                             Age = p.Age,
                             DateIssued = DateTime.Now,
                             DoctorName = a.DoctorName,
                             Inventories = db.Inventorys.ToList()
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPrescription(PatientPrescriptionViewModel prescriptionViewModel)
        {
            prescriptionViewModel.Inventories = db.Inventorys.ToList();

            if (ModelState.IsValid)
            {
                var inventory = db.Inventorys.Where(i => i.ItemID.Equals(prescriptionViewModel.InventoryId))
                    .FirstOrDefault();
                try
                {
                    if(prescriptionViewModel.PrescriptionValid < DateTime.Now)
                    {
                        return View(prescriptionViewModel);
                    }

                    if(inventory.itemQuantity < prescriptionViewModel.itemQuantity)
                    {
                        ModelState.AddModelError(string.Empty,
                            "You do not have enough stock: " + inventory.itemName + " " +
                            inventory.itemQuantity);

                        return View(prescriptionViewModel);
                    }

                    db.Prescriptions.Add(new Prescription
                    {
                        DoctorName = prescriptionViewModel.DoctorName,
                        PatientName = prescriptionViewModel.PatientName,
                        DateIssued = prescriptionViewModel.DateIssued,
                        PrescriptionValid = prescriptionViewModel.PrescriptionValid,
                        InventoryId = prescriptionViewModel.InventoryId,
                        PatientID = prescriptionViewModel.PatientId,
                        PrescriptionDetails = prescriptionViewModel.PrescriptionDetails,
                        itemQuantity = prescriptionViewModel.itemQuantity
                    });

                    inventory.itemQuantity -= prescriptionViewModel.itemQuantity;

                    db.SaveChanges();
                    return RedirectToAction("Index2");
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return View(prescriptionViewModel);
        }

        public ActionResult PrescriptionHistory()
        {
            var prescriptions = db.Prescriptions.Include(p => p.Patient);
            
            var patientId = Convert.ToInt32(Session["id"]);

            var model = (from p in db.Prescriptions
                         where p.PatientID.Equals(patientId)
                         select new PrescriptionListViewModel
                         {
                             Id = p.PrescriptionId,
                             PatientName = p.PatientName,
                             DateIssued = p.DateIssued,
                             Inventories = db.Inventorys.ToList(),
                             InventoryId = p.InventoryId,
                             PrescriptionValid = p.PrescriptionValid,
                             itemQuantity = p.itemQuantity,
                             PrescriptionDetails = p.PrescriptionDetails
                         }).ToList();

            return View(model);
        }

        public ActionResult ViewPrescription(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            var model = new PrescriptionListViewModel
            {
                Id = prescription.PrescriptionId,
                PatientName = prescription.PatientName,
                PrescriptionValid = prescription.PrescriptionValid,
                PrescriptionDetails = prescription.PrescriptionDetails,
                DateIssued = prescription.DateIssued,
                InventoryId = prescription.InventoryId,
                Inventories = db.Inventorys.ToList(),
                itemQuantity = prescription.itemQuantity
            };
            return View(model);
        }
    }
}
