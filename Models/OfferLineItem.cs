using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class OfferLineItem
    {
        public int Id { get; set; }

        [Required]
        public int OfferId { get; set; }

        [Required]
        public int PackageItemId { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; } = string.Empty; // "Kelebek Mobilya"

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty; // "Modern Mutfak Dolabı"

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }

        // Navigation properties
        public virtual Offer Offer { get; set; } = null!;
        public virtual PackageItem PackageItem { get; set; } = null!;
    }
}
