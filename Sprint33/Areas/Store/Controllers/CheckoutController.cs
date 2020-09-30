using Sprint33.Areas.Store.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using Sprint33.Services;
using Sprint33.Services.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class CheckoutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ICustomerOrderRepository orderRepository = new CustomerOrderRepository();

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

            Coupon coupon = new Coupon();

            if (!model.CustomerOrder.IsCouponNull())
            {
                model.CouponId = model.CustomerOrder.CouponId;
                coupon = db.Coupons.Find(model.CouponId);
                model.CouponCode = coupon.Code;
                model.CouponDiscount = coupon.DiscountRate;
            }

            return View(model);
        }

        // Post: Store/Checkout/ApplyCoupon
        [HttpPost]
        public async Task<string> ApplyCoupon(string code, int orderId)
        {
            if (code == string.Empty)
            {
                return "CodeNull";
            }
            var order = await db.CustomerOrders.FindAsync(orderId);
            var valid = db.Coupons.IsCouponCodeEnteredValid(code, order.TotalCost);

            if (order.CouponId != null)
            {
                return "CouponInUse";
            }

            var coupon = new Coupon();

            if (valid)
            {
                coupon = db.Coupons.GetCouponByCode(code);

                var customerOrders = db.CustomerOrders.GetCustomerOrders(Convert.ToInt32(Session["id"]));

                if (customerOrders.Any(o => o.CouponId == coupon.Id))
                {
                    return "CouponAlreadyUsed";
                }
                else
                {
                    orderRepository.ApplyCoupon(coupon, order);

                    return "Success";
                }
            }

            return "Failed";
        }

        [HttpPost]
        public string UpdateBillingInfo(BillingForm model)
        {
            if (!ModelState.IsValid)
            {
                return "Failed";
            }

            orderRepository.UpdateBillingInfo(model);

            return "Success";
        }


    }
}