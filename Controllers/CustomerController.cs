using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.ViewModels;

namespace ServiceMarketplace.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer/Offers/5 (listingId)
        public async Task<IActionResult> Offers(int listingId)
        {
            var offers = await _context.Offers
                .Include(o => o.Materials)
                .Where(o => o.ListingId == listingId)
                .OrderBy(o => o.TotalOfferPrice)
                .ToListAsync();

            if (!offers.Any())
            {
                ViewBag.Message = "No offers received for this listing yet.";
            }

            var viewModel = new CompareOffersViewModel
            {
                ListingId = listingId,
                ListingTitle = $"Listing #{listingId}", // Mock - replace with real listing
                ListingArea = 120, // Mock
                Offers = offers
            };

            // Calculate best values for highlighting
            if (offers.Any())
            {
                viewModel.LowestTotalPrice = offers.Min(o => o.TotalOfferPrice);
                viewModel.LowestLaborCost = offers.Min(o => o.LaborCost);
                viewModel.LowestMaterialCost = offers.Min(o => o.MaterialCostTotal);
                viewModel.ShortestDays = offers.Where(o => o.EstimatedDays > 0).Any() 
                    ? offers.Where(o => o.EstimatedDays > 0).Min(o => o.EstimatedDays) 
                    : 0;
                viewModel.LongestWarranty = offers.Max(o => o.WarrantyMonths);
            }

            return View(viewModel);
        }

        // POST: Customer/Accept/5 (offerId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int offerId)
        {
            var offer = await _context.Offers.FindAsync(offerId);
            if (offer == null) return NotFound();

            // Only pending offers can be accepted
            if (offer.Status != "Pending")
            {
                TempData["Error"] = "This offer is no longer available for acceptance.";
                return RedirectToAction("Offers", new { listingId = offer.ListingId });
            }

            // Accept this offer
            offer.Status = "Accepted";

            // Reject all other pending offers for this listing
            var otherOffers = await _context.Offers
                .Where(o => o.ListingId == offer.ListingId && o.Id != offerId && o.Status == "Pending")
                .ToListAsync();

            foreach (var other in otherOffers)
            {
                other.Status = "Rejected";
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Offer #{offerId} has been accepted! {otherOffers.Count} other offer(s) were rejected.";
            return RedirectToAction("Offers", new { listingId = offer.ListingId });
        }

        // GET: Customer/OfferDetails/5 (offerId) - Expanded view
        public async Task<IActionResult> OfferDetails(int offerId)
        {
            var offer = await _context.Offers
                .Include(o => o.Materials)
                .FirstOrDefaultAsync(o => o.Id == offerId);

            if (offer == null) return NotFound();

            return View(offer);
        }
    }
}
