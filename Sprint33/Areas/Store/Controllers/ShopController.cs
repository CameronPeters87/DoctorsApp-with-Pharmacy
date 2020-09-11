using PagedList;
using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Areas.Store.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Store.Controllers
{
    public class ShopController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Store/Shop
        public async Task<ActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var model = new MainVM
            {
                ProductContents = await (from p in db.Products
                                         select new ProductContentVM
                                         {
                                             Id = p.Id,
                                             ImageUrl = p.ImageUrl,
                                             Name = p.Name,
                                             Price = p.SellingPrice,
                                             IsOnSale = p.IsOnSale,
                                             DiscountedPrice = p.DiscountedPrice,
                                             ProductLink = "/store/shop/product-details/" + p.Slug
                                         }).ToListAsync()
            };

            var onePageOfProducts = model.ProductContents.ToPagedList(pageNumber, 5);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View("Main", model);
        }

        public JsonResult LiveSearch(string search)
        {

            List<ProductContentVM> products = new List<ProductContentVM>();

            products = (from p in db.Products
                        select new ProductContentVM
                        {
                            Id = p.Id,
                            ImageUrl = p.ImageUrl,
                            Name = p.Name,
                            Price = p.SellingPrice,
                            IsOnSale = p.IsOnSale,
                            DiscountedPrice = p.DiscountedPrice,
                            ProductLink = "/store/shop/product-details/" + p.Slug
                        }).ToList();

            var model = products.Where(p => p.Name.Contains(search)).ToList();

            return Json(model);
        }

        [ActionName("product-details")]
        public ActionResult ProductDetails(string slug)
        {
            var product = db.Products.FindBySlug(slug);

            product.SellingPrice = (float)Math.Round(product.SellingPrice, 2);
            product.DiscountedPrice = (float)Math.Round(product.DiscountedPrice, 2);

            var model = new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                BarcodeUrl = product.BarcodeUrl,
                Category = product.Category,
                Description = product.Description,
                DiscountedPrice = product.DiscountedPrice,
                ImageUrl = product.ImageUrl,
                IsOnSale = product.IsOnSale,
                PackSize = product.PackSize,
                Quantity = product.Quantity,
                SellingPrice = product.SellingPrice
            };

            return View("ProductDetails", model);
        }

        public ActionResult CartItemsPartial()
        {
            var patientId = Convert.ToInt32(Session["id"]);
            var patient = db.Patients.Find(patientId);

            var currentCart = db.CustomerCarts.GetCurrentCartItems(patientId);

            var model = new CartItemsVM
            {
                LinkToSummary = "/store/cart/summary/" + patientId.ToString(),
                NumberOfItemsInCart = currentCart.Count()
            };

            return PartialView("_CartItemsPartial", model);
        }

    }
}