using ServiceMarketplace.Models;

namespace ServiceMarketplace.ViewModels
{
    public class CategoryGroup
    {
        public string GroupType { get; set; } = string.Empty;
        public string? GroupTitle { get; set; }
        public List<MainCategory> Categories { get; set; } = new();
    }

    public class HomeViewModel
    {
        public List<CategoryGroup> Groups { get; set; } = new();
        public Dictionary<int, int> PackageCounts { get; set; } = new();
        public string SearchQuery { get; set; } = "";
        public string ActiveFilter { get; set; } = "all";
        public int TotalPackages { get; set; }
        public int TotalListings { get; set; }
        public int TotalSuppliers { get; set; }
    }
}
