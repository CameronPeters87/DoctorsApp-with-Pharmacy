using IronBarCode;
using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Areas.Store.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.Services;
using Sprint33.Services.Interfaces;
using System;
using System.Drawing;
using System.IO;
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

            model.VatTotal = cart.GetTotalTax();
            model.SubTotal = cart.GetTotalPrice();
            model.TotalCost = model.SubTotal = model.SubTotal;

            return View("Index", model);
        }

        [HttpPost]
        public async Task<string> AddItemToCart(int id, int qty)
        {
            var product = await db.Products.FindAsync(id);

            var cart = new ShoppingCart(this.HttpContext, db);

            var isInCart = product.IsItemInCustomerCart(cart.GetCartItems());

            if (product.IsOutOfStock(qty))
            {
                return "No Stock";
            }

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

            var lastOrder = db.CustomerOrders.GetLastOrder();

            #region Generating QR
            string _path = Path.Combine(Server.MapPath("~/Files/ConfirmationQRs"), lastOrder.Id + ".png");
            string _pathDelivery = Path.Combine(Server.MapPath("~/Files/DelivererConfirmationQR"), lastOrder.Id + "_delivery.png");
            try
            {
                //using IronBarCode;
                //using System.Drawing;
                // Styling a QR code and adding annotation text
                var MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode(lastOrder.Id.ToString(), BarcodeWriterEncoding.QRCode);
                MyBarCode.AddBarcodeValueTextBelowBarcode();
                MyBarCode.SetMargins(100);
                MyBarCode.ChangeBarCodeColor(Color.Purple);
                // Save as HTML
                MyBarCode.SaveAsPng(_path);

                var MyDeliveryBarCode = IronBarCode.BarcodeWriter.CreateBarcode("delivery" + lastOrder.Id.ToString(), BarcodeWriterEncoding.QRCode);
                MyDeliveryBarCode.AddBarcodeValueTextBelowBarcode();
                MyDeliveryBarCode.SetMargins(50);
                MyDeliveryBarCode.ChangeBarCodeColor(Color.Green);
                // Save as HTML
                MyDeliveryBarCode.SaveAsPng(_pathDelivery);
            }
            catch (Exception e)
            {
                throw;
            }

            lastOrder.ConfirmationQR_Url = "/Files/ConfirmationQRs/" + lastOrder.Id.ToString() + ".png";
            lastOrder.DelivererConfirmationQR_Url = "/Files/DelivererConfirmationQR/" + lastOrder.Id.ToString() + "_delivery.png";
            db.Entry(lastOrder).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            #endregion

            return Redirect(customerOrderRepository
                .GetCheckoutUrl(customerOrderRepository
                    .GetOrderId(this.HttpContext), patientId));
        }

        [HttpPost]
        public async Task<string> UpdateQuantity(int newQty, int id)
        {
            return "success";
        }
    }
}