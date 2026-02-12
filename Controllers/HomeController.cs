using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;
using ServiceMarketplace.ViewModels;

namespace ServiceMarketplace.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index(string? search = null, string? filter = null)
    {
        var query = _context.MainCategories
            .Where(c => c.IsActive)
            .AsQueryable();

        // Arama
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c =>
                c.Name.Contains(search) ||
                c.Description.Contains(search));
        }

        // Filtre
        if (!string.IsNullOrWhiteSpace(filter) && filter != "all")
        {
            query = query.Where(c => c.GroupType == filter);
        }

        var categories = await query
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        // Grupla — anonim tip yerine CategoryGroup kullan
        var groups = categories
            .GroupBy(c => new { c.GroupType, c.GroupTitle })
            .OrderBy(g => g.Key.GroupType == "IcMekan" ? 0 : g.Key.GroupType == "DisMekan" ? 1 : 2)
            .Select(g => new CategoryGroup
            {
                GroupType = g.Key.GroupType,
                GroupTitle = g.Key.GroupTitle,
                Categories = g.ToList()
            })
            .ToList();

        // Her kategorinin paket sayısını al
        var packageCounts = await _context.ServicePackages
            .Where(sp => sp.IsActive)
            .GroupBy(sp => sp.MainCategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Count);

        // İstatistikler
        var totalPackages = await _context.ServicePackages.CountAsync(sp => sp.IsActive);
        var totalListings = await _context.Listings.CountAsync();

        // Tedarikçi sayısı: Identity role tablosundan çek
        var supplierRoleIds = await _context.Roles
            .Where(r => r.Name == "MaterialSupplier" || r.Name == "LaborProvider")
            .Select(r => r.Id)
            .ToListAsync();
        var totalSuppliers = await _context.UserRoles
            .Where(ur => supplierRoleIds.Contains(ur.RoleId))
            .Select(ur => ur.UserId)
            .Distinct()
            .CountAsync();

        var viewModel = new HomeViewModel
        {
            Groups = groups,
            PackageCounts = packageCounts,
            SearchQuery = search ?? "",
            ActiveFilter = filter ?? "all",
            TotalPackages = totalPackages,
            TotalListings = totalListings,
            TotalSuppliers = totalSuppliers
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
