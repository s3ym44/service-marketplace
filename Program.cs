using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;

// Enable legacy timestamp behavior for Npgsql to handle DateTime with Kind=Unspecified
// This is needed because HTML form date inputs don't include timezone information
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// QuestPDF License
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Get connection string from environment variable (Railway) or appsettings
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Parse Railway PostgreSQL URL format: postgres://user:pass@host:port/database
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    var connStr = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
    
    // Override ALL connection string sources
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connStr;
    Environment.SetEnvironmentVariable("DOTNET_ConnectionStrings__DefaultConnection", connStr);
    
    Console.WriteLine($"[CONFIG] DATABASE_URL parsed successfully");
    Console.WriteLine($"[CONFIG] Host: {uri.Host}");
}
else
{
    Console.WriteLine("[CONFIG] WARNING: DATABASE_URL not found, using appsettings");
}

// Get the final connection string from Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string not found.");

Console.WriteLine($"[CONFIG] Final connection string host: {(connectionString.Contains("railway") ? "Railway" : "Local")}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    
    options.User.RequireUniqueEmail = true;
    
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// Authorization policies
builder.Services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(Roles.Admin));
    options.AddPolicy("MaterialSupplierOnly", policy => policy.RequireRole(Roles.MaterialSupplier));
    options.AddPolicy("LaborProviderOnly", policy => policy.RequireRole(Roles.LaborProvider));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole(Roles.Customer));
    options.AddPolicy("SupplierOrAdmin", policy => policy.RequireRole(Roles.MaterialSupplier, Roles.LaborProvider, Roles.Admin));
});

// Session support (kept for backward compatibility)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Railway dynamic PORT - set early
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Clear();
app.Urls.Add($"http://0.0.0.0:{port}");

Console.WriteLine($"=== SERVICE MARKETPLACE STARTUP ===");
Console.WriteLine($"PORT: {port}");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"DATABASE_URL exists: {!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL"))}");

// === RAILWAY DEPLOYMENT ===
// Last updated: 2026-02-10 17:26 UTC - Force refresh with MigrateAsync
// Force migration in production
if (app.Environment.IsProduction())
{
    Console.WriteLine("=== PRODUCTION ENVIRONMENT DETECTED ===");
    
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        Console.WriteLine("Database Connection String (masked):");
        var connStr = db.Database.GetConnectionString();
        if (connStr != null)
        {
            var masked = connStr.Length > 50 ? connStr.Substring(0, 50) + "..." : connStr;
            Console.WriteLine($"  {masked}");
        }
        
        try
        {
            Console.WriteLine("🔄 Running migrations...");
            await db.Database.MigrateAsync(); // ← ÖNEMLI: Tüm pending migrations'ı uygular
            Console.WriteLine("✅ Migrations completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ MIGRATION ERROR: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner: {ex.InnerException.Message}");
            }
            
            // Production'da migration hatası olsa bile devam et
            Console.WriteLine("Continuing despite migration error...");
        }
    }
    
    // Seed Roles and Default Admin
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            Console.WriteLine("Seeding roles...");
            await ServiceMarketplace.Services.RoleSeeder.SeedRolesAsync(services);
            Console.WriteLine("✓ Roles seeded successfully!");
            
            Console.WriteLine("Seeding default admin...");
            await ServiceMarketplace.Services.RoleSeeder.SeedDefaultAdminAsync(services);
            Console.WriteLine("✓ Default admin created/verified!");
            
            // Seed Catalog Data
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            Console.WriteLine("Seeding catalog data...");
            await ServiceMarketplace.Services.CatalogSeeder.SeedAllCatalogDataAsync(dbContext);
            Console.WriteLine("✓ Catalog data seeded successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ SEEDING ERROR: {ex.Message}");
        }
    }
    
    Console.WriteLine("=== MIGRATION CHECK COMPLETE ===");
}

// Health check FIRST - before any middleware that might fail
app.MapWhen(context => context.Request.Path == "/health", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"status\":\"healthy\",\"timestamp\":\"" + DateTime.UtcNow.ToString("o") + "\"}");
    });
});

// Global exception handler - log all errors
app.Use(async (context, next) =>
{
    Console.WriteLine($"[REQUEST] {context.Request.Method} {context.Request.Path}");
    try
    {
        await next();
        Console.WriteLine($"[RESPONSE] {context.Request.Path} => {context.Response.StatusCode}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] {context.Request.Path} => {ex.GetType().Name}: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"[INNER] {ex.InnerException.Message}");
        }
        throw;
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Forward headers for Railway reverse proxy
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | 
                       Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

Console.WriteLine("=== Application ready ===");
app.Run();
