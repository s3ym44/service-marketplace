using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        public int? ListingId { get; set; } // Nullable for existing data
        public virtual Listing? Listing { get; set; }

        // YENİ: Teklif türü - Material veya Labor
        [Required]
        public string OfferType { get; set; } = "Material"; // "Material" or "Labor"

        // YENİ: Kullanıcı ilişkisi (Supplier veya LaborProvider)
        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; }

        // Malzeme maliyeti
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaterialCostTotal { get; set; }

        // İşçilik maliyeti
        [Column(TypeName = "decimal(18,2)")]
        public decimal LaborCost { get; set; }

        [Required]
        public string LaborCostType { get; set; } = "Fixed"; // "Fixed" or "PerM2"

        // Toplam teklif fiyatı
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOfferPrice { get; set; }

        public int EstimatedDays { get; set; }
        public int WarrantyMonths { get; set; }

        public string MaterialSource { get; set; } = "Custom"; // "Admin", "Custom", "Mixed"

        public string AdditionalServicesJson { get; set; } = "[]"; // Stored as JSON

        public string Status { get; set; } = OfferStatus.Pending; // Pending, Accepted, Rejected

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();
        
        // YENİ: Line items for package-based offers (Phase 1)
        public virtual ICollection<OfferLineItem> LineItems { get; set; } = new List<OfferLineItem>();
        
        // DEPRECATED: Will be replaced by OfferItems
        [Obsolete("Use OfferItems instead")]
        public virtual List<OfferMaterial> Materials { get; set; } = new List<OfferMaterial>();

        // Computed properties
        [NotMapped]
        public bool IsMaterialOffer => OfferType == OfferTypes.Material;

        [NotMapped]
        public bool IsLaborOffer => OfferType == OfferTypes.Labor;
        
        [NotMapped]
        public decimal CalculatedTotal => OfferItems?.Sum(i => i.TotalPrice) ?? TotalOfferPrice;
    }
}
