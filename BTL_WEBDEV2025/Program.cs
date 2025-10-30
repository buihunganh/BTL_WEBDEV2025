using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using BTL_WEBDEV2025.Data;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers with TempData Session Provider
builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

// Database: Choose provider by configuration (SqlServer default, Sqlite optional)
var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (string.Equals(databaseProvider, "Sqlite", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")
            ?? "Data Source=app.db");
    }
    else
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});

// Sessions for simple auth state
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseResponseCompression();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        const string oneYear = "public,max-age=31536000,immutable";
        ctx.Context.Response.Headers["Cache-Control"] = oneYear;
    }
});

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Apply pending migrations and seed minimal data (roles/admin) on startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();

        // Seed Roles
        if (!db.Roles.Any())
        {
            db.Roles.AddRange(
                new BTL_WEBDEV2025.Models.Role { Id = 1, Name = "Admin" },
                new BTL_WEBDEV2025.Models.Role { Id = 2, Name = "Customer" }
            );
            db.SaveChanges();
        }

        // Seed a default admin if missing
        if (!db.Users.Any(u => u.RoleId == 1))
        {
            db.Users.Add(new BTL_WEBDEV2025.Models.User
            {
                Email = "admin@nike.com",
                PasswordHash = "admin123", // demo only
                FullName = "Admin Account",
                PhoneNumber = "123456",
                RoleId = 1
            });
            db.SaveChanges();
        }
    }
    catch { /* ignore startup seeding errors in dev */ }
}

app.Run();
