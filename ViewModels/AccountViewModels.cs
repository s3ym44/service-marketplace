using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a role")]
        [Display(Name = "Register as")]
        public string Role { get; set; } = "Customer"; // Customer, MaterialSupplier, LaborProvider

        // For MaterialSupplier and LaborProvider
        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }
        
        // For MaterialSupplier
        [Display(Name = "Tax Number")]
        public string? TaxNumber { get; set; }
        
        // For LaborProvider
        [Display(Name = "License Number")]
        public string? LicenseNumber { get; set; }
        
        [Display(Name = "Specialties (comma separated)")]
        public string? Specialties { get; set; } // e.g., "Alçıpan, Elektrik, Boya"

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
