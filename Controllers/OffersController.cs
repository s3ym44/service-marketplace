using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;
using ServiceMarketplace.ViewModels;
using System.Text.Json;

namespace ServiceMarketplace.Controllers
{
    [Authorize(Roles = "MaterialSupplier,LaborProvider")]
    public class OffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OffersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Offers/CreateLineItemsOffer/5 (for package-based listings)
        public async Task<IActionResult> CreateLineItemsOffer(int listingId)
        {
            var listing = await _context.Listings
                .Include(l => l.ServicePackage)
                    .ThenInclude(sp => sp.Items.OrderBy(i => i.DisplayOrder))
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == listingId);

            if (listing == null)
            {
                TempData["Error"] = "İlan bulunamadı.";
                return RedirectToAction("OpenListings", "Listings");
            }

            if (listing.ServicePackage == null)
            {
                TempData["Error"] = "Bu ilan paket tabanlı değil. Standart teklif formunu kullanın.";
                return RedirectToAction("Create", new { listingId });
            }

            // Parse dimensions and calculated metrics
            var dimensions = string.IsNullOrEmpty(listing.Dimensions) 
                ? null 
                : JsonSerializer.Deserialize<Dictionary<string, decimal>>(listing.Dimensions);
            
            var metrics = string.IsNullOrEmpty(listing.CalculatedMetrics)
                ? null
                : JsonSerializer.Deserialize<Dictionary<string, decimal>>(listing.CalculatedMetrics);

            ViewBag.Listing = listing;
            ViewBag.Dimensions = dimensions;
            ViewBag.Metrics = metrics;

