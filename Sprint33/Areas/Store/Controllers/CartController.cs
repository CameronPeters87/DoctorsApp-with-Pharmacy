using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Areas.Store.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Store/Cart
        public ActionResult Index()
        {
            var patientId = Convert.ToInt32(Session["id"]);

            var currentCart = db.CustomerCarts.GetCurrentCartItems(patientId);

            var model = new CartSummaryVM();

            // Product list
            foreach (var item in currentCart)
            {
                model.ProductList.Add(new CartItemsSummary
                {
                    Name = item.Product.Name,
                    Id = item.Product.Id,
                    Price = item.Product.IsOnSale ? item.Product.DiscountedPrice : item.Product.SellingPrice,
                    ImageUrl = item.Product.ImageUrl,
                    Quantity = item.Quantity
                });
            }

            model.TotalCost = currentCart.GetTotalPrice();
            model.VatTotal = currentCart.GetTotalTax();
            model.SubTotal = model.TotalCost - model.VatTotal;

            return View(model);
        }

        [HttpPost]
        public async Task<string> AddItemToCart(int id, int qty)
        {
            var product = await db.Products.FindAsync(id);

            var patientId = Convert.ToInt32(Session["id"]);
            var patient = await db.Patients.FindAsync(patientId);

            // Check if item in cart
            var currentCart = db.CustomerCarts.GetCurrentCartItems(patientId);
            var isInCart = product.IsItemInCustomerCart(currentCart);
            if (isInCart)
            {
                return "Item in Cart";
            }

            float vat;

            if (product.IsOnSale)
            {
                vat = Calculation.CalculateVatOnProduct(product.DiscountedPrice, qty, 0.15f);
            }
            else
                vat = Calculation.CalculateVatOnProduct(product.SellingPrice, qty, 0.15f);

            db.CustomerCarts.Add(new CustomerCart
            {
                CustomerId = patientId,
                CustomerOrderId = null,
                Patient = patient,
                Price = product.IsOnSale ? product.DiscountedPrice : product.SellingPrice,
                Quantity = qty,
                Product = product,
                ProductId = product.Id,
                VatAmount = vat
            });

            await db.SaveChangesAsync();

            return "Success";
        }

        [HttpPost]
        public ActionResult CreateOrder()
        {
            return View();
        }

        public ActionResult Checkout()
        {

            return View();
        }
    }
}