using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models
{
    public class PackageItem
    {
        public int Id { get; set; }

        [Required]
        public int ServicePackageId { get; set; }

        // === 3 KATMANLI HİYERARŞİ ===
        [Required]
        [MaxLength(50)]
        public string MainCategory { get; set; } = string.Empty; // "Zemin", "Duvar", "Tavan", "Mobilya"

        [MaxLength(50)]
        public string? SubCategory { get; set; } // "Hafriyat", "Tesisat", "Şap", "Döşeme"

        [Required]
        [MaxLength(30)]
        public string ItemType { get; set; } = string.Empty; // "İşçilik", "Ana Ürün", "Yardımcı Malzeme", "Alternatif Ürün"

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty; // "Zemin Kırım ve Söküm"

        [MaxLength(20)]
        public string Unit { get; set; } = string.Empty; // "m²", "m", "adet", "kg"

        [MaxLength(100)]
        public string? ConsumptionFormula { get; set; } // "1 m² / alan", "3-5 adet / m²"

        public int DisplayOrder { get; set; }

        public bool IsRequired { get; set; } = true;

        // Navigation properties
        public virtual ServicePackage ServicePackage { get; set; } = null!;
        public virtual ICollection<OfferLineItem> OfferLineItems { get; set; } = new List<OfferLineItem>();
        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
    }
}
