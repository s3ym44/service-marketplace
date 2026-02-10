using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TemplatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Templates
        public async Task<IActionResult> Index(string renovationType = null)
        {
            var query = _context.ServiceTemplates
                .Include(t => t.Items)
                .ThenInclude(i => i.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(renovationType))
            {
                query = query.Where(t => t.RenovationType == renovationType);
            }

            ViewBag.SelectedRenovationType = renovationType;
            ViewBag.RenovationTypes = await _context.ServiceTemplates
                .Where(t => !string.IsNullOrEmpty(t.RenovationType))
                .Select(t => t.RenovationType)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

            return View(await query.OrderBy(t => t.Name).ToListAsync());
        }

        // GET: Templates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Templates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceTemplate model)
        {
            if (ModelState.IsValid)
            {
                _context.ServiceTemplates.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }
            return View(model);
        }

        // GET: Templates/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var template = await _context.ServiceTemplates
                .Include(t => t.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (template == null) return NotFound();

            // Populate products for dropdown
            ViewBag.Products = await _context.AdminPriceReferences
                .Where(p => p.IsActive)
                .Select(p => new { p.Id, Name = p.Category + " - " + p.MaterialName + " (" + p.Brand + ")" })
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(template);
        }

        // POST: Templates/Edit/5 (Update Header)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceTemplate model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    // Don't modify Items here, they are handled separately
                    _context.Entry(model).Collection(t => t.Items).IsModified = false;
                    
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Şablon güncellendi.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ServiceTemplates.AnyAsync(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }
             // Repopulate View Data if error
            var existingItems = await _context.ServiceTemplateItems
                .Include(i => i.Product)
                .Where(i => i.ServiceTemplateId == id)
                .ToListAsync();
            model.Items = existingItems;

            ViewBag.Products = await _context.AdminPriceReferences
                .Where(p => p.IsActive)
                .Select(p => new { p.Id, Name = p.Category + " - " + p.MaterialName + " (" + p.Brand + ")" })
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(model);
        }

        // POST: Templates/AddItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int serviceTemplateId, int adminPriceReferenceId, decimal quantity, string subCategory, string notes, string defaultNote)
        {
            var template = await _context.ServiceTemplates.FindAsync(serviceTemplateId);
            if (template == null) return NotFound();

            // Get the next display order
            var maxOrder = await _context.ServiceTemplateItems
                .Where(i => i.ServiceTemplateId == serviceTemplateId)
                .Select(i => (int?)i.DisplayOrder)
                .MaxAsync() ?? 0;

            var item = new ServiceTemplateItem
            {
                ServiceTemplateId = serviceTemplateId,
                AdminPriceReferenceId = adminPriceReferenceId,
                Quantity = quantity,
                SubCategory = subCategory,
                DisplayOrder = maxOrder + 1,
                Notes = notes,
                DefaultNote = defaultNote
            };

            _context.ServiceTemplateItems.Add(item);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Ürün eklendi.";

            return RedirectToAction(nameof(Edit), new { id = serviceTemplateId });
        }

        // POST: Templates/DeleteItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.ServiceTemplateItems.FindAsync(id);
            if (item == null) return NotFound();

            int templateId = item.ServiceTemplateId;
            _context.ServiceTemplateItems.Remove(item);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Ürün silindi.";

            return RedirectToAction(nameof(Edit), new { id = templateId });
        }

        // POST: Templates/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _context.ServiceTemplates.FindAsync(id);
            if (template != null)
            {
                _context.ServiceTemplates.Remove(template);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Şablon silindi.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
