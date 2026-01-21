using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;
using ServiceMarketplace.ViewModels;
using System.Text.Json;

namespace ServiceMarketplace.Controllers
{
    [Authorize(Roles = "Supplier")]
    public class OffersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OffersController(ApplicationDbContext context)
        {
            _context = context;
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
                var offer = new Offer
                {
                    ListingId = OfferData.ListingId,
                    SupplierId = GetCurrentUserId(),
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
        public IActionResult MyOffers()
        {
            // Şimdilik tüm teklifler - Identity yapılandırıldığında User.Identity.Name ile filtrelenecek
            var currentUserId = GetCurrentUserId();
            var offers = _context.Offers
                .Include(o => o.Materials)
                .Where(o => o.SupplierId == currentUserId)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
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
            if (offer.SupplierId != currentUserId)
                return Forbid();
            
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