            return View();
        }

        // POST: Offers/CreateLineItemsOffer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLineItemsOffer(int listingId, int estimatedDays, 
            int warrantyMonths, string[] itemIds, string[] brands, string[] productNames, 
            decimal[] quantities, decimal[] unitPrices)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var listing = await _context.Listings
                .Include(l => l.ServicePackage)
                .FirstOrDefaultAsync(l => l.Id == listingId);

            if (listing == null || listing.ServicePackage == null)
            {
                TempData["Error"] = "İlan bulunamadı.";
                return RedirectToAction("OpenListings", "Listings");
            }

            // Determine offer type based on user role
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user!);
            var offerType = roles.Contains("MaterialSupplier") ? OfferTypes.Material : OfferTypes.Labor;

            // Create offer
            var offer = new Offer
            {
                ListingId = listingId,
                UserId = userId,
                OfferType = offerType,
                EstimatedDays = estimatedDays,
                WarrantyMonths = warrantyMonths,
                Status = OfferStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                MaterialSource = "Package"
            };

            // Create line items
            var lineItems = new List<OfferLineItem>();
            decimal totalPrice = 0;

            for (int i = 0; i < itemIds.Length; i++)
            {
                if (string.IsNullOrEmpty(brands[i]) || string.IsNullOrEmpty(productNames[i]))
                    continue;

                var packageItemId = int.Parse(itemIds[i]);
                var packageItem = await _context.PackageItems.FindAsync(packageItemId);
                if (packageItem == null) continue;

                var lineTotal = quantities[i] * unitPrices[i];
                totalPrice += lineTotal;

                lineItems.Add(new OfferLineItem
                {
                    PackageItemId = packageItemId,
                    Brand = brands[i],
                    ProductName = productNames[i],
                    Quantity = quantities[i],
                    Unit = packageItem.Unit,
                    UnitPrice = unitPrices[i],
                    LineTotal = lineTotal
                });
            }

            offer.TotalOfferPrice = totalPrice;
            offer.LineItems = lineItems;

            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();


            TempData["Success"] = "Teklifiniz başarıyla gönderildi!";
            return RedirectToAction("MyOffers");
        }

        // GET: Offers/Create/5
        public IActionResult Create(int listingId)
        {
            // In a real app, retrieve listing details. Mocking for now as Listing model might not exist yet fully populated.
            // var listing = _context.Listings.Find(listingId); 
            // Mock data
            var calculation = new ListingCalculation
            {
                ListingId = listingId,
                RequiredPaint = 45.0, // Liters
                RequiredPrimer = 15.0,
                EstimatedLaborMin = 5000,
                EstimatedLaborMax = 7000,
                EstimatedMaterialMin = 3000,
                EstimatedMaterialMax = 5000
            };

            var adminMaterials = _context.AdminPriceReferences.Where(a => a.IsActive).ToList();
            var templates = _context.ServiceTemplates.Where(t => t.IsActive).ToList();

            var viewModel = new CreateOfferViewModel
            {
                ListingId = listingId,
                ListingTitle = "Sample Listing " + listingId,
                ListingArea = 120, // m2 mock
                Calculation = calculation,
                AdminMaterials = adminMaterials,
                Templates = templates
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOfferDto OfferData)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate and return view
                 var adminMaterials = _context.AdminPriceReferences.Where(a => a.IsActive).ToList();
                 var templates = _context.ServiceTemplates.Where(t => t.IsActive).ToList();
                 var viewModel = new CreateOfferViewModel
                 {
                     ListingId = OfferData.ListingId,
                     Calculation = new ListingCalculation(), // Re-fetch
                     AdminMaterials = adminMaterials,
                     Templates = templates,
                     OfferData = OfferData
                 };
                return View(viewModel);
            }

            // ... (Rest of existing Create POST logic until end of method)
            try
            {
                // Determine OfferType based on user role
                var currentUser = await _userManager.GetUserAsync(User);
                var offerType = OfferTypes.Material; // Default
                
                if (currentUser != null)
                {
                    var roles = await _userManager.GetRolesAsync(currentUser);
                    if (roles.Contains(Roles.LaborProvider))
                    {
                        offerType = OfferTypes.Labor;
                    }
                    else if (roles.Contains(Roles.MaterialSupplier))
                    {
                        offerType = OfferTypes.Material;
                    }
                }

                var offer = new Offer
                {
                    ListingId = OfferData.ListingId,
                    UserId = GetCurrentUserId(),
                    OfferType = offerType, // Auto-set based on user role
                    LaborCost = OfferData.LaborCost,
                    LaborCostType = OfferData.LaborCostType,
                    EstimatedDays = OfferData.EstimatedDays,
                    WarrantyMonths = OfferData.WarrantyMonths,
                    AdditionalServicesJson = OfferData.AdditionalServicesJson,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };

                // Deserialize materials
                var materialsStart = JsonSerializer.Deserialize<List<MaterialSelectionDto>>(OfferData.MaterialsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                decimal materialTotal = 0;
                bool hasAdmin = false;
                bool hasCustom = false;

                if (materialsStart != null)
                {
                    foreach (var item in materialsStart)
                    {
                        var mat = new OfferMaterial
                        {
                            MaterialName = item.Name,
                            Brand = item.Brand,
                            Quantity = item.Quantity,
                            Unit = item.Unit,
                            UnitPrice = item.UnitPrice,
                            TotalPrice = item.Quantity * item.UnitPrice,
                            Source = item.Source,
                            AdminPriceId = item.AdminPriceId
                        };
                        
                        // Security check for AdminStock
                        if (item.Source == "AdminStock" && item.AdminPriceId.HasValue)
                        {
                            var adminPrice = _context.AdminPriceReferences.Find(item.AdminPriceId.Value);
                            if (adminPrice != null)
                            {
                                // Validate price manipulation? 
                                // User plan says: "AdminStock election check price manipulation"
                                // We trust the ID from admin price, but the user might send modified unit price.
                                // Let's override the unit price with the server source of truth to be safe.
                                mat.UnitPrice = adminPrice.BasePrice; 
                                mat.TotalPrice = mat.Quantity * mat.UnitPrice;
                                hasAdmin = true;
                            }
                        }
                        else
                        {
                            hasCustom = true;
                        }

                        materialTotal += mat.TotalPrice;
                        offer.Materials.Add(mat);
                    }
                }

                offer.MaterialCostTotal = materialTotal;
                
                // Calculate Total Offer Price
                // Logic: Labor + Material + Extras
                decimal laborTotal = OfferData.LaborCostType == "PerM2" 
                    ? OfferData.LaborCost * 120 // Mock Area 120, need real listing area
                    : OfferData.LaborCost;

                // Extras
                decimal extrasTotal = 0; // Parse AdditionalServicesJson if needed

                offer.TotalOfferPrice = laborTotal + materialTotal + extrasTotal;

                offer.MaterialSource = (hasAdmin && hasCustom) ? "Mixed" : (hasAdmin ? "Admin" : "Custom");

                _context.Offers.Add(offer);
                await _context.SaveChangesAsync();

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error saving offer: " + ex.Message);
                var adminMaterials = _context.AdminPriceReferences.Where(a => a.IsActive).ToList();
                var templates = _context.ServiceTemplates.Where(t => t.IsActive).ToList();
                return View(new CreateOfferViewModel { OfferData = OfferData, AdminMaterials = adminMaterials, Templates = templates });
            }
        }

        // ... (Existing actions)

        [HttpGet]
        public IActionResult GetTemplateItems(int templateId)
        {
            var items = _context.ServiceTemplateItems
                .Include(i => i.Product)
                .Where(i => i.ServiceTemplateId == templateId)
                .Select(i => new {
                     name = i.Product.MaterialName,
                     brand = i.Product.Brand,
                     quantity = i.Quantity,
                     unit = i.Product.Unit,
                     unitPrice = i.Product.BasePrice,
                     source = "AdminStock",
                     adminPriceId = i.Product.Id,
                     note = i.DefaultNote
                })
                .ToList();
            return Json(items);
        }

        // ... (Existing GetAdminPrices, DownloadPdf, etc)
        public async Task<IActionResult> MyOffers()
        {
            // Şimdilik tüm teklifler - Identity yapılandırıldığında User.Identity.Name ile filtrelenecek
            var currentUserId = GetCurrentUserId();
            var offers = await _context.Offers
                .Include(o => o.Materials)
                .Include(o => o.Listing) // Added this line based on the provided snippet
                .Where(o => o.UserId == currentUserId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(); // Changed ToList() to ToListAsync()
            return View(offers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            
            if (offer == null) 
                return NotFound();
            
            // Sadece kendi teklifini iptal edebilir
            var currentUserId = GetCurrentUserId();
            // Ensure the supplier owns this offer
            if (offer.UserId != currentUserId)
            {    return Forbid();
            }
            
            // Sadece Pending durumundaki teklifler iptal edilebilir
            if (offer.Status != "Pending")
            {
                TempData["Error"] = "Only pending offers can be cancelled.";
                return RedirectToAction("MyOffers");
            }
            
            offer.Status = "Cancelled";
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Offer cancelled successfully.";
            return RedirectToAction("MyOffers");
        }

        public IActionResult Success()
        {
            return View();
        }

        // Helper: Basit kullanıcı ID'si (Identity olmadan)
        private string GetCurrentUserId()
        {
            // Identity yapılandırıldığında: return User.Identity?.Name ?? "Anonymous";
            // Şimdilik session bazlı pseudo ID
            var sessionId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = "User_" + Guid.NewGuid().ToString("N")[..8];
                HttpContext.Session.SetString("UserId", sessionId);
            }
            return sessionId;
        }

        public IActionResult Details(int id)
        {
            var offer = _context.Offers
                .Include(o => o.Materials)
                .FirstOrDefault(o => o.Id == id);
            
            if (offer == null) return NotFound();

            return View(offer);
        }

        [HttpGet]
        public IActionResult GetAdminPrices()
        {
             var prices = _context.AdminPriceReferences
                 .Where(p => p.IsActive)
                 .Select(p => new {
                     p.Id,
                     p.Category,
                     p.MaterialName,
                     p.Brand,
                     p.Quality,
                     p.Unit,
                     p.BasePrice,
                     Description = $"{p.MaterialName} - {p.Brand} ({p.Quality})"
                 })
                 .ToList();
             return Json(prices);
        }

        [HttpGet]
        public IActionResult DownloadPdf(int id)
        {
            var offer = _context.Offers
                .Include(o => o.Materials)
                .FirstOrDefault(o => o.Id == id);

            if (offer == null) return NotFound();

            var pdfGenerator = new Services.OfferPdfGenerator();
            var pdfBytes = pdfGenerator.GeneratePdf(offer);

            return File(pdfBytes, "application/pdf", $"Teklif_{offer.Id}.pdf");
        }
    }
}
