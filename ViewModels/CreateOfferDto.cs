using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.ViewModels
{
    public class CreateOfferDto
    {
        public int ListingId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal LaborCost { get; set; }

        [Required]
        public string LaborCostType { get; set; } = "Fixed"; // Fixed, PerM2

        public int EstimatedDays { get; set; }
        
        [Range(0, 240)]
        public int WarrantyMonths { get; set; }

        // JSON string to receive from frontend
        public string MaterialsJson { get; set; } = "[]";
        
        // JSON string for additional services
        public string AdditionalServicesJson { get; set; } = "[]";
    }

    public class MaterialSelectionDto
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public string Source { get; set; } // AdminStock, Custom
        public int? AdminPriceId { get; set; }
    }
}
