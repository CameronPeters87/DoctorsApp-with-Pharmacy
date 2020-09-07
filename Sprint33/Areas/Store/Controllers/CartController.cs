using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Areas.Store.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Store/Cart
        public ActionResult Summary(int id)
        {
            var currentCart = db.CustomerCarts.GetCurrentCartItems(id);

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

            return View("Index", model);
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

        public async Task<ActionResult> CreateOrder()
        {
            var patientId = Convert.ToInt32(Session["id"]);
            var patient = db.Patients.Find(patientId);

            var currentCart = db.CustomerCarts.GetCurrentCartItems(patientId);
            var defaultStatus = db.OrderStatuses.Where(s => s.Name == "Pending Payment").FirstOrDefault();

            var totalCost = currentCart.GetTotalPrice();
            var tax = currentCart.GetTotalTax();

            await db.Billings.CreateBilling(db, patient);

            var lastBilling = db.Billings.GetLastBilling();

            db.CustomerOrders.Add(new CustomerOrder
            {
                Customer = patient,
                CustomerId = patientId,
                OrderStatus = defaultStatus,
                OrderStatusId = defaultStatus.Id,
                OrderDate = DateTime.Today,
                TotalCost = totalCost,
                TotalTax = tax,
                SubTotal = totalCost - tax,
                Billing = lastBilling,
                BillingId = lastBilling.Id
            });

            await db.SaveChangesAsync();

            var lastOrder = db.CustomerOrders.GetLastOrder();

            foreach (var item in currentCart)
            {
                item.CustomerOrderId = lastOrder.Id;
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return Redirect(string.Format("/store/checkout/index/{0}/{1}",
                lastOrder.Id, lastOrder.CustomerId));
        }
    }
}