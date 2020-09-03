using Sprint33.Areas.Pharmacist.Models;
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
                                           FirstName = p.FirstName,
                                           Surname = p.Surname,
                                           ContactNumber = p.ContactNumber,
                                           Email = p.Email,
                                           IsLoyal = p.isLoyal
                                       }).ToListAsync();
            return View(customersList);
        }
    }
}