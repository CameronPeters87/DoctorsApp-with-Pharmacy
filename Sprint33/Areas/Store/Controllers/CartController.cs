using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Areas.Store.Models;
using Sprint33.Models;
using Sprint33.Services;
using Sprint33.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ICustomerOrderRepository customerOrderRepository = new CustomerOrderRepository();

        // GET: Store/Cart
        public ActionResult Summary()
        {
            var cart = new ShoppingCart(this.HttpContext, db);

            var model = new CartSummaryVM();

            // Product list
            foreach (var item in cart.GetCartItems())
            {
                model.CartItems.Add(new CartItemsSummary
                {
                    Name = item.Product.Name,
                    Id = item.Id,
                    Price = item.Product.IsOnSale ? item.Product.DiscountedPrice : item.Product.SellingPrice,
                    ImageUrl = item.Product.ImageUrl,
                    Quantity = item.Quantity
                });
            }

            model.TotalCost = cart.GetTotalPrice();
            model.VatTotal = cart.GetTotalTax();

            return View("Index", model);
        }

        [HttpPost]
        public async Task<string> AddItemToCart(int id, int qty)
        {
            var product = await db.Products.FindAsync(id);

            var cart = new ShoppingCart(this.HttpContext, db);

            var isInCart = product.IsItemInCustomerCart(cart.GetCartItems());

            if (isInCart)
            {
                return "Item in Cart";
            }

            cart.AddToCart(product, qty);

            return "Success";
        }

        public ActionResult RemoveItem(int id)
        {
            var cart = new ShoppingCart(HttpContext, db);

            cart.RemoveCartItem(id);

            return RedirectToAction("Summary");
        }

        public ActionResult CreateOrder()
        {
            var patientId = Convert.ToInt32(Session["id"]);

            customerOrderRepository.InitializeOrder(patientId, this);

            return Redirect(customerOrderRepository
                .GetCheckoutUrl(customerOrderRepository
                    .GetOrderId(this.HttpContext), patientId));
        }
    }
}