using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    /// <summary>
    /// Satıcı kataloğu - MaterialSupplier'ların sabit fiyat listesi
    /// </summary>
    public class SupplierCatalog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // MaterialSupplier
        public virtual ApplicationUser User { get; set; }

        [Required]
        public int RecipeItemId { get; set; }
        public virtual RecipeItem RecipeItem { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty; // "Marshall Alçıpan Levha 12.5mm"

        [StringLength(100)]
        public string? Brand { get; set; } // "Marshall", "Knauf"

        [StringLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal FixedPrice { get; set; } // Sabit fiyat (e-ticaret gibi)

        [StringLength(20)]
        public string? SKU { get; set; } // Stok kodu

        public int StockQuantity { get; set; } = 0; // İsteğe bağlı stok takibi

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();
    }
}
