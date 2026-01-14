using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class AdminPriceReference
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; }
        public string MaterialName { get; set; }
        public string Brand { get; set; }
        public string Quality { get; set; } // Standard, Premium

        public decimal Quantity { get; set; }
        public string Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }

        public double RegionModifier { get; set; } = 1.0;

        public bool IsActive { get; set; } = true;
    }
}
