using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.ViewModels
{
    public class BulkUpdateViewModel
    {
        public List<string> Categories { get; set; } = new();

        [Display(Name = "Category")]
        public string? SelectedCategory { get; set; } // null = all categories

        [Required]
        [Range(-99, 500, ErrorMessage = "Percentage must be between -99% and 500%")]
        [Display(Name = "Percentage Change (%)")]
        public decimal PercentageChange { get; set; }
    }

    public class RegionModifierViewModel
    {
        public List<string> Categories { get; set; } = new();

        [Display(Name = "Category")]
        public string? SelectedCategory { get; set; }

        [Required]
        [Range(0.1, 5.0, ErrorMessage = "Modifier must be between 0.1 and 5.0")]
        [Display(Name = "New Region Modifier")]
        public double NewModifier { get; set; } = 1.0;
    }
}
