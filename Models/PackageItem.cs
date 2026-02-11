using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models
{
    public class PackageItem
    {
        public int Id { get; set; }

        [Required]
        public int ServicePackageId { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty; // "Dolap", "Tezgah", "Seramik"

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // "Alt + Üst Mutfak Dolapları"

        [Required]
        [StringLength(50)]
        public string ItemType { get; set; } = string.Empty; // "Material" / "Labor"

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty; // "m²", "metre", "adet"

        public int DisplayOrder { get; set; }

        public bool IsRequired { get; set; } = true;

        // Navigation properties
        public virtual ServicePackage ServicePackage { get; set; } = null!;
        public virtual ICollection<OfferLineItem> OfferLineItems { get; set; } = new List<OfferLineItem>();
    }
}
