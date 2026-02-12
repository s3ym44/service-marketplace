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
            // Showroom kategorileri: mevcut olanları güncelle, olmayanları ekle
            // NOT: ID 1-4 mevcut veritabanı kayıtlarıdır, FK ilişkileri korunuyor
            var allCategories = new List<MainCategory>
            {
                // === İÇ MEKAN === (ID 1-4 mevcut, 5+ yeni)
                new MainCategory { Id = 1, Name = "Mutfak Tadilat", Description = "Komple mutfak yenileme, dolap, tezgah, seramik", Icon = "🍳", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=600&h=400&fit=crop", DisplayOrder = 1, IsActive = true },
                new MainCategory { Id = 2, Name = "Banyo Tadilat", Description = "Banyo renovasyon, fayans, vitrifiye, tesisat", Icon = "🚿", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1552321554-5fefe8c9ef14?w=600&h=400&fit=crop", DisplayOrder = 2, IsActive = true },
                new MainCategory { Id = 3, Name = "Salon Tadilat", Description = "Salon duvar, zemin, tavan ve aydınlatma", Icon = "🛋️", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=600&h=400&fit=crop", DisplayOrder = 3, IsActive = true },
                new MainCategory { Id = 4, Name = "Yatak Odası Tadilat", Description = "Yatak odası duvar, zemin, gardırop", Icon = "🛏️", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1616594039964-ae9021a400a0?w=600&h=400&fit=crop", DisplayOrder = 4, IsActive = true },
                new MainCategory { Id = 5, Name = "WC Tadilat", Description = "WC yenileme, klozet, lavabo değişimi", Icon = "🚽", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1585412727339-54e4bae3bbf9?w=600&h=400&fit=crop", DisplayOrder = 5, IsActive = true },
                new MainCategory { Id = 6, Name = "Hol Tadilat", Description = "Giriş holü, vestiyer, zemin kaplama", Icon = "🚪", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1513694203232-719a280e022f?w=600&h=400&fit=crop", DisplayOrder = 6, IsActive = true },
                new MainCategory { Id = 7, Name = "Çocuk Odası Tadilat", Description = "Çocuk odası güvenli tasarım ve renklendirme", Icon = "🧸", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1617806118233-18e1de247200?w=600&h=400&fit=crop", DisplayOrder = 7, IsActive = true },
                new MainCategory { Id = 8, Name = "Oturma Odası Tadilat", Description = "Oturma odası dekorasyon ve yenileme", Icon = "📺", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1618219908412-a29a1bb7b86e?w=600&h=400&fit=crop", DisplayOrder = 8, IsActive = true },
                new MainCategory { Id = 9, Name = "Balkon Tadilat", Description = "Balkon cam, zemin, tavan kapatma", Icon = "🌿", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1622372738946-62e02505feb3?w=600&h=400&fit=crop", DisplayOrder = 9, IsActive = true },
                new MainCategory { Id = 10, Name = "Merdiven Boşluğu Tadilat", Description = "Merdiven boşluğu boya, aydınlatma", Icon = "🪜", GroupType = "IcMekan", GroupTitle = "İç Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1572025442646-866d16c84a54?w=600&h=400&fit=crop", DisplayOrder = 10, IsActive = true },

                // === DIŞ MEKAN ===
                new MainCategory { Id = 11, Name = "Cephe Kaplama", Description = "Dış cephe mantolama, kaplama, panel", Icon = "🧱", GroupType = "DisMekan", GroupTitle = "Dış Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1486406146926-c627a92ad1ab?w=600&h=400&fit=crop", DisplayOrder = 11, IsActive = true },
                new MainCategory { Id = 12, Name = "Boya & Sıva İşleri", Description = "Dış cephe boya, sıva, dekoratif kaplama", Icon = "🎨", GroupType = "DisMekan", GroupTitle = "Dış Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1589939705384-5185137a7f0f?w=600&h=400&fit=crop", DisplayOrder = 12, IsActive = true },
                new MainCategory { Id = 13, Name = "Çatı Onarım", Description = "Çatı tamiri, kiremit değişimi", Icon = "🏚️", GroupType = "DisMekan", GroupTitle = "Dış Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1632759145351-1d592919f522?w=600&h=400&fit=crop", DisplayOrder = 13, IsActive = true },
                new MainCategory { Id = 14, Name = "Çatı İzolasyon", Description = "Isı ve su izolasyonu", Icon = "🛡️", GroupType = "DisMekan", GroupTitle = "Dış Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1504307651254-35680f356dfd?w=600&h=400&fit=crop", DisplayOrder = 14, IsActive = true },
                new MainCategory { Id = 15, Name = "Bahçe Düzenleme", Description = "Peyzaj, çim, bitki, taş kaplama", Icon = "🌳", GroupType = "DisMekan", GroupTitle = "Dış Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1558904541-efa843a96f01?w=600&h=400&fit=crop", DisplayOrder = 15, IsActive = true },
                new MainCategory { Id = 16, Name = "Havuz İmalatı", Description = "Havuz yapım, onarım, bakım", Icon = "🏊", GroupType = "DisMekan", GroupTitle = "Dış Mekan Tadilatları", ImageUrl = "https://images.unsplash.com/photo-1576013551627-0cc20b96c2a7?w=600&h=400&fit=crop", DisplayOrder = 16, IsActive = true },

                // === TİCARİ ===
                new MainCategory { Id = 17, Name = "Ofis & İş Yeri Tadilat", Description = "Ofis bölme, zemin, tavan, elektrik", Icon = "💼", GroupType = "Ticari", GroupTitle = "Ticari & Ofis", ImageUrl = "https://images.unsplash.com/photo-1497366216548-37526070297c?w=600&h=400&fit=crop", DisplayOrder = 17, IsActive = true },
            };

            foreach (var cat in allCategories)
            {
                var existing = await context.MainCategories.FindAsync(cat.Id);
                if (existing != null)
                {
                    // Mevcut kaydı güncelle
                    existing.Name = cat.Name;
                    existing.Description = cat.Description;
                    existing.Icon = cat.Icon;
                    existing.GroupType = cat.GroupType;
                    existing.GroupTitle = cat.GroupTitle;
                    existing.ImageUrl = cat.ImageUrl;
                    existing.DisplayOrder = cat.DisplayOrder;
                    existing.IsActive = cat.IsActive;
                }
                else
                {
                    // Yeni kayıt ekle
                    await context.MainCategories.AddAsync(cat);
                }
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"✓ {allCategories.Count} MainCategory seeded/updated successfully!");
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
                    MainCategory = "Mobilya",
                    SubCategory = "Dolap İşleri",
                    Name = "Alt + Üst Mutfak Dolapları",
                    ItemType = "Ana Ürün",
                    Unit = "m²",
                    ConsumptionFormula = "1 m² / alan",
                    DisplayOrder = 1,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    MainCategory = "Mobilya",
                    SubCategory = "Tezgah",
                    Name = "Mutfak Tezgahı (Granit/Çimstone)",
                    ItemType = "Ana Ürün",
                    Unit = "m",
                    ConsumptionFormula = "1 m / uzunluk",
                    DisplayOrder = 2,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    MainCategory = "Duvar",
                    SubCategory = "Kaplama",
                    Name = "Duvar Seramiği 30x60",
                    ItemType = "Ana Ürün",
                    Unit = "m²",
                    ConsumptionFormula = "1 m² / alan",
                    DisplayOrder = 3,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    MainCategory = "Mobilya",
                    SubCategory = "Armatür",
                    Name = "Eviye + Batarya",
                    ItemType = "Ana Ürün",
                    Unit = "adet",
                    ConsumptionFormula = "1 adet",
                    DisplayOrder = 4,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    MainCategory = "Mobilya",
                    SubCategory = "Cihazlar",
                    Name = "Ocak + Davlumbaz Set",
                    ItemType = "Alternatif Ürün",
                    Unit = "set",
                    ConsumptionFormula = "1 set",
                    DisplayOrder = 5,
                    IsRequired = false
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    MainCategory = "Mobilya",
                    SubCategory = "Montaj",
                    Name = "Dolap Montaj İşçiliği",
                    ItemType = "İşçilik",
                    Unit = "m²",
                    ConsumptionFormula = "1 m² / alan",
                    DisplayOrder = 6,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    MainCategory = "Duvar",
                    SubCategory = "Kaplama",
                    Name = "Seramik Döşeme İşçiliği",
                    ItemType = "İşçilik",
                    Unit = "m²",
                    ConsumptionFormula = "1 m² / alan",
                    DisplayOrder = 7,
                    IsRequired = true
                },
                new PackageItem
                {
                    ServicePackageId = 1,
                    MainCategory = "Duvar",
                    SubCategory = "Tesisat",
                    Name = "Tesisat ve Elektrik İşçiliği",
                    ItemType = "İşçilik",
                    Unit = "gün",
                    ConsumptionFormula = "2-3 gün",
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
