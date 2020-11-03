using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Pharmacist/Customers
        public async Task<ActionResult> Index()
        {
            var customersList = await (from p in db.Patients
                                       select new CustomerVM
                                       {
                                           Id = p.UserID,
                                           FirstName = p.FirstName,
                                           Surname = p.Surname,
                                           ContactNumber = p.ContactNumber,
                                           Email = p.Email,
                                           IsLoyal = p.isLoyal
                                       }).ToListAsync();

            foreach (var item in customersList)
            {
                item.NumberOfPurchases = db.CustomerOrders.Where(o => o.CustomerId == item.Id).ToList().Count;
            }

            return View(customersList);
        }

        public ActionResult SendEmail(int id)
        {
            var customer = db.Patients.Find(id);

            var model = new CustomerEmailModel
            {
                Patient = customer,
                Email = customer.Email,
                //Subject = "test"
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult SendEmail(CustomerEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = db.Patients.Where(o => o.Email == model.Email).FirstOrDefault();

            //EmailExtensions.SendMail(model.Email, model.Subject, model.Body);

            EmailExtensions.SendSms(customer.ContactNumber, model.Body);

            return RedirectToAction("Index");
        }
    }
}