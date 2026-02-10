using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models
{
    public class ServiceTemplate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } // e.g. "Standard Kitchen Renovation"

        public string Description { get; set; }

        [StringLength(100)]
        public string RenovationType { get; set; } // "Mutfak", "Banyo", "Salon", "Yatak Odası"

        public bool IsActive { get; set; } = true;

        // Calculated properties
        public int TotalItems => Items?.Count ?? 0;

        public decimal EstimatedCost => Items?.Sum(i => (i.Quantity * i.Product?.BasePrice ?? 0)) ?? 0;

        public virtual ICollection<ServiceTemplateItem> Items { get; set; } = new List<ServiceTemplateItem>();
    }
}
