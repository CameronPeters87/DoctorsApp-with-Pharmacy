using Sprint33.Areas.Store.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class CheckoutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Store/Checkout
        public async Task<ActionResult> Index(int orderId, int customerId)
        {
            var model = new Checkout();

            model.CustomerOrder = await db.CustomerOrders.Where(o => o.Id == orderId &&
            o.CustomerId == customerId).FirstOrDefaultAsync();

            var currentCart = db.CustomerCarts.GetCartItemsFromOrder(customerId, orderId);

            // Product list
            foreach (var item in currentCart)
            {
                model.CartItems.Add(new CartItemsSummary
                {
                    Name = item.Product.Name,
                    Id = item.Product.Id,
                    Price = item.Product.IsOnSale ? item.Product.DiscountedPrice : item.Product.SellingPrice,
                    ImageUrl = item.Product.ImageUrl,
                    Quantity = item.Quantity
                });
            }

            return View(model);
        }
    }
}