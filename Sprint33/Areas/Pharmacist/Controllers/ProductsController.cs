using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pharmacist/Products
        public async Task<ActionResult> Index(string search)
        {
            ProductListVM model = new ProductListVM();

            model.Products = await db.Products
                .Where(m => m.Name.Contains(search) ||
                    search == null || search == string.Empty)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(model);
        }

        public ActionResult AddProduct()
        {
            AddProductVM model = new AddProductVM
            {
                Categories = new SelectList(db.Categories.ToList(), "Id", "Name"),
                Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(AddProductVM model)
        {
            if (ModelState.IsValid)
            {
                Supplier supplier = db.Suppliers.Find(model.SupplierId);
                // Check for and set sku
                // Sku must be Upper case and replace spaces with dashes
                string sku = "";
                string CustomSkuName = model.Name + model.PackSize;
                if (string.IsNullOrWhiteSpace(model.SkuCode))
                {
                    sku = CustomSkuName.Replace(" ", "-").ToLower();
                }
                else
                {
                    sku = model.SkuCode.Replace(" ", "-").ToLower();
                }

                // Check if sku is unique
                if (db.Products.Any(x => x.SkuCode.Equals(sku)))
                {
                    ModelState.AddModelError("", "The Sku Code already exists");
                    model = new AddProductVM
                    {
                        Categories = new SelectList(db.Categories.ToList(), "Id", "Name"),
                        Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name")
                    };
                    return View(model);
                }

                // Check for and set slug
                // Slug must be lower case and replace spaces with dashes
                string slug = "";
                string name = sku;
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = name.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                // Check if slug and title is unique
                if (db.Products.Any(x => x.Slug.Equals(slug)))
                {
                    ModelState.AddModelError("", "The slug or title already exists");
                    model = new AddProductVM
                    {
                        Categories = new SelectList(db.Categories.ToList(), "Id", "Name"),
                        Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name")
                    };
                    return View(model);
                }

                #region Upload Image and Generate Barcode
                // Upload Image
                string imgUrl = "";
                try
                {
                    if (model.Image == null)
                    {
                        imgUrl = "/Files/Images/noproduct.png";
                    }
                    else if (model.Image.ContentLength > 0)
                    {
                        var uploadPath = "~/Files/Images";
                        string _FileName = Path.GetFileName(model.Image.FileName);
                        string UniqueFileName = model.SkuCode +
                            "-" + _FileName.ToLower();

                        string _path = Path.Combine(Server
                            .MapPath(uploadPath), UniqueFileName);
                        model.Image.SaveAs(_path);

                        imgUrl = "/Files/Images/" + UniqueFileName;
                    }
                    else { }
                }
                catch (Exception e)
                {
                    throw e;
                }

                /* 
                 * Generate Barcode
                 */

                // Check if SkuCode exists already
                if (db.Products.Any(x => x.SkuCode.Equals(sku)))
                {
                    ModelState.AddModelError("", "The barcode already exists");
                    return View(model);
                }

                // Save Barcode to that path
                //string _barcodePath = Path.Combine(Server
                //    .MapPath("~/Files/Barcodes"), sku + ".png");

                //// Generate a Simple BarCode image and save as PNG
                ////using IronBarCode;
                //GeneratedBarcode MyBarCode = BarcodeWriter
                //    .CreateBarcode(sku,
                //    BarcodeWriterEncoding.Code128);

                //MyBarCode.SetMargins(50);

                //MyBarCode.SaveAsPng(_barcodePath);

                //string _barcodeUrl = "/Files/Barcodes/" + sku + ".png";
                string _barcodeUrl = "/";
                #endregion

                // Add Product
                db.Products.Add(new Product
                {
                    Name = model.Name,
                    SkuCode = sku,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    Slug = slug,
                    ImageUrl = imgUrl,
                    BarcodeUrl = _barcodeUrl,
                    PackSize = model.PackSize,
                    Quantity = 0,
                    SupplierPrice = model.SupplierPrice,
                    SupplierId = model.SupplierId,
                    SellingPrice = model.SellingPrice,
                    IsOnSale = false,
                    Supplier = supplier
                });

                await db.SaveChangesAsync();

                // Add notification
                var product = await db.Products.OrderByDescending(m => m.Id).FirstOrDefaultAsync();
                db.Notifications.Add(new Notification
                {
                    CreatedDate = DateTime.Now,
                    Message = "You have added a new product to the pharmacy: " +
                        product.Name,
                    isRead = false,
                    Icon = "fa-check",
                    BackgroundColorIcon = "bg-success"
                });

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            model = new AddProductVM
            {
                Categories = new SelectList(db.Categories.ToList(), "Id", "Name"),
                Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name")
            };
            return View(model);
        }

        public async Task<ActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var model = new AddProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Slug = product.Slug,
                ImageUrl = product.ImageUrl,
                SkuCode = product.SkuCode,
                BarcodeUrl = product.BarcodeUrl,
                PackSize = product.PackSize,
                SellingPrice = product.SellingPrice,
                SupplierPrice = product.SupplierPrice,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                Categories = new SelectList(db.Categories.ToList(), "Id", "Name"),
                Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name")
            };

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> EditProduct(AddProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model = new AddProductVM
                {
                    Categories = new SelectList(db.Categories.ToList(), "Id", "Name"),
                    Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name")
                };
                return View(model);
            }

            var product = await db.Products.FindAsync(model.Id);

            #region Upload Image
            // Upload Image
            string imgUrl = "";
            try
            {
                if (model.Image == null)
                {
                    imgUrl = model.ImageUrl;
                }
                else if (model.Image.ContentLength > 0)
                {
                    var uploadPath = "~/Files/Images";
                    string _FileName = Path.GetFileName(model.Image.FileName);
                    string UniqueFileName = model.SkuCode +
                        "-" + _FileName.ToLower();

                    string _path = Path.Combine(Server
                        .MapPath(uploadPath), UniqueFileName);
                    model.Image.SaveAs(_path);

                    imgUrl = "/Files/Images/" + UniqueFileName;
                }
                else { }
            }
            catch (Exception e)
            {
                throw e;
            }

            #endregion

            product.Name = model.Name;
            product.Description = model.Description;
            product.CategoryId = model.CategoryId;
            product.ImageUrl = imgUrl;
            product.PackSize = model.PackSize;
            product.SellingPrice = model.SellingPrice;
            product.SupplierId = model.SupplierId;
            product.SupplierPrice = model.SupplierPrice;

            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}