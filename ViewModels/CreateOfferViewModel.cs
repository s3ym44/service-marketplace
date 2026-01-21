using ServiceMarketplace.Models;

namespace ServiceMarketplace.ViewModels
{
    public class CreateOfferViewModel
    {
        public int ListingId { get; set; }
        
        // Listing details for display
        public string ListingTitle { get; set; } // Simplified
        public double ListingArea { get; set; } // m2
        public ListingCalculation Calculation { get; set; }

        public List<AdminPriceReference> AdminMaterials { get; set; } = new List<AdminPriceReference>();
        public List<ServiceTemplate> Templates { get; set; } = new List<ServiceTemplate>();

        public CreateOfferDto OfferData { get; set; } = new CreateOfferDto();
    }
}
