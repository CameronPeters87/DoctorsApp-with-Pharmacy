using PagedList;
using Sprint33.Areas.Pharmacist.HelperMethods;
using Sprint33.Areas.Store.Models;
using Sprint33.Models;
using Sprint33.Services;
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
                                         orderby p.Id descending
                                         select new ProductContentVM
                                         {
                                             Id = p.Id,
                                             ImageUrl = p.ImageUrl,
                                             Name = p.Name,
                                             Price = p.SellingPrice,
                                             IsOnSale = p.IsOnSale,
                                             DiscountedPrice = p.DiscountedPrice,
                                             DiscountedRate = p.IsOnSale ? p.DiscountedRate : 0,
                                             ProductLink = "/store/shop/product-details/" + p.Slug
                                         }).ToListAsync(),
                Categories = db.Categories.ToList()
            };

            var onePageOfProducts = model.ProductContents.ToPagedList(pageNumber, 20);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(model);
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
                DiscountedRate = product.IsOnSale ? product.DiscountedRate : 0,
                SellingPrice = product.SellingPrice
            };

            return View("ProductDetails", model);
        }

        public ActionResult CartItemsPartial()
        {
            var activeCart = new ShoppingCart(this.HttpContext, db);

            //var model = new CartItemsVM
            //{
            //    LinkToSummary = "/store/cart/summary/",
            //    NumberOfItemsInCart = activeCart.GetCount()
            //};

            ViewBag.LinkToSummary = "/store/cart/summary/";
            ViewBag.NumberOfItemsInCart = activeCart.GetCount();

            return PartialView("_CartItemsPartial");
        }

    }
}