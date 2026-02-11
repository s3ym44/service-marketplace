using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models
{
    public class SupplierProduct
    {
        public int Id { get; set; }

        // Supplier who offers this product
        [Required]
        public string SupplierId { get; set; } = string.Empty;
        public ApplicationUser Supplier { get; set; } = null!;

        // Optional: Link to PackageItem (for package-based products)
        public int? PackageItemId { get; set; }
        public PackageItem? PackageItem { get; set; }

        // Product Details
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty; // Dolap, Tezgah, Seramik

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ModelCode { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // Pricing
        [Required]
        [Range(0.01, 1000000)]
        public decimal UnitPrice { get; set; }

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; } = string.Empty; // m², m, adet, set, gün

        // Stock info (optional)
        public bool InStock { get; set; } = true;

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }

        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
