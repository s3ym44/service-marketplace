using System.ComponentModel.DataAnnotations;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.ViewModels
{
    public class CompareOffersViewModel
    {
        public int ListingId { get; set; }
        public string ListingTitle { get; set; } = string.Empty;
        public decimal ListingArea { get; set; }
        
        public List<Offer> Offers { get; set; } = new();
        
        // Best values for highlighting
        public decimal LowestTotalPrice { get; set; }
        public decimal LowestLaborCost { get; set; }
        public decimal LowestMaterialCost { get; set; }
        public int ShortestDays { get; set; }
        public int LongestWarranty { get; set; }
        
        public bool HasAcceptedOffer => Offers.Any(o => o.Status == "Accepted");
    }
}
