using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class DiscountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Pharmacist/Discount
        public async Task<ActionResult> Index()
        {
            var model = new DiscountVM
            {
                AllProductList = await db.Products.ToListAsync(),
                DiscountedProducts = await db.Products.Where(p => p.IsOnSale == true)
                    .ToListAsync(),
                Categories = new SelectList(await db.Categories.ToListAsync(), "Id", "Name")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<string> ApplyDiscount(float discountValue, int productSelected, string optionValue)
        {
            var product = await db.Products.FindAsync(productSelected);
            float discountByPercent;

            if (discountValue < 0 || discountValue > product.SellingPrice)
            {
                TempData["Error"] = "Invalid Discount Price Amount";
                return "InvalidPrice";
            }

            if (optionValue == "Percentage")
            {
                discountByPercent = product.SellingPrice * (discountValue / 100);
                discountByPercent = (float)Math.Round(discountByPercent, 2);
                product.DiscountedPrice = product.SellingPrice - discountByPercent;
                product.IsOnSale = true;
                product.DiscountedRate = (int)discountValue;
            }
            else
            {
                product.DiscountedPrice = discountValue;
                product.IsOnSale = true;
            }

            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return "success";
        }
        [HttpPost]
        public async Task<int> RemoveDiscountProduct(int discountToDelete)
        {
            var product = await db.Products.FindAsync(discountToDelete);
            int id = product.Id;

            product.IsOnSale = false;
            product.DiscountedPrice = 0;
            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return id;
        }
    }
}