using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    /// <summary>
    /// Usta kataloğu - LaborProvider'ların sabit fiyat listesi
    /// </summary>
    public class LaborCatalog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // LaborProvider
        public virtual ApplicationUser User { get; set; }

        [Required]
        public int RecipeItemId { get; set; }
        public virtual RecipeItem RecipeItem { get; set; }

        [Required]
        [StringLength(200)]
        public string LaborType { get; set; } = string.Empty; // "Alçıpan Asma Tavan İşçiliği"

        [StringLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal FixedPricePerUnit { get; set; } // Birim fiyat (örn: 50₺/m²)

        public int EstimatedDuration { get; set; } = 0; // Tahmini süre (dakika)

        public int WarrantyMonths { get; set; } = 12; // Garanti süresi

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();
    }
}
