using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using InStoreCart = Sprint33.PharmacyEntities.InStoreCart;


namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class InStoreSalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private float TAX_const = 0.15f;

        // GET: Sales
        public async Task<ActionResult> SalesReport()
        {
            var model = new InStoreSalesReportVM
            {
                InStoreSales = await (from s in db.InStoreSales
                                      orderby s.Id descending
                                      select new InStoreSaleVM
                                      {
                                          InStoreSaleId = s.Id,
                                          SaleDate = s.SaleDate,
                                          TotalCost = s.TotalCost,
                                          CartItems = db.InStoreCarts.Where(c => c.InStoreSaleId == s.Id).ToList(),
                                          ReceiptLink = "/Pharmacist/InStoreSales/ReceiptToPdf/" + s.Id
                                      }).ToListAsync()
            };

            foreach (var item in model.InStoreSales)
            {
                item.LongSaleDate = item.SaleDate.ToLongDateString();
            }
            return View(model);
        }

        public async Task<ActionResult> NewInStoreSale()
        {
            NewInStoreSaleVM newInStoreSaleVM = new NewInStoreSaleVM();
            var model = newInStoreSaleVM;
            var lastInStoreSale = db.InStoreSales.GetLastInStoreSale();

            model.ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name");
            model.ProductList = await db.Products.ToListAsync();

            model.CurrentCartItems = db.InStoreCarts.GetCurrentCartItems();

            model.TotalPrice = model.CurrentCartItems.GetTotalPriceOfCurrentCart();
            model.TaxTotal = model.CurrentCartItems.GetTaxOfCurrentCart();

            var exists = model.CheckCashTenderedExists();

            if (exists)
            {
                model.Change = lastInStoreSale.Change;
            }
            if (lastInStoreSale != null)
                ViewBag.ReceiptLink = "InStoreSales/ReceiptToPdf/" + lastInStoreSale.Id;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToInStoreCart(NewInStoreSaleVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "You did not select a product";
                model.ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name");
                return RedirectToAction("NewInStoreSale");
            }
            Product product = db.Products.Find(model.ProductId);
            var currentCart = db.InStoreCarts.GetCurrentCartItems();

            // Quantity has to be greater than 0
            if (model.Quantity <= 0)
            {
                TempData["Error"] = "That is an invalid quantity amount";
                return RedirectToAction("NewInStoreSale");
            }
            // Display error if there is no stock
            if (product.IsOutOfStock(model.Quantity))
            {
                TempData["Error"] = "You don't have enough stock: " + product.Name +
                    " (" + product.PackSize + ")";
                return RedirectToAction("NewInStoreSale");
            }

            if(product.IsItemInCart(currentCart))
            {
                TempData["Error"] = "That product is already in cart";
                return RedirectToAction("NewInStoreSale");
            }

            float price = product.IsOnSale ? product.DiscountedPrice * model.Quantity :
                product.SellingPrice * model.Quantity;

            float vatTotal = Calculation.CalculateVatOnProduct(price,
                model.Quantity, TAX_const);


            db.InStoreCarts.Add(new InStoreCart
            {
                ProductId = model.ProductId,
                VatAmount = vatTotal,
                Price = price,
                Quantity = model.Quantity,
                Product = product
            });

            // Decrease product quantity
            product.Quantity -= model.Quantity;

            db.Entry(product).State = EntityState.Modified;

            await db.SaveChangesAsync();

            return RedirectToAction("NewInStoreSale");
        }

        // Ajax Post: StockOrders/EditQuantity
        [HttpPost]
        public async Task<string> EditQuantity(int newQty, int id)
        {
            var inStoreCartItem = await db.InStoreCarts.FindAsync(id);
            var product = await db.Products.Where(p => p.Id == inStoreCartItem.ProductId)
                .FirstOrDefaultAsync();
            float price = product.IsOnSale ? product.DiscountedPrice * newQty :
                product.SellingPrice * newQty;
            float vatTotal = Calculation.CalculateVatOnProduct(product.SellingPrice,
                newQty, TAX_const);

            if (newQty < 0)
            {
                TempData["Error"] = "Invalid Quantity amount";
                return "InvalidQuantity";
            }

            if (inStoreCartItem != null)
            {
                inStoreCartItem.Quantity = newQty;
                inStoreCartItem.VatAmount = vatTotal;
                inStoreCartItem.Price = price;
                db.Entry(inStoreCartItem).State = EntityState.Modified;
            }

            await db.SaveChangesAsync();

            return "success";
        }
        public async Task<ActionResult> RemoveItemFromCart(int id)
        {
            var inStoreCartItem = await db.InStoreCarts.FindAsync(id);
            db.InStoreCarts.Remove(inStoreCartItem);
            await db.SaveChangesAsync();

            return RedirectToAction("NewInStoreSale");
        }

        [HttpPost]
        public async Task<ActionResult> CompleteInStoreSale(NewInStoreSaleVM model)
        {
            if (!ModelState.IsValid)
            {
                model.ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name");
                return RedirectToAction("NewInStoreSale");
            }

            float change = 0;

            if (model.PaymentMethod == null)
            {
                TempData["Error"] = "Please select a payment method";
                return RedirectToAction("NewInStoreSale");
            }
            else if (model.PaymentMethod == "Cash")
            {
                change = model.CashTendered - model.TotalPrice;
            }
            else { }

            if (change < 0)
            {
                TempData["Error"] = "Cash Tendered can't be less than total price";
                return RedirectToAction("NewInStoreSale");
            }

            db.InStoreSales.Add(new InStoreSale
            {
                SaleDate = DateTime.Now,
                PaymentMethod = model.PaymentMethod,
                Change = change,
                CashTendered = model.PaymentMethod == "Cash" ? model.CashTendered : model.TotalPrice,
                TotalCost = model.TotalPrice,
                TotalTax = model.TaxTotal,
            });

            await db.SaveChangesAsync();

            // Assign InStoreSaleId to inStoreCart items
            var inStoreCart = await db.InStoreCarts.Where(c => c.InStoreSaleId.Equals(null)).ToListAsync();
            var lastInStoreSale = await db.InStoreSales.OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();
            inStoreCart.ForEach(c => c.InStoreSaleId = lastInStoreSale.Id);

            await db.SaveChangesAsync();

            TempData["SM"] = "You completed a POS!";

            return lastInStoreSale != null ? RedirectToAction("ReceiptToPdf", new { id = lastInStoreSale.Id }) :
                RedirectToAction("NewInStoreSale");
        }

        [HttpPost]
        public async Task<ActionResult> Reset(NewInStoreSaleVM model)
        {
            if (!ModelState.IsValid)
            {
                model.ProductsDropdown = new SelectList(db.Products.ToList(), "Id", "Name");
                return RedirectToAction("NewInStoreSale");
            }

            var itemsInCart = await db.InStoreCarts
                .Where(c => c.InStoreSaleId == null)
                .OrderByDescending(c => c.ProductId)
                .ToListAsync();

            // Revert the product quantity back
            var actualProducts = await (from p in db.Products
                                        join c in db.InStoreCarts on p.Id equals c.ProductId
                                        where p.Id == c.ProductId && c.InStoreSaleId == null
                                        orderby p.Id descending
                                        select p).ToListAsync();
            var product = new Product();

            foreach (var item in itemsInCart)
            {
                product = actualProducts.Where(p => p.Id == item.ProductId).FirstOrDefault();
                product.Quantity += item.Quantity;

                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            db.InStoreCarts.RemoveRange(itemsInCart);
            await db.SaveChangesAsync();

            return RedirectToAction("NewInStoreSale");
        }

        public async Task<ActionResult> Receipt(int id)
        {
            var model = await (from s in db.InStoreSales
                               where s.Id == id
                               select new ReceiptVm()
                               {
                                   InStoreSaleDateTime = s.SaleDate,
                                   CashTendered = s.CashTendered,
                                   Change = s.Change,
                                   InStoreSaleId = s.Id,
                                   PaymentMethod = s.PaymentMethod,
                                   TotalCost = s.TotalCost,
                                   TotalTax = s.TotalTax
                               }).FirstOrDefaultAsync();

            model.StringSaleDateLong = model.InStoreSaleDateTime.ToLongDateString();

            model.InStoreSaleItems = await db.InStoreCarts.Where(s => s.InStoreSaleId == id).ToListAsync();

            return View(model);
        }

        public ActionResult ReceiptToPdf(int id)
        {
            var result = new Rotativa.ActionAsPdf("Receipt", new { id = id });
            return result;
        }
    }
}