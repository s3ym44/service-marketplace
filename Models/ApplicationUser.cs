using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class ApplicationUser : IdentityUser // Keep as string ID for compatibility
    {
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }
        
        [NotMapped]
        public string FullName
        {
            get
            {
                // Handle case where FirstName/LastName might not be set (migration not applied)
                var first = !string.IsNullOrEmpty(FirstName) ? FirstName : "";
                var last = !string.IsNullOrEmpty(LastName) ? LastName : "";
                var full = $"{first} {last}".Trim();
                return !string.IsNullOrEmpty(full) ? full : Email ?? UserName ?? "User";
            }
        }
        
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
