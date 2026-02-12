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

        // GET: Admin/Index
        public async Task<IActionResult> Index(string category, string brand, bool showInactive = false)
        {
            var query = _context.AdminPriceReferences.Include(p => p.CategoryModel).AsQueryable();

            if (!showInactive)
                query = query.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category || (p.CategoryModel != null && p.CategoryModel.Name == category));

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.Brand.Contains(brand));

            var prices = await query.OrderBy(p => p.Category).ThenBy(p => p.MaterialName).ToListAsync();

            // Get unique categories for filter dropdown
            ViewBag.Categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .Select(c => c.Name)
                .ToListAsync();

            // Fallback to distinct strings if DB categories are empty (unlikely with seed)
            if (ViewBag.Categories == null || ((List<string>)ViewBag.Categories).Count == 0)
            {
                 ViewBag.Categories = await _context.AdminPriceReferences
                    .Select(p => p.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();
            }

            ViewBag.CurrentCategory = category;
            ViewBag.CurrentBrand = brand;
            ViewBag.ShowInactive = showInactive;

            return View(prices);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            return View(new AdminPriceReference { IsActive = true, RegionModifier = 1.0 });
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,MaterialName,Description,Brand,Quality,Quantity,Unit,BasePrice,IsLabor,IsActive,RegionModifier")] AdminPriceReference model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}")
                    .ToList();
                TempData["Error"] = "Validation: " + string.Join(" | ", errors);
            }

            if (ModelState.IsValid)
            {
                // Auto-fill Category string from ID if selected
                if (model.CategoryId.HasValue)
                {
                    var cat = await _context.Categories.FindAsync(model.CategoryId);
                    if (cat != null) model.Category = cat.Name;
                }
                
                // If no CategoryId but Category string is there, try to match or create? 
                // For now, let's assume UI provides ID if possible, or string if custom.

                _context.AdminPriceReferences.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Price added successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            return View(model);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var price = await _context.AdminPriceReferences.FindAsync(id);
            if (price == null) return NotFound();

            ViewBag.Categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            return View(price);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category,CategoryId,MaterialName,Description,Brand,Quality,Quantity,Unit,BasePrice,IsLabor,IsActive,RegionModifier")] AdminPriceReference model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}")
                    .ToList();
                TempData["Error"] = "Validation: " + string.Join(" | ", errors);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Auto-fill Category string from ID if selected
                    if (model.CategoryId.HasValue)
                    {
                        var cat = await _context.Categories.FindAsync(model.CategoryId);
                        if (cat != null) model.Category = cat.Name;
                    }

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

            ViewBag.Categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
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

            // Soft delete
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
        // ... (Existing RegionModifier actions)

        #region Category Management

        // GET: Admin/Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
            return View(categories);
        }

        // GET: Admin/CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([Bind("Name,Description,DisplayOrder,IsActive")] Category model)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction(nameof(Categories));
            }
            return View(model);
        }

        // GET: Admin/EditCategory/5
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: Admin/EditCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, [Bind("Id,Name,Description,DisplayOrder,IsActive")] Category model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Category updated successfully.";
                    return RedirectToAction(nameof(Categories));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Categories.AnyAsync(c => c.Id == id))
                        return NotFound();
                    throw;
                }
            }
            return View(model);
        }

        // POST: Admin/DeleteCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            // Check if category has products
            var hasProducts = await _context.AdminPriceReferences.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
            {
                TempData["Error"] = "Bu kategori altında ürünler var. Önce ürünleri silin veya başka kategoriye taşıyın.";
                return RedirectToAction(nameof(Categories));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"'{category.Name}' kategorisi başarıyla silindi.";
            return RedirectToAction(nameof(Categories));
        }

        #endregion

        #region Excel Import

        // GET: Admin/Import
        public IActionResult Import()
        {
            return View();
        }

        // POST: Admin/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen bir dosya seçin.");
                return View();
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Sadece .xlsx formatı desteklenir.");
                return View();
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var workbook = new ClosedXML.Excel.XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header

                        int addedCount = 0;
                        int updatedCount = 0;

                        foreach (var row in rows)
                        {
                            // Expected Columns: 
                            // 1: Category Name
                            // 2: Material Name
                            // 3: Description
                            // 4: Brand
                            // 5: Quality
                            // 6: Quantity
                            // 7: Unit
                            // 8: Base Price
                            // 9: IsLabor (true/false)

                            var categoryName = row.Cell(1).GetValue<string>();
                            var materialName = row.Cell(2).GetValue<string>();
                            
                            if (string.IsNullOrWhiteSpace(materialName)) continue;

                            var description = row.Cell(3).GetValue<string>();
                            var brand = row.Cell(4).GetValue<string>();
                            var quality = row.Cell(5).GetValue<string>();
                            var quantity = row.Cell(6).GetValue<decimal>();
                            var unit = row.Cell(7).GetValue<string>();
                            var basePrice = row.Cell(8).GetValue<decimal>();
                            var isLaborStr = row.Cell(9).GetValue<string>();
                            bool isLabor = isLaborStr?.ToLower() == "true" || isLaborStr == "1" || isLaborStr?.ToLower() == "evet";

                            // Find or Create Category
                            Category category = null;
                            if (!string.IsNullOrWhiteSpace(categoryName))
                            {
                                category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                                if (category == null)
                                {
                                    category = new Category { Name = categoryName, IsActive = true };
                                    _context.Categories.Add(category);
                                    await _context.SaveChangesAsync();
                                }
                            }

                            // Check if product exists (Update or Add)
                            var existing = await _context.AdminPriceReferences.FirstOrDefaultAsync(p => 
                                p.MaterialName == materialName && 
                                p.Brand == brand && 
                                p.Quality == quality);

                            if (existing != null)
                            {
                                existing.BasePrice = basePrice;
                                existing.Quantity = quantity;
                                existing.Unit = unit;
                                existing.Description = description;
                                existing.IsLabor = isLabor;
                                if (category != null)
                                {
                                    existing.CategoryId = category.Id;
                                    existing.Category = category.Name;
                                }
                                updatedCount++;
                            }
                            else
                            {
                                var newProduct = new AdminPriceReference
                                {
                                    MaterialName = materialName,
                                    Description = description,
                                    Brand = brand ?? "",
                                    Quality = quality ?? "Standart",
                                    Quantity = quantity,
                                    Unit = unit ?? "Adet",
                                    BasePrice = basePrice,
                                    IsLabor = isLabor,
                                    IsActive = true,
                                    RegionModifier = 1.0,
                                    Category = category?.Name ?? categoryName,
                                    CategoryId = category?.Id
                                };
                                _context.AdminPriceReferences.Add(newProduct);
                                addedCount++;
                            }
                        }

                        await _context.SaveChangesAsync();
                        TempData["Success"] = $"İçe aktarım tamamlandı. {addedCount} yeni eklendi, {updatedCount} güncellendi.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View();
            }
        }

        #endregion

        #region Recipe Import

        // GET: Admin/ImportRecipe
        public IActionResult ImportRecipe()
        {
            ViewBag.ServicePackages = _context.ServicePackages
                .Where(sp => sp.IsActive)
                .OrderBy(sp => sp.Name)
                .ToList();
            return View();
        }

        // POST: Admin/ImportRecipe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportRecipe(IFormFile file, int? servicePackageId, string? newPackageName, string? newPackageCode, int? mainCategoryId)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Lütfen bir dosya seçin.";
                return RedirectToAction(nameof(ImportRecipe));
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Sadece .xlsx formatı desteklenir.";
                return RedirectToAction(nameof(ImportRecipe));
            }

            try
            {
                ServicePackage package;

                if (servicePackageId.HasValue && servicePackageId > 0)
                {
                    package = await _context.ServicePackages
                        .Include(sp => sp.Items)
                        .FirstOrDefaultAsync(sp => sp.Id == servicePackageId);

                    if (package == null)
                    {
                        TempData["Error"] = "Seçilen paket bulunamadı.";
                        return RedirectToAction(nameof(ImportRecipe));
                    }

                    // Mevcut kalemleri temizle
                    _context.PackageItems.RemoveRange(package.Items);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(newPackageName))
                    {
                        TempData["Error"] = "Yeni paket adı giriniz.";
                        return RedirectToAction(nameof(ImportRecipe));
                    }

                    package = new ServicePackage
                    {
                        Name = newPackageName,
                        Code = newPackageCode ?? ("PKT" + DateTime.Now.ToString("yyMMddHHmm")),
                        MainCategoryId = mainCategoryId ?? 1,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                    };
                    _context.ServicePackages.Add(package);
                    await _context.SaveChangesAsync();
                }

                // Excel oku
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var workbook = new ClosedXML.Excel.XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                int order = 1;
                int addedCount = 0;
                var errors = new List<string>();

                foreach (var row in rows)
                {
                    var mainCategory = row.Cell(1).GetValue<string>()?.Trim();
                    var subCategory = row.Cell(2).GetValue<string>()?.Trim();
                    var itemType = row.Cell(3).GetValue<string>()?.Trim();
                    var name = row.Cell(4).GetValue<string>()?.Trim();
                    var unit = row.Cell(5).GetValue<string>()?.Trim();
                    var consumption = row.Cell(6).GetValue<string>()?.Trim();

                    if (string.IsNullOrWhiteSpace(name)) continue;
                    if (mainCategory == "Ana Kategori") continue;

                    if (string.IsNullOrWhiteSpace(mainCategory) || string.IsNullOrWhiteSpace(itemType))
                    {
                        errors.Add($"Satır {row.RowNumber()}: Ana Kategori veya Ürün Türü boş. ({name})");
                        continue;
                    }

                    var packageItem = new PackageItem
                    {
                        ServicePackageId = package.Id,
                        MainCategory = mainCategory,
                        SubCategory = subCategory,
                        ItemType = itemType,
                        Name = name,
                        Unit = unit ?? "adet",
                        ConsumptionFormula = consumption,
                        DisplayOrder = order++,
                        IsRequired = itemType != "Alternatif Ürün"
                    };

                    _context.PackageItems.Add(packageItem);
                    addedCount++;
                }

                await _context.SaveChangesAsync();

                var message = $"Reçete başarıyla yüklendi! {addedCount} kalem eklendi. Paket: {package.Name}";
                if (errors.Any())
                {
                    message += $" ({errors.Count} satır atlandı)";
                    TempData["Warning"] = string.Join("\n", errors.Take(10));
                }
                TempData["Success"] = message;
                return RedirectToAction(nameof(RecipeDetails), new { id = package.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return RedirectToAction(nameof(ImportRecipe));
            }
        }

        // GET: Admin/RecipeDetails/5
        public async Task<IActionResult> RecipeDetails(int id)
        {
            var package = await _context.ServicePackages
                .Include(sp => sp.Items.OrderBy(i => i.DisplayOrder))
                .Include(sp => sp.MainCategory)
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (package == null) return NotFound();

            // Grupla: MainCategory → SubCategory → Items (strongly typed)
            ViewBag.GroupedItems = package.Items
                .GroupBy(i => i.MainCategory)
                .OrderBy(g => g.First().DisplayOrder)
                .Select(g => new RecipeMainGroup
                {
                    MainCategory = g.Key,
                    TotalCount = g.Count(),
                    SubGroups = g.GroupBy(i => i.SubCategory ?? "")
                        .OrderBy(sg => sg.First().DisplayOrder)
                        .Select(sg => new RecipeSubGroup
                        {
                            SubCategory = sg.Key,
                            Items = sg.OrderBy(i => i.DisplayOrder).ToList()
                        }).ToList()
                }).ToList();

            return View(package);
        }

        // GET: Admin/DownloadRecipeTemplate
        public IActionResult DownloadRecipeTemplate()
        {
            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var ws = workbook.Worksheets.Add("Reçete Şablonu");

            ws.Cell(1, 1).Value = "Ana Kategori";
            ws.Cell(1, 2).Value = "Alt Kategori";
            ws.Cell(1, 3).Value = "Ürün Türü";
            ws.Cell(1, 4).Value = "Malzeme / Ürün Adı";
            ws.Cell(1, 5).Value = "Birim";
            ws.Cell(1, 6).Value = "Tüketim";

            var headerRange = ws.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#1a2332");
            headerRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

            ws.Cell(2, 1).Value = "Zemin";
            ws.Cell(2, 2).Value = "Hafriyat";
            ws.Cell(2, 3).Value = "İşçilik";
            ws.Cell(2, 4).Value = "Zemin Kırım ve Söküm";
            ws.Cell(2, 5).Value = "m²";
            ws.Cell(2, 6).Value = "1 m² / alan";

            ws.Cell(3, 1).Value = "Zemin";
            ws.Cell(3, 2).Value = "Döşeme";
            ws.Cell(3, 3).Value = "Ana Ürün";
            ws.Cell(3, 4).Value = "Yer Seramiği 60x60";
            ws.Cell(3, 5).Value = "m²";
            ws.Cell(3, 6).Value = "1.1 m² / alan";

            ws.Cell(4, 1).Value = "Zemin";
            ws.Cell(4, 2).Value = "Döşeme";
            ws.Cell(4, 3).Value = "Yardımcı Malzeme";
            ws.Cell(4, 4).Value = "Seramik Yapıştırıcı";
            ws.Cell(4, 5).Value = "kg";
            ws.Cell(4, 6).Value = "8-12 kg / m²";

            var validTypes = new[] { "İşçilik", "Ana Ürün", "Yardımcı Malzeme", "Alternatif Ürün" };
            var typeValidation = ws.Range("C2:C500").CreateDataValidation();
            typeValidation.List($"\"{string.Join(",", validTypes)}\"");

            ws.Column(1).Width = 18;
            ws.Column(2).Width = 18;
            ws.Column(3).Width = 22;
            ws.Column(4).Width = 40;
            ws.Column(5).Width = 10;
            ws.Column(6).Width = 20;

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Recete_Sablonu.xlsx");
        }

        #endregion
    }
}

// Helper classes for RecipeDetails view (avoid anonymous types in Razor)
namespace ServiceMarketplace.ViewModels
{
    public class RecipeMainGroup
    {
        public string MainCategory { get; set; } = "";
        public int TotalCount { get; set; }
        public List<RecipeSubGroup> SubGroups { get; set; } = new();
    }

    public class RecipeSubGroup
    {
        public string SubCategory { get; set; } = "";
        public List<ServiceMarketplace.Models.PackageItem> Items { get; set; } = new();
    }
}
