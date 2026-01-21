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
        
        // Navigation property for related products (AdminPriceReferences)
        public virtual ICollection<AdminPriceReference> Products { get; set; } = new List<AdminPriceReference>();
    }
}
