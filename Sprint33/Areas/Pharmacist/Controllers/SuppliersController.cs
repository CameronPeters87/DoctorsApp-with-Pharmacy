using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class SuppliersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Suppliers
        public ActionResult Index()
        {
            var model = new SupplierListVM();
            model.Suppliers = db.Suppliers.ToList();
            return View(model);
        }

        public ActionResult AddSupplier()
        {
            AddSupplierVM model = new AddSupplierVM();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddSupplier(AddSupplierVM model)
        {
            if (ModelState.IsValid)
            {
                if (db.Suppliers.Any(x => x.Name.Equals(model.Name)))
                {
                    ModelState.AddModelError("", "The Name already exists");
                    return View(model);
                }
                // Add Address
                db.Addresses.Add(new PharmacyEntities.Address
                {
                    Street_Number = model.Address.Street_Number,
                    Route = model.Address.Route,
                    City = model.Address.City,
                    Province = model.Address.Province,
                    ZipCode = model.Address.ZipCode,
                    Country = model.Address.Country
                });

                await db.SaveChangesAsync();
                var supplierAddress = await db.Addresses.OrderByDescending(a => a.Id)
                    .FirstOrDefaultAsync();

                // Add Supplier
                db.Suppliers.Add(new Supplier
                {
                    Name = model.Name,
                    Email = model.Email,
                    ContactNumber = model.ContactNumber,
                    Notes = model.Notes,
                    AddressId = supplierAddress.Id,
                    Address = supplierAddress
                });

                // Add notification
                db.Notifications.Add(new Notification
                {
                    CreatedDate = DateTime.Now,
                    Message = "You have registered a new Supplier to the pharmacy: " +
                        model.Name,
                    isRead = false,
                    Icon = "fa-truck",
                    BackgroundColorIcon = "bg-warning"
                });

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> EditSupplier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            var model = new AddSupplierVM
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Email = supplier.Email,
                ContactNumber = supplier.ContactNumber,
                Notes = supplier.Notes,
                AddressId = supplier.AddressId,
                Address = supplier.Address
            };

            return View(model);
        }
    }
}