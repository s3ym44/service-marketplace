using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class ApplicationUser : IdentityUser // Keep as string ID for compatibility
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        
        // Supplier & LaborProvider fields
        public string? CompanyName { get; set; } // For MaterialSupplier and LaborProvider
        public string? TaxNumber { get; set; } // For MaterialSupplier
        public string? LicenseNumber { get; set; } // For LaborProvider
        public string? Specialties { get; set; } // For LaborProvider (JSON array of specialties)
        
        public bool IsVerified { get; set; } = false; // Admin approval status
        
        public string? PhoneNumber2 { get; set; } // Secondary phone
        public string? Address { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
