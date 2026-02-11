using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models
{
    public class ServicePackage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty; // "XRANA23"

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // "Komple Mutfak Tadilatı"

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int MainCategoryId { get; set; }

        [StringLength(500)]
        public string? ThumbnailImage { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual MainCategory MainCategory { get; set; } = null!;
        public virtual ICollection<PackageItem> Items { get; set; } = new List<PackageItem>();
        public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
