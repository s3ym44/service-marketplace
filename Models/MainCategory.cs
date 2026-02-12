using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models
{
    /// <summary>
    /// Ana tadilat kategorileri (Mutfak, Banyo, Salon, Yatak Odası, vb.)
    /// </summary>
    public class MainCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // "Mutfak Tadilat"

        [StringLength(50)]
        public string Icon { get; set; } = "bi-house"; // Emoji veya icon class: "🍳"

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; } // Kategori görseli URL

        [StringLength(50)]
        public string GroupType { get; set; } = "IcMekan"; // "IcMekan", "DisMekan", "Ticari"

        [StringLength(50)]
        public string? GroupTitle { get; set; } // "İç Mekan Tadilatları"

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        // Navigation properties
        public virtual ICollection<ServicePackage> ServicePackages { get; set; } = new List<ServicePackage>();
        public virtual ICollection<RecipeTemplate> RecipeTemplates { get; set; } = new List<RecipeTemplate>();
        public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
