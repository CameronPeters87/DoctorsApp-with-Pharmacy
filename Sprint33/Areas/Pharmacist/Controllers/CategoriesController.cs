using PagedList;
using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class CategoriesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var model = new CategoryVM
            {
                Categories = await db.Categories.ToListAsync()
            };

            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryVM model)
        {
            if (ModelState.IsValid)
            {
                if (await db.Categories.AnyAsync(c => c.Name.Equals(model.Name)))
                {
                    TempData["Error"] = "That category already exists";
                    return RedirectToAction("Index");
                }
                db.Categories.Add(new Category
                {
                    Name = model.Name
                });

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public async Task<int> RemoveCategory(int categoryId)
        {

            Category category = await db.Categories.FindAsync(categoryId);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return categoryId;
        }

    }
}