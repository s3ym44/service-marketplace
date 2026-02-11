using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;
using ServiceMarketplace.Services;
using ServiceMarketplace.ViewModels;

namespace ServiceMarketplace.Controllers
{
    public class ListingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ListingCalculationService _calculationService;

        public ListingsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _calculationService = new ListingCalculationService();
        }

        // GET: Listings/Create
        [Authorize(Roles = "Customer")]
        public IActionResult Create()
        {
            return View(new CreateListingViewModel());
        }

        // GET: Listings/CreateWithPackage
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateWithPackage(int packageId)
        {
            var package = await _context.ServicePackages
                .Include(sp => sp.MainCategory)
                .Include(sp => sp.Items.OrderBy(i => i.DisplayOrder))
                .FirstOrDefaultAsync(sp => sp.Id == packageId);

            if (package == null)
            {
                TempData["Error"] = "Paket bulunamadı.";
                return RedirectToAction("Index", "ServicePackages");
            }

            ViewBag.Package = package;
            return View();
        }

        // POST: Listings/CreateWithPackage
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateWithPackage(int packageId, string title, string description, 
            string location, decimal width, decimal height, decimal depth, DateTime? startDate, DateTime? deadline)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var package = await _context.ServicePackages
                .Include(sp => sp.MainCategory)
                .FirstOrDefaultAsync(sp => sp.Id == packageId);
            if (package == null)
            {
                TempData["Error"] = "Paket bulunamadı.";
                return RedirectToAction("Index", "ServicePackages");
            }

            // Store dimensions as JSON
            var dimensions = System.Text.Json.JsonSerializer.Serialize(new
            {
                width,
                height,
                depth
            });

            // Calculate metrics
            var areaM2 = (width * height) / 10000;
            var lengthM = width / 100;
            var wallAreaM2 = ((width * height * 2) + (depth * height * 2)) / 10000;

            var calculatedMetrics = System.Text.Json.JsonSerializer.Serialize(new
            {
                areaM2,
                lengthM,
                wallAreaM2
            });

            var listing = new Listing
            {
                UserId = userId,
                ServicePackageId = packageId,
                MainCategoryId = package.MainCategoryId,
                Title = title,
                Description = description,
                Location = location,
                Dimensions = dimensions,
                CalculatedMetrics = calculatedMetrics,
                Area = areaM2,
                StartDate = startDate,
                Deadline = deadline,
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
                ServiceType = package.MainCategory.Name // For backward compatibility
            };

            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();

            TempData["Success"] = "İlanınız başarıyla oluşturuldu!";
            return RedirectToAction(nameof(MyListings));
        }

        // POST: Listings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(CreateListingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            // Calculate estimates
            var calc = _calculationService.Calculate(
                model.ServiceType, 
                model.Area, 
                model.RoomCount, 
                model.CeilingHeight);

            var listing = new Listing
            {
                UserId = userId,
                ServiceType = model.ServiceType,
                Title = model.Title,
                Description = model.Description,
                Area = model.Area,
                RoomCount = model.RoomCount,
                CeilingHeight = model.CeilingHeight,
                Location = model.Location,
                StartDate = model.StartDate,
                Deadline = model.Deadline,
                Budget = model.Budget,
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
                
                // Store estimates
                EstimatedMaterialMin = calc.MaterialCostMin,
                EstimatedMaterialMax = calc.MaterialCostMax,
                EstimatedLaborMin = calc.LaborCostMin,
                EstimatedLaborMax = calc.LaborCostMax,
                EstimatedDaysMin = calc.EstimatedDaysMin,
                EstimatedDaysMax = calc.EstimatedDaysMax
            };

            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your listing has been published!";
            return RedirectToAction(nameof(MyListings));
        }

        // GET: Listings/MyListings (Customer's own listings)
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyListings()
        {
            var userId = _userManager.GetUserId(User);

            var listings = await _context.Listings
                .Where(l => l.UserId == userId)
                .Include(l => l.Offers)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return View(listings);
        }

        // GET: Listings/OpenListings (For Suppliers to browse)
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> OpenListings(string? serviceType, string? location)
        {
            var query = _context.Listings
                .Where(l => l.Status == "Open")
                .Include(l => l.User)
                .Include(l => l.Offers)
                .AsQueryable();

            if (!string.IsNullOrEmpty(serviceType))
                query = query.Where(l => l.ServiceType == serviceType);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(l => l.Location.Contains(location));

            var listings = await query
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            ViewBag.ServiceTypeFilter = serviceType;
            ViewBag.LocationFilter = location;

            return View(listings);
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var listing = await _context.Listings
                .Include(l => l.User)
                .Include(l => l.Offers)
                .ThenInclude(o => o.Materials)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (listing == null) return NotFound();

            var viewModel = new ListingDetailViewModel
            {
                Id = listing.Id,
                ServiceType = listing.ServiceType,
                Title = listing.Title,
                Description = listing.Description,
                Area = listing.Area,
                RoomCount = listing.RoomCount,
                CeilingHeight = listing.CeilingHeight,
                Location = listing.Location,
                StartDate = listing.StartDate,
                Deadline = listing.Deadline,
                Budget = listing.Budget,
                Status = listing.Status,
                CreatedAt = listing.CreatedAt,
                
                CustomerName = listing.User?.FullName ?? "Unknown",
                CustomerPhone = listing.User?.PhoneNumber,
                
                EstimatedMaterialMin = listing.EstimatedMaterialMin,
                EstimatedMaterialMax = listing.EstimatedMaterialMax,
                EstimatedLaborMin = listing.EstimatedLaborMin,
                EstimatedLaborMax = listing.EstimatedLaborMax,
                EstimatedDaysMin = listing.EstimatedDaysMin,
                EstimatedDaysMax = listing.EstimatedDaysMax,
                
                OfferCount = listing.Offers.Count,
                LowestOffer = listing.Offers.Any() ? listing.Offers.Min(o => o.TotalOfferPrice) : null,
                HasAcceptedOffer = listing.Offers.Any(o => o.Status == "Accepted")
            };

            return View(viewModel);
        }

        // API endpoint for live calculation (used by JavaScript)
        [HttpPost]
        public IActionResult Calculate([FromBody] CalculationRequest request)
        {
            var result = _calculationService.Calculate(
                request.ServiceType,
                request.Area,
                request.RoomCount,
                request.CeilingHeight);

            return Json(result);
        }

        // POST: Listings/Close/5 (Customer closes their listing)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Close(int id)
        {
            var userId = _userManager.GetUserId(User);
            var listing = await _context.Listings.FindAsync(id);

            if (listing == null) return NotFound();
            if (listing.UserId != userId) return Forbid();

            listing.Status = "Closed";
            listing.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Listing closed successfully.";
            return RedirectToAction(nameof(MyListings));
        }
    }

    public class CalculationRequest
    {
        public string ServiceType { get; set; } = "Boya";
        public decimal Area { get; set; }
        public int RoomCount { get; set; } = 1;
        public decimal CeilingHeight { get; set; } = 2.8m;
    }
}
