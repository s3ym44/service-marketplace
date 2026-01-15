using Microsoft.AspNetCore.Identity;

namespace ServiceMarketplace.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? CompanyName { get; set; } // For suppliers
        public string? PhoneNumber2 { get; set; } // Secondary phone
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
