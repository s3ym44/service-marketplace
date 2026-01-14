using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;
using ServiceMarketplace.ViewModels;

namespace ServiceMarketplace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin - Dashboard with price list
        public async Task<IActionResult> Index(string category, string brand, bool showInactive = false)
        {
            var query = _context.AdminPriceReferences.AsQueryable();

            if (!showInactive)
                query = query.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.Brand.Contains(brand));

            var prices = await query.OrderBy(p => p.Category).ThenBy(p => p.MaterialName).ToListAsync();

            // Get unique categories for filter dropdown
            ViewBag.Categories = await _context.AdminPriceReferences
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            ViewBag.CurrentCategory = category;
            ViewBag.CurrentBrand = brand;
            ViewBag.ShowInactive = showInactive;

            return View(prices);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.AdminPriceReferences
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return View(new AdminPriceReference { IsActive = true, RegionModifier = 1.0 });
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminPriceReference model)
        {
            if (ModelState.IsValid)
            {
                _context.AdminPriceReferences.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Price added successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.AdminPriceReferences
                .Select(p => p.Category)
                .Distinct()
                .ToList();

            return View(model);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var price = await _context.AdminPriceReferences.FindAsync(id);
            if (price == null) return NotFound();

            ViewBag.Categories = await _context.AdminPriceReferences
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return View(price);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminPriceReference model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Price updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.AdminPriceReferences.AnyAsync(p => p.Id == id))
                        return NotFound();
                    throw;
                }
            }

            ViewBag.Categories = await _context.AdminPriceReferences
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            return View(model);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var price = await _context.AdminPriceReferences.FindAsync(id);
            if (price == null) return NotFound();

            // Soft delete - just deactivate
            price.IsActive = false;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Price deactivated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Restore/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var price = await _context.AdminPriceReferences.FindAsync(id);
            if (price == null) return NotFound();

            price.IsActive = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Price restored successfully.";
            return RedirectToAction(nameof(Index), new { showInactive = true });
        }

        // GET: Admin/BulkUpdate
        public async Task<IActionResult> BulkUpdate()
        {
            var viewModel = new BulkUpdateViewModel
            {
                Categories = await _context.AdminPriceReferences
                    .Where(p => p.IsActive)
                    .Select(p => p.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Admin/BulkUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkUpdate(BulkUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var query = _context.AdminPriceReferences.Where(p => p.IsActive);

                if (!string.IsNullOrEmpty(model.SelectedCategory))
                    query = query.Where(p => p.Category == model.SelectedCategory);

                var prices = await query.ToListAsync();
                var multiplier = 1 + (model.PercentageChange / 100);

                foreach (var price in prices)
                {
                    price.BasePrice = Math.Round(price.BasePrice * multiplier, 2);
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = $"{prices.Count} prices updated by {model.PercentageChange:+0.##;-0.##}%.";
                return RedirectToAction(nameof(Index));
            }

            model.Categories = await _context.AdminPriceReferences
                .Where(p => p.IsActive)
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            return View(model);
        }

        // GET: Admin/RegionModifier
        public async Task<IActionResult> RegionModifier()
        {
            var viewModel = new RegionModifierViewModel
            {
                Categories = await _context.AdminPriceReferences
                    .Where(p => p.IsActive)
                    .Select(p => p.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Admin/RegionModifier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegionModifier(RegionModifierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var query = _context.AdminPriceReferences.Where(p => p.IsActive);

                if (!string.IsNullOrEmpty(model.SelectedCategory))
                    query = query.Where(p => p.Category == model.SelectedCategory);

                var prices = await query.ToListAsync();

                foreach (var price in prices)
                {
                    price.RegionModifier = model.NewModifier;
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = $"{prices.Count} items updated with region modifier {model.NewModifier}.";
                return RedirectToAction(nameof(Index));
            }

            model.Categories = await _context.AdminPriceReferences
                .Where(p => p.IsActive)
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            return View(model);
        }
    }
}
