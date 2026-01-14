using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.ViewModels
{
    public class CreateListingViewModel
    {
        [Required(ErrorMessage = "Service type is required")]
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; } = "Boya";

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 5)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Area is required")]
        [Range(1, 10000, ErrorMessage = "Area must be between 1 and 10000 m²")]
        [Display(Name = "Area (m²)")]
        public decimal Area { get; set; }

        [Required]
        [Range(1, 50)]
        [Display(Name = "Room Count")]
        public int RoomCount { get; set; } = 1;

        [Required]
        [Range(2.0, 6.0)]
        [Display(Name = "Ceiling Height (m)")]
        public decimal CeilingHeight { get; set; } = 2.8m;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(500)]
        [Display(Name = "Location")]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Preferred Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Deadline")]
        [DataType(DataType.Date)]
        public DateTime? Deadline { get; set; }

        [Display(Name = "Budget (optional)")]
        [Range(0, 10000000)]
        public decimal? Budget { get; set; }
    }

    public class ListingDetailViewModel
    {
        public int Id { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Area { get; set; }
        public int RoomCount { get; set; }
        public decimal CeilingHeight { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? Deadline { get; set; }
        public decimal? Budget { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
        // Customer info
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        
        // Estimates
        public decimal EstimatedMaterialMin { get; set; }
        public decimal EstimatedMaterialMax { get; set; }
        public decimal EstimatedLaborMin { get; set; }
        public decimal EstimatedLaborMax { get; set; }
        public decimal EstimatedTotalMin => EstimatedMaterialMin + EstimatedLaborMin;
        public decimal EstimatedTotalMax => EstimatedMaterialMax + EstimatedLaborMax;
        public int EstimatedDaysMin { get; set; }
        public int EstimatedDaysMax { get; set; }
        
        // Offers
        public int OfferCount { get; set; }
        public decimal? LowestOffer { get; set; }
        public bool HasAcceptedOffer { get; set; }
    }
}
