using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class Listing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // Customer who created
        
        // YENİ: Ana kategori ve reçete ilişkisi (nullable for backward compatibility)
        public int? MainCategoryId { get; set; }
        public virtual MainCategory? MainCategory { get; set; }
        
        public int? RecipeTemplateId { get; set; }
        public virtual RecipeTemplate? RecipeTemplate { get; set; }
        
        // YENİ: Paket sistemi desteği (Phase 1)
        public int? ServicePackageId { get; set; }
        public virtual ServicePackage? ServicePackage { get; set; }
        
        // Customer dimensions JSON: {"width": 320, "height": 420, "depth": 260}
        [StringLength(1000)]
        public string? Dimensions { get; set; }
        
        // Calculated metrics JSON: {"dolapAlani": 8.3, "tezgahUzunlugu": 3.2}
        [StringLength(2000)]
        public string? CalculatedMetrics { get; set; }
        
        // DEPRECATED: Artık RecipeTemplate kullanılacak
        [Obsolete("Use RecipeTemplateId instead")]
        public int? ServiceTemplateId { get; set; }
        public virtual ServiceTemplate? Template { get; set; }
        
        // DEPRECATED: Artık MainCategory.Name kullanılacak
        [Obsolete("Use MainCategory.Name instead")]
        public string? RenovationType { get; set; } // "Mutfak", "Banyo", "Salon", "Yatak Odası"
        
        [Required]
        public string ServiceType { get; set; } = string.Empty; // Boya, Seramik, Alçıpan
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Area { get; set; } // m²
        
        public int RoomCount { get; set; } = 1;
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal CeilingHeight { get; set; } = 2.8m; // meters
        
        [StringLength(500)]
        public string Location { get; set; } = string.Empty;
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? Deadline { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Budget { get; set; }
        
        [Required]
        public string Status { get; set; } = "Open"; // Open, InProgress, Closed, Cancelled
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }

        // Calculated estimates (stored for reference)
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedMaterialMin { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedMaterialMax { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedLaborMin { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedLaborMax { get; set; }
        
        public int EstimatedDaysMin { get; set; }
        public int EstimatedDaysMax { get; set; }

        // Navigation properties
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
