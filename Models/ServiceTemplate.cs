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

        public bool IsActive { get; set; } = true;

        public virtual ICollection<ServiceTemplateItem> Items { get; set; } = new List<ServiceTemplateItem>();
    }
}
