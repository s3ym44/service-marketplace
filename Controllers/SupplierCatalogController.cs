using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;
using System.Security.Claims;

namespace ServiceMarketplace.Controllers
{
    [Authorize(Roles = "MaterialSupplier,LaborProvider")]
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
        public async Task<IActionResult> Create([Bind("Category,PackageItemId,Brand,ProductName,ModelCode,Description,UnitPrice,Unit,InStock,StockQuantity")] SupplierProduct model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // SupplierId ve Supplier form'dan gelmez, server tarafında atanır
            model.SupplierId = userId!;
            ModelState.Remove("SupplierId");
            ModelState.Remove("Supplier");
            
            if (ModelState.IsValid)
            {
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category,PackageItemId,Brand,ProductName,ModelCode,Description,UnitPrice,Unit,InStock,StockQuantity")] SupplierProduct model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // SupplierId ve Supplier form'dan gelmez
            ModelState.Remove("SupplierId");
            ModelState.Remove("Supplier");

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

        #region Excel Import/Export

        // GET: SupplierCatalog/ImportProducts
        public IActionResult ImportProducts()
        {
            return View();
        }

        // GET: SupplierCatalog/DownloadProductTemplate
        public IActionResult DownloadProductTemplate()
        {
            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var ws = workbook.Worksheets.Add("Ürün Şablonu");

            ws.Cell(1, 1).Value = "Kategori";
            ws.Cell(1, 2).Value = "Marka";
            ws.Cell(1, 3).Value = "Ürün Adı";
            ws.Cell(1, 4).Value = "Model Kodu";
            ws.Cell(1, 5).Value = "Açıklama";
            ws.Cell(1, 6).Value = "Birim Fiyat (₺)";
            ws.Cell(1, 7).Value = "Birim";
            ws.Cell(1, 8).Value = "Stokta Var (Evet/Hayır)";
            ws.Cell(1, 9).Value = "Stok Adedi";

            var headerRange = ws.Range(1, 1, 1, 9);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#1a2332");
            headerRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

            ws.Cell(2, 1).Value = "Boya";
            ws.Cell(2, 2).Value = "Marshall";
            ws.Cell(2, 3).Value = "İç Cephe Duvar Boyası";
            ws.Cell(2, 4).Value = "MRS-2024-IC";
            ws.Cell(2, 5).Value = "15 Lt su bazlı mat boya";
            ws.Cell(2, 6).Value = 1200;
            ws.Cell(2, 7).Value = "adet";
            ws.Cell(2, 8).Value = "Evet";
            ws.Cell(2, 9).Value = 50;

            ws.Cell(3, 1).Value = "Seramik";
            ws.Cell(3, 2).Value = "Ege Seramik";
            ws.Cell(3, 3).Value = "Yer Seramiği 60x60";
            ws.Cell(3, 4).Value = "EGE-6060-P";
            ws.Cell(3, 5).Value = "Premium kalite yer seramiği";
            ws.Cell(3, 6).Value = 180;
            ws.Cell(3, 7).Value = "m²";
            ws.Cell(3, 8).Value = "Evet";
            ws.Cell(3, 9).Value = 200;

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Urun_Sablonu.xlsx");
        }

        // POST: SupplierCatalog/ImportProducts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportProducts(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Lütfen bir dosya seçin.";
                return RedirectToAction(nameof(ImportProducts));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var workbook = new ClosedXML.Excel.XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                int addedCount = 0;
                int updatedCount = 0;
                var errors = new List<string>();

                foreach (var row in rows)
                {
                    var category = row.Cell(1).GetValue<string>()?.Trim();
                    var brand = row.Cell(2).GetValue<string>()?.Trim();
                    var productName = row.Cell(3).GetValue<string>()?.Trim();
                    var modelCode = row.Cell(4).GetValue<string>()?.Trim();
                    var description = row.Cell(5).GetValue<string>()?.Trim();
                    var unitPriceStr = row.Cell(6).GetValue<string>()?.Trim();
                    var unit = row.Cell(7).GetValue<string>()?.Trim();
                    var inStockStr = row.Cell(8).GetValue<string>()?.Trim()?.ToLower();
                    var stockQtyStr = row.Cell(9).GetValue<string>()?.Trim();

                    if (string.IsNullOrWhiteSpace(productName)) continue;

                    if (!decimal.TryParse(unitPriceStr, out decimal unitPrice))
                    {
                        errors.Add($"Satır {row.RowNumber()}: Geçersiz fiyat ({productName})");
                        continue;
                    }

                    bool inStock = inStockStr == "evet" || inStockStr == "true" || inStockStr == "1";
                    int.TryParse(stockQtyStr, out int stockQty);

                    var existing = await _context.SupplierProducts
                        .FirstOrDefaultAsync(sp => sp.SupplierId == userId
                            && sp.Brand == brand
                            && sp.ProductName == productName
                            && sp.IsActive);

                    if (existing != null)
                    {
                        existing.UnitPrice = unitPrice;
                        existing.Unit = unit ?? existing.Unit;
                        existing.ModelCode = modelCode;
                        existing.Description = description;
                        existing.InStock = inStock;
                        existing.StockQuantity = stockQty;
                        existing.UpdatedAt = DateTime.UtcNow;
                        updatedCount++;
                    }
                    else
                    {
                        var product = new SupplierProduct
                        {
                            SupplierId = userId!,
                            Category = category ?? "Genel",
                            Brand = brand ?? "",
                            ProductName = productName,
                            ModelCode = modelCode,
                            Description = description,
                            UnitPrice = unitPrice,
                            Unit = unit ?? "adet",
                            InStock = inStock,
                            StockQuantity = stockQty,
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.SupplierProducts.Add(product);
                        addedCount++;
                    }
                }

                await _context.SaveChangesAsync();

                var message = $"İçe aktarım tamamlandı! {addedCount} yeni eklendi, {updatedCount} güncellendi.";
                if (errors.Any())
                {
                    message += $" ({errors.Count} satır atlandı)";
                }
                TempData["Success"] = message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return RedirectToAction(nameof(ImportProducts));
            }
        }

        // GET: SupplierCatalog/ExportProducts
        public async Task<IActionResult> ExportProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _context.SupplierProducts
                .Where(sp => sp.SupplierId == userId && sp.IsActive)
                .OrderBy(sp => sp.Category)
                .ThenBy(sp => sp.Brand)
                .ToListAsync();

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var ws = workbook.Worksheets.Add("Ürünlerim");

            ws.Cell(1, 1).Value = "Kategori";
            ws.Cell(1, 2).Value = "Marka";
            ws.Cell(1, 3).Value = "Ürün Adı";
            ws.Cell(1, 4).Value = "Model Kodu";
            ws.Cell(1, 5).Value = "Açıklama";
            ws.Cell(1, 6).Value = "Birim Fiyat (₺)";
            ws.Cell(1, 7).Value = "Birim";
            ws.Cell(1, 8).Value = "Stokta Var";
            ws.Cell(1, 9).Value = "Stok Adedi";

            var headerRange = ws.Range(1, 1, 1, 9);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#1a2332");
            headerRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

            int r = 2;
            foreach (var p in products)
            {
                ws.Cell(r, 1).Value = p.Category;
                ws.Cell(r, 2).Value = p.Brand;
                ws.Cell(r, 3).Value = p.ProductName;
                ws.Cell(r, 4).Value = p.ModelCode ?? "";
                ws.Cell(r, 5).Value = p.Description ?? "";
                ws.Cell(r, 6).Value = p.UnitPrice;
                ws.Cell(r, 7).Value = p.Unit;
                ws.Cell(r, 8).Value = p.InStock ? "Evet" : "Hayır";
                ws.Cell(r, 9).Value = p.StockQuantity ?? 0;
                r++;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Urunlerim_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        #endregion
    }
}
