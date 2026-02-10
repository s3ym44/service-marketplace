using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    /// <summary>
    /// Teklif kalemi - Reçete itemlerine karşılık gelen teklif detayı
    /// </summary>
    public class OfferItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; }

        [Required]
        public int RecipeItemId { get; set; }
        public virtual RecipeItem RecipeItem { get; set; }

        // Catalog reference (optional - if using catalog prices)
        public int? SupplierCatalogId { get; set; }
        public virtual SupplierCatalog? SupplierCatalog { get; set; }

        public int? LaborCatalogId { get; set; }
        public virtual LaborCatalog? LaborCatalog { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } // Quantity * UnitPrice

        public bool IsManualPrice { get; set; } = false; // Elle mi girildi, catalog'dan mı?

        [StringLength(500)]
        public string? Notes { get; set; }

        // Computed
        [NotMapped]
        public bool IsFromCatalog => SupplierCatalogId.HasValue || LaborCatalogId.HasValue;
    }
}
