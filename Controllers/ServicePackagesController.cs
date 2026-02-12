using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.Controllers
{
    public class ServicePackagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicePackagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServicePackages
        public async Task<IActionResult> Index(int? categoryId = null)
        {
            var query = _context.ServicePackages
                .Include(sp => sp.MainCategory)
                .Where(sp => sp.IsActive);

            if (categoryId.HasValue)
            {
                query = query.Where(sp => sp.MainCategoryId == categoryId.Value);
                var category = await _context.MainCategories.FindAsync(categoryId.Value);
                ViewBag.SelectedCategory = category?.Name;
            }

            var packages = await query
                .OrderBy(sp => sp.MainCategory.DisplayOrder)
                .ThenBy(sp => sp.Name)
                .ToListAsync();

            ViewBag.Categories = await _context.MainCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
            ViewBag.SelectedCategoryId = categoryId;

            return View(packages);
        }

        // GET: ServicePackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.ServicePackages
                .Include(sp => sp.MainCategory)
                .Include(sp => sp.Items.OrderBy(i => i.DisplayOrder))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }
    }
}
