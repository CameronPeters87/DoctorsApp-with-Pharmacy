using IronBarCode;
using Sprint33.Extensions;
using Sprint33.Models;
using System;
using System.Drawing;
using System.IO;
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
                ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name"),
                MedicineName = "",
                Instructions = "",
                PackSize = ""
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

            model.Instructions = "";
            model.MedicineName = "";
            model.PackSize = "";

            return View("Create", model);
        }

        [HttpPost]
        public async Task<string> Complete(CreatePrescriptionModel model)
        {
            if (!ModelState.IsValid || model.ExpiryDate < DateTime.Today)
            {
                model.PatientID = model.PatientID;
                model.Patient = db.Patients.Where(p => p.UserID == model.PatientID).FirstOrDefault();
                model.PrescriptionDetails = db.PrescriptionDetails.Where(d => d.PatientId == model.PatientID &&
                    d.PrescriptionId == null);
                model.ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name");

                return "Failed";
            }

            // Barcode
            // Generate a new random GUID using System.Guid class
            Guid barcodeGuid = Guid.NewGuid();
            string barcode = barcodeGuid.ToString();

            // Saving the signature as image
            string signaturePath;
            MemoryStream ms = new MemoryStream(model.Signature);
            Image returnImage = Image.FromStream(ms);
            returnImage.Save(Server.MapPath("~/Files/Signatures/" + barcode + ".png"), System.Drawing.Imaging.ImageFormat.Png);
            signaturePath = "/Files/Signatures/" + barcode + ".png";

            /* 
             * Generate Barcode
             */

            // Save Barcode to that path
            string _barcodePath = Path.Combine(Server
                .MapPath("~/Files/Barcodes/Prescriptions"), barcode + ".png");

            // Generate a Simple BarCode image and save as PNG
            //using IronBarCode;
            GeneratedBarcode MyBarCode = BarcodeWriter
                .CreateBarcode(barcode,
                BarcodeWriterEncoding.Code128);

            MyBarCode.SetMargins(25);

            MyBarCode.SaveAsPng(_barcodePath);

            string _barcodeUrl = "/Files/Barcodes/Prescriptions/" + barcode + ".png";

            // Get list of prescription details
            var details = db.PrescriptionDetails.Where(d => d.PatientId == model.PatientID &&
                d.PrescriptionId == null).ToList();

            // Patient
            var patient = db.Patients.Find(model.PatientID);

            db.Prescriptions.Add(new Prescription
            {
                Barcode = barcode,
                BarcodeUrl = _barcodeUrl,
                DateIssued = DateTime.Now,
                Patient = patient,
                PatientID = model.PatientID,
                PrescriptionDetails = details,
                PrescriptionValid = model.ExpiryDate,
                SignatureUrl = signaturePath
            });

            await db.SaveChangesAsync();

            //var lastPrescription = await db.Prescriptions.OrderByDescending(s => s.Id)
            //    .FirstOrDefaultAsync();

            return barcode;

            //return RedirectToAction("PrescriptionToPdf", new { id = lastPrescription.Id });
        }

        public async Task<ActionResult> Prescription(int id)
        {

            var model = await db.Prescriptions.FindAsync(id);

            return View(model);
        }

        public ActionResult ViewPrescription(string id)
        {
            var prescription = db.Prescriptions.Where(p => p.Barcode == id).FirstOrDefault();

            return View(prescription);
        }

        [ActionName("document-preview")]
        public ActionResult PrescriptionToPdf(int id)
        {
            var result = new Rotativa.ActionAsPdf("Prescription", new { id = id });
            return result;
        }

        public ActionResult PrescriptionHistory()
        {
            var patientId = Convert.ToInt32(Session["id"]);
            var prescriptions = db.Prescriptions.Where(p => p.PatientID == patientId)
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(prescriptions);
        }

        public ActionResult All()
        {
            var prescriptions = db.Prescriptions
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(prescriptions);
        }

        public ActionResult Send(string id)
        {
            var prescription = db.Prescriptions.Where(p => p.Barcode == id).FirstOrDefault();
            string link = "/prescriptions/document-preview?id=" + prescription.Id.ToString();
            db.Notifications.PushPrescriptionNotificaiton(link);

            db.SaveChanges();

            TempData["Sent"] = "You successfully sent the prescription to the pharmacist.";

            return RedirectToAction("ViewPrescription", new { id = prescription.Barcode });
        }

        public ActionResult TestPrescriptionLayout()
        {
            return View();
        }

    }
}
