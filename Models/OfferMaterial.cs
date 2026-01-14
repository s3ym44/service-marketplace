using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class OfferMaterial
    {
        [Key]
        public int Id { get; set; }

        public int OfferId { get; set; }
        [ForeignKey("OfferId")]
        public Offer Offer { get; set; }

        [Required]
        public string MaterialName { get; set; }

        public string Brand { get; set; }

        public decimal Quantity { get; set; }

        public string Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public string Source { get; set; } // "AdminStock", "Custom"

        public int? AdminPriceId { get; set; }
    }
}
