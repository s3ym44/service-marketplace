using System.ComponentModel.DataAnnotations;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.ViewModels
{
    public class CompareOffersViewModel
    {
        public int ListingId { get; set; }
        public string ListingTitle { get; set; } = string.Empty;
        public decimal ListingArea { get; set; }

        // Full listing with ServicePackage + Items
        public Listing? Listing { get; set; }

        public List<Offer> Offers { get; set; } = new();
        
        // Best values for highlighting
        public decimal LowestTotalPrice { get; set; }
        public decimal LowestLaborCost { get; set; }
        public decimal LowestMaterialCost { get; set; }
        public int ShortestDays { get; set; }
        public int LongestWarranty { get; set; }

        // Per-line-item best prices: PackageItemId → lowest LineTotal
        public Dictionary<int, decimal> BestLineItemPrices { get; set; } = new();

        // Computed
        public bool HasAcceptedOffer => Offers.Any(o => o.Status == "Accepted");
        public bool IsPackageBased => Listing?.ServicePackageId != null;
        public List<PackageItem> PackageItems => Listing?.ServicePackage?.Items?.OrderBy(i => i.DisplayOrder).ToList() ?? new();
    }
}
