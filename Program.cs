using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Get connection string from environment variable (Railway) or appsettings
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
string connectionString;

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Parse Railway PostgreSQL URL format: postgres://user:pass@host:port/database
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string not found.");
}

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
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SupplierOnly", policy => policy.RequireRole("Supplier"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
    options.AddPolicy("SupplierOrAdmin", policy => policy.RequireRole("Supplier", "Admin"));
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
            Console.WriteLine("Checking database connection...");
            var canConnect = db.Database.CanConnect();
            Console.WriteLine($"Can connect: {canConnect}");
            
            if (canConnect)
            {
                Console.WriteLine("Getting pending migrations...");
                var pending = db.Database.GetPendingMigrations().ToList();
                Console.WriteLine($"Pending migrations count: {pending.Count}");
                
                if (pending.Any())
                {
                    Console.WriteLine("Pending migrations:");
                    foreach (var m in pending)
                    {
                        Console.WriteLine($"  - {m}");
                    }
                    
                    Console.WriteLine("Applying migrations...");
                    db.Database.Migrate();
                    Console.WriteLine("✓ All migrations applied successfully!");
                }
                else
                {
                    Console.WriteLine("✓ Database is up to date, no pending migrations.");
                }
            }
            else
            {
                Console.WriteLine("✗ Cannot connect to database!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ MIGRATION ERROR: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner: {ex.InnerException.Message}");
            }
            
            // Production'da migration hatası olsa bile devam et
            Console.WriteLine("Continuing despite migration error...");
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // HSTS kaldır - Railway handles SSL
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
