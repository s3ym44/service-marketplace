using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        public int? ListingId { get; set; } // Nullable for existing data
        public string SupplierId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LaborCost { get; set; }

        [Required]
        public string LaborCostType { get; set; } // "Fixed" or "PerM2"

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaterialCostTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOfferPrice { get; set; }

        public int EstimatedDays { get; set; }
        public int WarrantyMonths { get; set; }

        public string MaterialSource { get; set; } // "Admin", "Custom", "Mixed"

        public string AdditionalServicesJson { get; set; } // Stored as JSON

        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Listing? Listing { get; set; }
        public List<OfferMaterial> Materials { get; set; } = new List<OfferMaterial>();
    }
}
