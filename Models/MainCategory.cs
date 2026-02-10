using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models
{
    /// <summary>
    /// Ana tadilat kategorileri (Mutfak, Banyo, Salon, Yatak Odası)
    /// </summary>
    public class MainCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // "Mutfak Tadilatı"

        [StringLength(50)]
        public string Icon { get; set; } = "bi-house"; // Bootstrap icon class

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        // Navigation properties
        public virtual ICollection<RecipeTemplate> RecipeTemplates { get; set; } = new List<RecipeTemplate>();
        public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
