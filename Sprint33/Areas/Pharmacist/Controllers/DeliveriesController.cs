using Geocoding;
using Geocoding.Google;
using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZXing;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class DeliveriesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyCPWzQ0h1vedStiQWFQ5Ez1Jf2f1rj209Q" };

        // GET: Pharmacist/Deliveries
        public async Task<ActionResult> Index()
        {
            var orders = db.CustomerOrders.Where(o => o.OrderStatus.ProcessNumber == 4).OrderByDescending(o => o.Id).ToList();

            string pharmacyAddress = "37 Magaliesberg St Durban South South Africa 4079";
            var _latitude = LocationExtensions.GetLatFromPlaceName(pharmacyAddress, geocoder);
            var _longitude = LocationExtensions.GetLongFromPlaceName(pharmacyAddress, geocoder);
            double destination_Lat;
            double destination_Long;

            var model = await (from o in db.Deliveries
                               where o.Status == "In Transit"
                               orderby o.Id descending
                               select new DeliveriesModel
                               {
                                   Delivery = o
                               }).ToListAsync();

            foreach (var item in model)
            {
                destination_Lat = LocationExtensions.GetLatFromPlaceName(item.Delivery.CustomerOrder.Address +
                    item.Delivery.CustomerOrder.City +
                    item.Delivery.CustomerOrder.Country +
                    item.Delivery.CustomerOrder.ZipCode,
                    geocoder);
                destination_Long = LocationExtensions.GetLongFromPlaceName(item.Delivery.CustomerOrder.Address +
                    item.Delivery.CustomerOrder.City +
                    item.Delivery.CustomerOrder.Country +
                    item.Delivery.CustomerOrder.ZipCode,
                    geocoder);
                item.Distance = LocationExtensions.GetDistance(_latitude, _longitude,
                    destination_Lat, destination_Long, 'K');
            }

            return View(model);
        }

        public ActionResult Directions(int? deliveryId)
        {
            var delivery = db.Deliveries.Find(deliveryId);

            if (delivery == null)
            {
                delivery = new Delivery();
            }

            var model = new DirectionsModel
            {
                Delivery = delivery
            };

            return View(model);
        }

        public ActionResult SetDelivery(int id)
        {
            var order = db.CustomerOrders.Find(id);

            var model = new SetDeliveryModel
            {
                CustomerOrder = order,
                OrderId = order.Id,
                Drivers = new SelectList(db.Drivers.Where(d => d.Status == "Active").ToList(), "Id", "Name")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SetDelivery(SetDeliveryModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CustomerOrder = db.CustomerOrders.Find(model.OrderId);
                model.OrderId = model.OrderId;
                model.Drivers = new SelectList(db.Drivers.Where(d => d.Status == "Active").ToList(), "Id", "Name");
                return View(model);
            }

            var driver = db.Drivers.Find(model.DriverId);
            driver.Status = "In Transit";
            db.Entry(driver).State = EntityState.Modified;

            var order = await db.CustomerOrders.FindAsync(model.OrderId);
            var orderStatus = await db.OrderStatuses.Where(o => o.ProcessNumber == 4).FirstOrDefaultAsync();
            order.OrderStatus = orderStatus;
            order.OrderStatusId = orderStatus.Id;
            db.Entry(order).State = EntityState.Modified;

            var qr_code_text = Guid.NewGuid();

            db.Deliveries.Add(new Delivery
            {
                CustomerOrder = db.CustomerOrders.Find(model.OrderId),
                OrderId = model.OrderId,
                DepartureTime = DateTime.Now,
                DriverId = model.DriverId,
                Driver = db.Drivers.Find(model.DriverId),
                Status = "In Transit",
                QRCodeImagePathConfirmation = GenerateQRCode(qr_code_text.ToString()),
                QRCodeTextConfirmation = qr_code_text.ToString()
            });

            db.Notifications.PushNotificaiton(string.Format("You changed Customer Order #{0} status", order.Id));

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private string GenerateQRCode(string qrcodeText)
        {
            string folderPath = "~/Images/";
            string imagePath = "~/Images/QrCode.jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            return imagePath;
        }

    }
}