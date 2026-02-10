using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        // Parent category support for hierarchical categories
        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        
        // Navigation property for related products (AdminPriceReferences)
        public virtual ICollection<AdminPriceReference> Products { get; set; } = new List<AdminPriceReference>();
        
        // Child categories
        public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();
    }
}
