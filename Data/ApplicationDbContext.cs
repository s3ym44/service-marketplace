using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferMaterial> OfferMaterials { get; set; }
        public DbSet<AdminPriceReference> AdminPriceReferences { get; set; }
        public DbSet<ListingCalculation> ListingCalculations { get; set; }
        public DbSet<Listing> Listings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships if needed
            modelBuilder.Entity<OfferMaterial>()
                .HasOne(om => om.Offer)
                .WithMany(o => o.Materials)
                .HasForeignKey(om => om.OfferId)
                .OnDelete(DeleteBehavior.Cascade);

            // Offer-Listing relationship (optional, no FK for existing data compatibility)
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Listing)
                .WithMany(l => l.Offers)
                .HasForeignKey(o => o.ListingId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed AdminPriceReferences data as per requirements
             modelBuilder.Entity<AdminPriceReference>().HasData(
                new AdminPriceReference { Id = 1, Category = "Boya", MaterialName = "İç Cephe Duvar Boyası", Brand = "Fawori", Quality = "Standart", Quantity = 15, Unit = "Litre", BasePrice = 850, RegionModifier = 1.0, IsActive = true },
                new AdminPriceReference { Id = 2, Category = "Boya", MaterialName = "İç Cephe Duvar Boyası", Brand = "Marshall", Quality = "Premium", Quantity = 15, Unit = "Litre", BasePrice = 1200, RegionModifier = 1.0, IsActive = true },
                new AdminPriceReference { Id = 3, Category = "Boya", MaterialName = "Astar", Brand = "Marshall", Quality = "Standart", Quantity = 15, Unit = "Litre", BasePrice = 450, RegionModifier = 1.0, IsActive = true },
                new AdminPriceReference { Id = 4, Category = "Seramik", MaterialName = "Yer Seramiği 60x60", Brand = "Ege Seramik", Quality = "Premium", Quantity = 1, Unit = "m²", BasePrice = 180, RegionModifier = 1.0, IsActive = true }
            );

            // Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Supplier", NormalizedName = "SUPPLIER" },
                new IdentityRole { Id = "3", Name = "Customer", NormalizedName = "CUSTOMER" }
            );
        }
    }
}
