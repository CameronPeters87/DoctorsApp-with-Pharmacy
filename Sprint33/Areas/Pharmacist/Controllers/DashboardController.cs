using Sprint33.Models;
using System.Linq;
using System.Web.Mvc;


namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class DashboardController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Pharmacist/Dashboard
        public ActionResult Index()
        {
            ViewBag.TotalPurchase = db.StockOrders.ToList().Count;
            ViewBag.TotalOnlineSales = db.CustomerOrders.ToList().Count;
            ViewBag.TotalInstoreSales = db.InStoreSales.ToList().Count;
            ViewBag.TotalCustomers = db.Patients.ToList().Count;
            ViewBag.TotalSuppliers = db.Suppliers.ToList().Count;
            ViewBag.Products = db.Products.ToList().Count;

            ViewBag.TotalExpense = db.StockOrders.Sum(s => (float?)s.TotalCost) ?? 0f;
            ViewBag.TotalIncome = SumTotalInStoreSales(db) + SumTotalOnlineSales(db);
            ViewBag.ProfitLoss = ViewBag.TotalIncome - ViewBag.TotalExpense;

            ViewBag.LowStockMedicines = db.Products.Where(p => p.Quantity < 50)
                .ToList();

            return View();
        }

        [HttpPost]
        public ActionResult GenerateOverview(int Period)
        {
            return RedirectToAction("GeneratedSalesOverview", new { period = Period });
        }

        public ActionResult GeneratedSalesOverview(int period)
        {
            /*
             * Total Number of Customers
             * Number of Customers Registered during the period
             * Number of Sales during the period
             * Avg Order Per Customer
             * Revenue Earned
             */

            return View();
        }

        private float? SumTotalOnlineSales(ApplicationDbContext db)
        {
            return db.CustomerOrders.Sum(c => (float?)c.TotalCost) ?? 0f;
        }
        private float? SumTotalInStoreSales(ApplicationDbContext db)
        {
            return db.InStoreSales.Sum(c => (float?)c.TotalCost) ?? 0f;
        }
    }
}