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

        public decimal Quantity { get; set; } // Default quantity for this template

        public string DefaultNote { get; set; }
    }
}
