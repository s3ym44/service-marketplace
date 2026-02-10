namespace ServiceMarketplace.Models
{
    /// <summary>
    /// ASP.NET Identity Role names
    /// </summary>
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string MaterialSupplier = "MaterialSupplier";
        public const string LaborProvider = "LaborProvider";
        
        // Convenience check for any supplier type
        public static bool IsSupplier(string role) => 
            role == MaterialSupplier || role == LaborProvider;
    }

    public static class OfferTypes
    {
        public const string Material = "Material";
        public const string Labor = "Labor";
    }

    public static class ItemTypes
    {
        public const string Material = "Material";
        public const string Labor = "Labor";
    }

    public static class OfferStatus
    {
        public const string Pending = "Pending";
        public const string Accepted = "Accepted";
        public const string Rejected = "Rejected";
        public const string Cancelled = "Cancelled";
    }
}
