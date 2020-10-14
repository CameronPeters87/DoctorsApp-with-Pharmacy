using Geocoding;
using Geocoding.Google;
using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

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

            string pharmacyAddress = "108 Allenby Road Durban South Africa 4051";
            //string pharmacyAddress = "108 Allenby Road Durban North KZN South Africa";
            var _latitude = LocationExtensions.GetLatFromPlaceName(pharmacyAddress, geocoder);
            var _longitude = LocationExtensions.GetLongFromPlaceName(pharmacyAddress, geocoder);
            double destination_Lat;
            double destination_Long;

            var model = await (from o in db.CustomerOrders
                               where o.OrderStatus.ProcessNumber == 4
                               orderby o.Id descending
                               select new DeliveriesModel
                               {
                                   CustomerOrder = o
                               }).ToListAsync();

            foreach (var item in model)
            {
                destination_Lat = LocationExtensions.GetLatFromPlaceName(item.CustomerOrder.Address +
                    item.CustomerOrder.City +
                    item.CustomerOrder.Country +
                    item.CustomerOrder.ZipCode,
                    geocoder);
                destination_Long = LocationExtensions.GetLongFromPlaceName(item.CustomerOrder.Address +
                    item.CustomerOrder.City +
                    item.CustomerOrder.Country +
                    item.CustomerOrder.ZipCode,
                    geocoder);
                item.Distance = LocationExtensions.GetDistance(_latitude, _longitude,
                    destination_Lat, destination_Long, 'K');

            }

            return View(model);
        }

        public ActionResult Directions()
        {
            return View();
        }
    }
}