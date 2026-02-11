using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;
using System.Security.Claims;

namespace ServiceMarketplace.Controllers
{
    [Authorize(Roles = "Supplier")]
    public class SupplierCatalogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierCatalogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /SupplierCatalog
        public async Task<IActionResult> Index(string? category = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var query = _context.SupplierProducts
                .Include(sp => sp.PackageItem)
                .Where(sp => sp.SupplierId == userId && sp.IsActive);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(sp => sp.Category == category);
            }

            var products = await query
                .OrderBy(sp => sp.Category)
                .ThenBy(sp => sp.Brand)
                .ToListAsync();

            // Get distinct categories for filter
            ViewBag.Categories = await _context.SupplierProducts
                .Where(sp => sp.SupplierId == userId && sp.IsActive)
                .Select(sp => sp.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            ViewBag.SelectedCategory = category;

            return View(products);
        }

        // GET: /SupplierCatalog/Create
        public async Task<IActionResult> Create()
        {
            // Load package items for dropdown
            ViewBag.PackageItems = await _context.PackageItems
                .Include(pi => pi.ServicePackage)
                .Where(pi => pi.ServicePackage.IsActive)
                .OrderBy(pi => pi.ServicePackage.Name)
                .ThenBy(pi => pi.DisplayOrder)
                .ToListAsync();

            return View();
        }

        // POST: /SupplierCatalog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierProduct model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (ModelState.IsValid)
            {
                model.SupplierId = userId!;
                model.CreatedAt = DateTime.UtcNow;
                model.IsActive = true;

                _context.SupplierProducts.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Ürün başarıyla kataloga eklendi!";
                return RedirectToAction(nameof(Index));
            }

            // Reload package items if validation fails
            ViewBag.PackageItems = await _context.PackageItems
                .Include(pi => pi.ServicePackage)
                .Where(pi => pi.ServicePackage.IsActive)
                .OrderBy(pi => pi.ServicePackage.Name)
                .ThenBy(pi => pi.DisplayOrder)
                .ToListAsync();

            return View(model);
        }

        // GET: /SupplierCatalog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _context.SupplierProducts
                .Where(sp => sp.Id == id && sp.SupplierId == userId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            // Load package items for dropdown
            ViewBag.PackageItems = await _context.PackageItems
                .Include(pi => pi.ServicePackage)
                .Where(pi => pi.ServicePackage.IsActive)
                .OrderBy(pi => pi.ServicePackage.Name)
                .ThenBy(pi => pi.DisplayOrder)
                .ToListAsync();

            return View(product);
        }

        // POST: /SupplierCatalog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierProduct model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.SupplierProducts
                        .Where(sp => sp.Id == id && sp.SupplierId == userId)
                        .FirstOrDefaultAsync();

                    if (existing == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existing.Category = model.Category;
                    existing.PackageItemId = model.PackageItemId;
                    existing.Brand = model.Brand;
                    existing.ProductName = model.ProductName;
                    existing.ModelCode = model.ModelCode;
                    existing.Description = model.Description;
                    existing.UnitPrice = model.UnitPrice;
                    existing.Unit = model.Unit;
                    existing.InStock = model.InStock;
                    existing.StockQuantity = model.StockQuantity;
                    existing.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Ürün başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Reload package items if validation fails
            ViewBag.PackageItems = await _context.PackageItems
                .Include(pi => pi.ServicePackage)
                .Where(pi => pi.ServicePackage.IsActive)
                .OrderBy(pi => pi.ServicePackage.Name)
                .ThenBy(pi => pi.DisplayOrder)
                .ToListAsync();

            return View(model);
        }

        // POST: /SupplierCatalog/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _context.SupplierProducts
                .Where(sp => sp.Id == id && sp.SupplierId == userId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            // Soft delete
            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Ürün başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _context.SupplierProducts.Any(e => e.Id == id && e.SupplierId == userId);
        }
    }
}
