using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class AdminPriceReference
    {
        [Key]
        public int Id { get; set; }

        // Category string for backward compatibility/legacy, or we can rely on Relation.
        // For now, let's keep it but make it optional if we switch fully to relation.
        public string Category { get; set; } 
        
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category CategoryModel { get; set; }

        public string MaterialName { get; set; }
        public string? Description { get; set; } // Detailed description
        public string Brand { get; set; }
        public string Quality { get; set; } // Standard, Premium

        public bool IsLabor { get; set; } = false; // Distinguish Material vs Labor

        public decimal Quantity { get; set; } // Base quantity for unit price usually 1
        public string Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }

        public double RegionModifier { get; set; } = 1.0;

        public bool IsActive { get; set; } = true;
    }
}
