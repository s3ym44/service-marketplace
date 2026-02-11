using ServiceMarketplace.Data;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.Services
{
    /// <summary>
    /// Ana kategorileri ve reçete şablonlarını seed eden service
    /// </summary>
    public static class CatalogSeeder
    {
        public static async Task SeedMainCategoriesAsync(ApplicationDbContext context)
        {
            if (context.MainCategories.Any())
            {
                Console.WriteLine("MainCategories already seeded.");
                return;
            }

            var categories = new List<MainCategory>
            {
                new MainCategory
                {
                    Id = 1,
                    Name = "Mutfak Tadilatı",
                    Icon = "bi-egg-fried",
                    Description = "Mutfak dolabı, tezgah, seramik ve kapsamlı mutfak yenileme işleri",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new MainCategory
                {
                    Id = 2,
                    Name = "Banyo Tadilatı",
                    Icon = "bi-droplet",
                    Description = "Banyo seramiği, tesisat, armatür ve kapsamlı banyo yenileme işleri",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new MainCategory
                {
                    Id = 3,
                    Name = "Salon Tadilatı",
                    Icon = "bi-tv",
                    Description = "Salon boya, parke, alçıpan ve dekoratif tadilat işleri",
                    DisplayOrder = 3,
                    IsActive = true
                },
                new MainCategory
                {
                    Id = 4,
                    Name = "Yatak Odası Tadilatı",
                    Icon = "bi-moon-stars",
                    Description = "Yatak odası boya, dolap ve genel tadilat işleri",
                    DisplayOrder = 4,
                    IsActive = true
                }
            };

            await context.MainCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {categories.Count} MainCategory seeded successfully!");
        }

        public static async Task SeedRecipeTemplatesAsync(ApplicationDbContext context)
        {
            if (context.RecipeTemplates.Any())
            {
                Console.WriteLine("RecipeTemplates already seeded.");
                return;
            }

            var templates = new List<RecipeTemplate>
            {
                new RecipeTemplate
                {
                    Id = 1,
                    MainCategoryId = 1, // Mutfak
                    Name = "Standart Mutfak Reçete",
                    Description = "Kapsamlı mutfak tadilatı için standart reçete (dolap, tezgah, seramik, elektrik)",
                    TotalItems = 158,
                    EstimatedBudgetMin = 50000,
                    EstimatedBudgetMax = 120000,
                    IsActive = true
                },
                new RecipeTemplate
                {
                    Id = 2,
                    MainCategoryId = 2, // Banyo
                    Name = "Standart Banyo Reçete",
                    Description = "Kapsamlı banyo tadilatı için standart reçete (seramik, tesisat, armatür)",
                    TotalItems = 95,
                    EstimatedBudgetMin = 30000,
                    EstimatedBudgetMax = 75000,
                    IsActive = true
                },
                new RecipeTemplate
                {
                    Id = 3,
                    MainCategoryId = 3, // Salon
                    Name = "Standart Salon Reçete",
                    Description = "Kapsamlı salon tadilatı için standart reçete (boya, parke, alçıpan)",
                    TotalItems = 72,
                    EstimatedBudgetMin = 20000,
                    EstimatedBudgetMax = 60000,
                    IsActive = true
                },
                new RecipeTemplate
                {
                    Id = 4,
                    MainCategoryId = 4, // Yatak Odası
                    Name = "Standart Yatak Odası Reçete",
                    Description = "Kapsamlı yatak odası tadilatı için standart reçete (boya, dolap, zemin)",
                    TotalItems = 55,
                    EstimatedBudgetMin = 15000,
                    EstimatedBudgetMax = 45000,
                    IsActive = true
                }
            };

            await context.RecipeTemplates.AddRangeAsync(templates);
            await context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {templates.Count} RecipeTemplate seeded successfully!");
        }

        public static async Task SeedRecipeItemsAsync(ApplicationDbContext context)
        {
            if (context.RecipeItems.Any())
            {
                Console.WriteLine("RecipeItems already seeded.");
                return;
            }

            var items = new List<RecipeItem>();

            // Mutfak Reçete Items (Template ID = 1)
            items.AddRange(new[]
            {
                // Malzemeler
                new RecipeItem
                {
                    RecipeTemplateId = 1,
                    ItemType = ItemTypes.Material,
                    Category = "Dolap İşleri",
                    Name = "MDF Dolap Kapağı",
                    Unit = "adet",
                    DefaultQuantity = 15,
                    DisplayOrder = 1,
                    IsRequired = true,
                    Notes = "Standart mutfak dolabı kapakları"
                },
                new RecipeItem
                {
                    RecipeTemplateId = 1,
                    ItemType = ItemTypes.Material,
                    Category = "Tezgah",
                    Name = "Granit Tezgah",
                    Unit = "m",
                    DefaultQuantity = 3.5m,
                    DisplayOrder = 2,
                    IsRequired = true
                },
                new RecipeItem
                {
                    RecipeTemplateId = 1,
                    ItemType = ItemTypes.Material,
                    Category = "Seramik",
                    Name = "Mutfak Duvar Seramiği 30x60",
                    Unit = "m²",
                    DefaultQuantity = 12,
                    DisplayOrder = 3,
                    IsRequired = true
                },
                
                // İşçilikler
                new RecipeItem
                {
                    RecipeTemplateId = 1,
                    ItemType = ItemTypes.Labor,
                    Category = "Dolap İşleri",
                    Name = "Mutfak Dolab ı Montaj İşçiliği",
                    Unit = "takım",
                    DefaultQuantity = 1,
                    DisplayOrder = 50,
                    IsRequired = true
                },
                new RecipeItem
                {
                    RecipeTemplateId = 1,
                    ItemType = ItemTypes.Labor,
                    Category = "Seramik",
                    Name = "Seramik Döşeme İşçiliği",
                    Unit = "m²",
                    DefaultQuantity = 12,
                    DisplayOrder = 51,
                    IsRequired = true
                },
                new RecipeItem
                {
                    RecipeTemplateId = 1,
                    ItemType = ItemTypes.Labor,
                    Category = "Elektrik",
                    Name = "Elektrik Tesisatı İşçiliği",
                    Unit = "nokta",
                    DefaultQuantity = 8,
                    DisplayOrder = 52,
                    IsRequired = true
                }
            });

            // Banyo Reçete Items (Template ID = 2)
            items.AddRange(new[]
            {
                new RecipeItem
                {
                    RecipeTemplateId = 2,
                    ItemType = ItemTypes.Material,
                    Category = "Seramik",
                    Name = "Banyo Duvar Seramiği 25x50",
                    Unit = "m²",
                    DefaultQuantity = 20,
                    DisplayOrder = 1,
                    IsRequired = true
                },
                new RecipeItem
                {
                    RecipeTemplateId = 2,
                    ItemType = ItemTypes.Material,
                    Category = "Armatür",
                    Name = "Duş Bataryası",
                    Unit = "adet",
                    DefaultQuantity = 1,
                    DisplayOrder = 2,
                    IsRequired = true
                },
                new RecipeItem
                {
                    RecipeTemplateId = 2,
                    ItemType = ItemTypes.Labor,
                    Category = "Seramik",
                    Name = "Banyo Seramik Uygulama İşçiliği",
                    Unit = "m²",
                    DefaultQuantity = 20,
                    DisplayOrder = 50,
                    IsRequired = true
                },
                new RecipeItem
                {
                    RecipeTemplateId = 2,
                    ItemType = ItemTypes.Labor,
                    Category = "Tesisat",
                    Name = "Tesisat Montaj İşçiliği",
                    Unit = "takım",
                    DefaultQuantity = 1,
                    DisplayOrder = 51,
                    IsRequired = true
                }
            });

            await context.RecipeItems.AddRangeAsync(items);
            await context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {items.Count} RecipeItem seeded successfully!");
        }

        public static async Task SeedServicePackagesAsync(ApplicationDbContext context)
        {
            if (context.ServicePackages.Any())
            {
                Console.WriteLine("ServicePackages already seeded.");
                return;
            }

            var packages = new List<ServicePackage>
            {
                new ServicePackage
                {
                    Id = 1,
                    Code = "XRANA23",
                    Name = "Komple Mutfak Tadilatı",
                    Description = "Dolap, tezgah, seramik ve elektrik işlerini içeren kapsamlı mutfak tadilat paketi",
                    MainCategoryId = 1, // Mutfak
                    ThumbnailImage = "/images/packages/mutfak-xrana23.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.ServicePackages.AddRangeAsync(packages);
            await context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {packages.Count} ServicePackage seeded successfully!");
        }

        public static async Task SeedPackageItemsAsync(ApplicationDbContext context)
        {
            if (context.PackageItems.Any())
            {
                Console.WriteLine("PackageItems already seeded.");
                return;
            }

            var items = new List<PackageItem>
            {
                // XRANA23 - Mutfak Paketi
                new PackageItem
                {
                    ServicePackageId = 1,
                    Category = "Dolap İşleri",
                    Name = "Alt + Üst Mutfak Dolapları",
                    ItemType = "Material",
                    Unit = "m²",
                    DisplayOrder = 1,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    Category = "Tezgah",
                    Name = "Mutfak Tezgahı (Granit/Çimstone)",
                    ItemType = "Material",
                    Unit = "m",
                    DisplayOrder = 2,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    Category = "Seramik",
                    Name = "Duvar Seramiği 30x60",
                    ItemType = "Material",
                    Unit = "m²",
                    DisplayOrder = 3,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    Category = "Armatür",
                    Name = "Eviye + Batarya",
                    ItemType = "Material",
                    Unit = "adet",
                    DisplayOrder = 4,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    Category = "Cihazlar",
                    Name = "Ocak + Davlumbaz Set",
                    ItemType = "Material",
                    Unit = "set",
                    DisplayOrder = 5,
                    IsRequired = false
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    Category = "İşçilik",
                    Name = "Dolap Montaj İşçiliği",
                    ItemType = "Labor",
                    Unit = "m²",
                    DisplayOrder = 6,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    Category = "İşçilik",
                    Name = "Seramik Döşeme İşçiliği",
                    ItemType = "Labor",
                    Unit = "m²",
                    DisplayOrder = 7,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    Category = "İşçilik",
                    Name = "Tesisat ve Elektrik İşçiliği",
                    ItemType = "Labor",
                    Unit = "gün",
                    DisplayOrder = 8,
                    IsRequired = true
                }
            };

            await context.PackageItems.AddRangeAsync(items);
            await context.SaveChangesAsync();
            
            Console.WriteLine($"✓ {items.Count} PackageItem seeded successfully!");
        }

        public static async Task SeedAllCatalogDataAsync(ApplicationDbContext context)
        {
            await SeedMainCategoriesAsync(context);
            await SeedRecipeTemplatesAsync(context);
            await SeedRecipeItemsAsync(context);
            await SeedServicePackagesAsync(context);
            await SeedPackageItemsAsync(context);
            
            Console.WriteLine("=== ALL CATALOG DATA SEEDED SUCCESSFULLY ===");
        }
    }
}
