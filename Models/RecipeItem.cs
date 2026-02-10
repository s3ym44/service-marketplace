using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    /// <summary>
    /// Reçete kalemi (Alçıpan Levha, Elektrik Tesisatı İşçiliği vb.)
    /// </summary>
    public class RecipeItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RecipeTemplateId { get; set; }
        public virtual RecipeTemplate RecipeTemplate { get; set; }

        [Required]
        [StringLength(20)]
        public string ItemType { get; set; } = ItemTypes.Material; // "Material" or "Labor"

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty; // "Alçıpan İşleri", "Elektrik"

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // "Alçıpan Levha 12.5mm"

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty; // "m²", "adet", "metre"

        [Column(TypeName = "decimal(18,2)")]
        public decimal DefaultQuantity { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsRequired { get; set; } = true;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        public virtual ICollection<SupplierCatalog> SupplierCatalogItems { get; set; } = new List<SupplierCatalog>();
        public virtual ICollection<LaborCatalog> LaborCatalogItems { get; set; } = new List<LaborCatalog>();
        public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();

        // Computed properties
        [NotMapped]
        public bool IsMaterial => ItemType == ItemTypes.Material;

        [NotMapped]
        public bool IsLabor => ItemType == ItemTypes.Labor;
    }
}
