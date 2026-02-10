using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class ServiceTemplateItem
    {
        public int Id { get; set; }

        public int ServiceTemplateId { get; set; }
        [ForeignKey("ServiceTemplateId")]
        public virtual ServiceTemplate ServiceTemplate { get; set; }

        public int AdminPriceReferenceId { get; set; }
        [ForeignKey("AdminPriceReferenceId")]
        public virtual AdminPriceReference Product { get; set; }

        [StringLength(100)]
        public string SubCategory { get; set; } // "Alçıpan", "Elektrik", "Sıhhi Tesisat", "Boya", "Mobilya", etc.

        // YENİ: Item tipi - Material veya Labor
        [Required]
        public string ItemType { get; set; } = "Material"; // "Material" or "Labor"

        public int DisplayOrder { get; set; } // Order within the template

        public decimal Quantity { get; set; } // Default quantity for this template

        public string Notes { get; set; } // Detailed notes for this item

        public string DefaultNote { get; set; }
    }
}
