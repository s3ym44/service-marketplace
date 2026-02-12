using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;
using ServiceMarketplace.ViewModels;

namespace ServiceMarketplace.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customer/Offers/5 (listingId)
        public async Task<IActionResult> Offers(int listingId)
        {
            // Verify this listing belongs to the current customer
            var userId = _userManager.GetUserId(User);
            var listing = await _context.Listings
                .Include(l => l.ServicePackage)
                    .ThenInclude(sp => sp.Items.OrderBy(i => i.DisplayOrder))
                .FirstOrDefaultAsync(l => l.Id == listingId);

            if (listing == null) return NotFound();
            if (listing.UserId != userId) return Forbid();

            var offers = await _context.Offers
                .Include(o => o.User)
                .Include(o => o.Materials)
                .Include(o => o.LineItems)
                    .ThenInclude(li => li.PackageItem)
                .Where(o => o.ListingId == listingId)
                .OrderBy(o => o.TotalOfferPrice)
                .ToListAsync();

            var viewModel = new CompareOffersViewModel
            {
                ListingId = listingId,
                ListingTitle = listing.Title,
                ListingArea = listing.Area,
                Listing = listing,
                Offers = offers
            };

            // Calculate best values for highlighting
            if (offers.Any())
            {
                viewModel.LowestTotalPrice = offers.Min(o => o.TotalOfferPrice);
                viewModel.LowestLaborCost = offers.Where(o => o.LaborCost > 0).Any()
                    ? offers.Where(o => o.LaborCost > 0).Min(o => o.LaborCost) : 0;
                viewModel.LowestMaterialCost = offers.Where(o => o.MaterialCostTotal > 0).Any()
                    ? offers.Where(o => o.MaterialCostTotal > 0).Min(o => o.MaterialCostTotal) : 0;
                viewModel.ShortestDays = offers.Where(o => o.EstimatedDays > 0).Any()
                    ? offers.Where(o => o.EstimatedDays > 0).Min(o => o.EstimatedDays) : 0;
                viewModel.LongestWarranty = offers.Where(o => o.WarrantyMonths > 0).Any()
                    ? offers.Max(o => o.WarrantyMonths) : 0;

                // Build per-line-item best prices (for Mix & Match highlighting)
                if (listing.ServicePackage != null)
                {
                    foreach (var pkgItem in listing.ServicePackage.Items)
                    {
                        var lineItemPrices = offers
                            .SelectMany(o => o.LineItems)
                            .Where(li => li.PackageItemId == pkgItem.Id && li.LineTotal > 0)
                            .ToList();

                        if (lineItemPrices.Any())
                        {
                            viewModel.BestLineItemPrices[pkgItem.Id] = lineItemPrices.Min(li => li.LineTotal);
                        }
                    }
                }
            }

            return View(viewModel);
        }

        // POST: Customer/Accept/5 (offerId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int offerId)
        {
            var userId = _userManager.GetUserId(User);
            var offer = await _context.Offers
                .Include(o => o.Listing)
                .FirstOrDefaultAsync(o => o.Id == offerId);

            if (offer == null) return NotFound();
            if (offer.Listing?.UserId != userId) return Forbid();

            if (offer.Status != "Pending")
            {
                TempData["Error"] = "Bu teklif artık kabul edilemez durumda.";
                return RedirectToAction("Offers", new { listingId = offer.ListingId });
            }

            // Accept this offer
            offer.Status = "Accepted";

            // Update listing status
            if (offer.Listing != null)
            {
                offer.Listing.Status = "InProgress";
                offer.Listing.UpdatedAt = DateTime.UtcNow;
            }

            // Reject all other pending offers for this listing
            var otherOffers = await _context.Offers
                .Where(o => o.ListingId == offer.ListingId && o.Id != offerId && o.Status == "Pending")
                .ToListAsync();

            foreach (var other in otherOffers)
            {
                other.Status = "Rejected";
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Teklif kabul edildi! {otherOffers.Count} diğer teklif reddedildi.";
            return RedirectToAction("Offers", new { listingId = offer.ListingId });
        }

        // GET: Customer/OfferDetails/5 (offerId)
        public async Task<IActionResult> OfferDetails(int offerId)
        {
            var userId = _userManager.GetUserId(User);
            var offer = await _context.Offers
                .Include(o => o.User)
                .Include(o => o.Materials)
                .Include(o => o.LineItems)
                    .ThenInclude(li => li.PackageItem)
                .Include(o => o.Listing)
                .FirstOrDefaultAsync(o => o.Id == offerId);

            if (offer == null) return NotFound();
            if (offer.Listing?.UserId != userId) return Forbid();

            return View(offer);
        }
    }
}
