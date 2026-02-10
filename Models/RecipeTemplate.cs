using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    /// <summary>
    /// Reçete şablonu (örn: Mutfak Reçete - 158 kalem)
    /// </summary>
    public class RecipeTemplate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MainCategoryId { get; set; }
        public virtual MainCategory MainCategory { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // "Standart Mutfak Reçete"

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public int TotalItems { get; set; } // 158

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedBudgetMin { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedBudgetMax { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<RecipeItem> Items { get; set; } = new List<RecipeItem>();
        public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
