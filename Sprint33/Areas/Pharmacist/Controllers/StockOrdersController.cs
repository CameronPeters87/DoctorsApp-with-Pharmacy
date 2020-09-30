using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class StockOrdersController : Controller
    {
        // GET: Pharmacist/StockOrders
        private ApplicationDbContext db = new ApplicationDbContext();
        private float TAX_const = 0.15f;

        // GET: Pharmacist/StockOrder
        public async Task<ActionResult> Index()
        {
            StockOrderVM model = new StockOrderVM();
            model.StockOrderListVM = await (from o in db.StockOrders
                                            join s in db.Suppliers on o.SupplierId equals s.Id
                                            orderby o.Id descending
                                            select new StockOrderListVM
                                            {
                                                StockOrderId = o.Id,
                                                Supplier = s.Name,
                                                OrderDate = o.StockOrderDate,
                                                PaymentPeriod = o.PaymentPeriod,
                                                TotalCost = o.TotalCost,
                                                CartItems = db.StockCarts.Where(c => c.StockOrderId == o.Id).ToList(),
                                                InvoiceLink = "StockOrders/InvoiceToPdf/" + o.Id,
                                                OrderStatus = o.OrderStatus,
                                                OrderStatusId = o.OrderStatusId
                                            }).ToListAsync();
            model.Suppliers = await db.Suppliers.ToListAsync();
            model.OrderStatuses = await db.OrderStatuses.ToListAsync();
            foreach (var item in model.StockOrderListVM)
            {
                item.LongOrderDate = item.OrderDate.ToLongDateString();
            }

            return View(model);
        }

        public async Task<ActionResult> NewPurchase(int supplierId)
        {
            Supplier supplier = await db.Suppliers.FindAsync(supplierId);
            var model = new NewPurchaseVM();

            model.ProductsDropdown = new SelectList(db.Products
                .Where(m => m.SupplierId.Equals(supplierId)).ToList(), "Id", "Name");

            model.ProductList = await db.Products
                .Where(m => m.SupplierId.Equals(supplierId)).ToListAsync();

            model.SupplierId = supplierId;
            model.Supplier = supplier.Name;

            model.StockCart = await db.StockCarts.Where(c => c.StockOrderId.Equals(null) &&
                c.SupplierId.Equals(supplierId)).ToListAsync();

            model.TaxTotal = model.StockCart.Select(c => c.VatAmount).Sum();
            model.TotalPrice = model.StockCart.Select(c => c.Price).Sum();
            model.SubTotal = model.TotalPrice - model.TaxTotal;


            ViewBag.SupplierId = supplierId;

            return View(model);
        }

        // Post: /Orders/AddToCart
        [HttpPost]
        public ActionResult AddToCart(NewPurchaseVM model)
        {
            if (ModelState.IsValid)
            {
                Product product = db.Products.Find(model.ProductId);
                float vatTotal = product.SupplierPrice * model.Quantity * TAX_const;

                // if product is already in list in list, display error 
                if (db.StockCarts.Any(c => c.ProductId.Equals(model.ProductId) &&
                    c.StockOrderId.Equals(null)))
                {
                    TempData["Error"] = "That product is already in cart";
                    return Redirect("/Pharmacist/StockOrders/NewPurchase?supplierId=" + model.SupplierId);
                }

                if (model.Quantity <= 0)
                {
                    TempData["Error"] = "That is an invalid quantity amount";
                    return Redirect("/Pharmacist/StockOrders/NewPurchase?supplierId=" + model.SupplierId);
                }

                db.StockCarts.Add(new StockCart
                {
                    ProductId = model.ProductId,
                    SupplierId = model.SupplierId,
                    VatAmount = vatTotal,
                    Price = product.SupplierPrice * model.Quantity,
                    Quantity = model.Quantity,
                    Product = product
                });

                db.SaveChanges();

                return Redirect("/Pharmacist/StockOrders/NewPurchase?supplierId=" + model.SupplierId);
            }
            TempData["Error"] = "You did not select a product";
            return Redirect("/Pharmacist/StockOrders/NewPurchase?supplierId=" + model.SupplierId);

        }

        // Get: Orders/RemoveFromCart/CartId
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cartItem = db.StockCarts.Find(id);

            int supplierId = cartItem.SupplierId;


            // Get the name of the product to display confirmation

            // Remove from cart
            db.StockCarts.Remove(cartItem);
            db.SaveChanges();

            return RedirectToAction("NewPurchase",
                new
                {
                    supplierId = supplierId,
                    area = "Pharmacist"
                });
        }

        // Ajax Post: StockOrders/EditQuantity
        [HttpPost]
        public async Task<string> EditQuantity(int newQty, int id)
        {
            var stockCartItem = await db.StockCarts.FindAsync(id);
            var product = await db.Products.Where(p => p.Id == stockCartItem.ProductId)
                .FirstOrDefaultAsync();
            float vatTotal = product.SupplierPrice * newQty * TAX_const;

            if (newQty < 0)
            {
                TempData["Error"] = "Invalid Quantity amount";
                return "InvalidQuantity";
            }

            if (stockCartItem != null)
            {
                stockCartItem.Quantity = newQty;
                stockCartItem.VatAmount = vatTotal;
                stockCartItem.Price = product.SupplierPrice * newQty;
                db.Entry(stockCartItem).State = EntityState.Modified;
            }

            await db.SaveChangesAsync();

            return "success";
        }

        // Post: /Orders/CreateOrder
        [HttpPost]
        public async Task<ActionResult> CreateOrder(NewPurchaseVM model)
        {
            if (ModelState.IsValid)
            {
                Supplier supplier = db.Suppliers.Find(model.SupplierId);

                // Change order status
                OrderStatus defaultOrderStatus = await db.OrderStatuses.Where(os => os.Name.Equals("Pending Payment"))
                                                        .FirstOrDefaultAsync();

                // Cant buy stock if theres no items in the cart
                if (!db.StockCarts.Any(c => c.SupplierId == model.SupplierId))
                {
                    TempData["Error"] = "There is no items in the cart to place order.";
                    return Redirect("/Pharmacist/StockOrders/NewPurchase?supplierId=" + model.SupplierId);
                }

                db.StockOrders.Add(new StockOrder
                {
                    SupplierId = model.SupplierId,
                    PaymentPeriod = model.PaymentPeriod,
                    StockOrderDate = DateTime.Now,
                    TotalCost = model.TotalPrice,
                    Notes = model.Notes,
                    SubTotal = model.SubTotal,
                    TotalTax = model.TaxTotal,
                    Supplier = supplier,
                    OrderStatusId = defaultOrderStatus.Id,
                    OrderStatus = defaultOrderStatus
                });

                await db.SaveChangesAsync();

                // Assign orderid to stockcart items
                var stockCart = await db.StockCarts.Where(c => c.StockOrderId.Equals(null)).ToListAsync();
                var latestOrder = await db.StockOrders.OrderByDescending(o => o.Id).FirstAsync();
                stockCart.ForEach(c => c.StockOrderId = latestOrder.Id); // Order Id Should Equal to the latest order

                // Add notification
                db.Notifications.Add(new Notification
                {
                    CreatedDate = DateTime.Now,
                    Message = "You have placed an order of R" + latestOrder.TotalCost,
                    isRead = false,
                    Icon = "fa-file-alt",
                    BackgroundColorIcon = "bg-info"
                });

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<ActionResult> ChangeOrderStatus(int stockOrderId, int orderStatusId)
        {
            var stockOrder = await db.StockOrders.FindAsync(stockOrderId);
            var newOrderStatus = await db.OrderStatuses.FindAsync(orderStatusId);
            var orderedItems = await db.StockCarts.Where(c => c.StockOrderId == stockOrder.Id)
                .ToListAsync();


            stockOrder.OrderStatusId = orderStatusId;
            stockOrder.OrderStatus = newOrderStatus;

            // Display notification and add the quantity of the
            // products bought to the stock on hand
            if (newOrderStatus.Name == "Completed")
            {
                foreach (var item in orderedItems)
                {
                    var product = await db.Products.FindAsync(item.ProductId);
                    product.Quantity += item.Quantity;
                    db.Entry(product).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                db.Notifications.Add(new Notification
                {
                    CreatedDate = DateTime.Now,
                    Message = "You changed Order Status of Order #" + stockOrderId +
                    " to " + newOrderStatus.Name + ". Your stock has been updated accordingly.",
                    isRead = false,
                    Icon = "fa-check",
                    BackgroundColorIcon = "bg-success"
                });
            }

            else if (newOrderStatus.Name == "On Hold")
            {
                db.Notifications.Add(new Notification
                {
                    CreatedDate = DateTime.Now,
                    Message = "You put Order #" + stockOrderId +
                    " " + newOrderStatus.Name,
                    isRead = false,
                    Icon = "fa-exclamation-triangle",
                    BackgroundColorIcon = "bg-warning"
                });
            }
            else
            {
                db.Notifications.Add(new Notification
                {
                    CreatedDate = DateTime.Now,
                    Message = "You changed Order Status of Order #" + stockOrderId +
                    " to " + newOrderStatus.Name,
                    isRead = false,
                    Icon = "fa-file-alt",
                    BackgroundColorIcon = "bg-primary"
                });

            }

            db.Entry(stockOrder).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Invoice(int id)
        {
            var model = await (from o in db.StockOrders
                               where o.Id == id
                               select new InvoiceVm
                               {
                                   StockOrderId = o.Id,
                                   StockOrderDate = o.StockOrderDate,
                                   IsPaid = o.OrderStatus.isPaid,
                                   PaymentPeriod = o.PaymentPeriod,
                                   SubTotal = o.SubTotal,
                                   TotalTax = o.TotalTax,
                                   TotalCost = o.TotalCost,
                                   Supplier = o.Supplier,
                               }).FirstOrDefaultAsync();

            model.StringOrderDate = model.StockOrderDate.ToLongDateString();

            switch (model.PaymentPeriod)
            {
                case "30":
                    model.PaymentDue = model.StockOrderDate.AddMonths(1);
                    break;
                case "60":
                    model.PaymentDue = model.StockOrderDate.AddMonths(2);
                    break;
                case "90":
                    model.PaymentDue = model.StockOrderDate.AddMonths(3);
                    break;
                default:
                    model.PaymentDue = model.StockOrderDate;
                    break;
            }

            model.StringPaymentDue = model.PaymentDue.ToLongDateString();

            model.StockOrderItems = db.StockCarts.Where(s => s.StockOrderId == id).ToList();
            return View(model);
        }

        public ActionResult InvoiceToPdf(int id)
        {
            var result = new Rotativa.ActionAsPdf("Invoice", new { id = id });
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Reset(NewPurchaseVM model)
        {
            if (!ModelState.IsValid)
            {
                model.ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name");
                return RedirectToAction("/Pharmacist/NewPurchase?supplierid=" + model.SupplierId);
            }

            var itemsToRemove = await db.StockCarts.Where(c => c.StockOrderId == null).ToListAsync();
            db.StockCarts.RemoveRange(itemsToRemove);
            await db.SaveChangesAsync();

            return RedirectToAction("NewPurchase",
                new
                {
                    model.SupplierId
                });
        }
    }
}