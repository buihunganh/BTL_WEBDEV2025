using BTL_WEBDEV2025.Data;
using BTL_WEBDEV2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBDEV2025.Controllers
{
    [Route("db")]
    public class DbController : Controller
    {
        private readonly AppDbContext _db;

        public DbController(AppDbContext db)
        {
            _db = db;
        }

        // GET /db/ping
        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            var canConnect = await _db.Database.CanConnectAsync();
            var productCount = 0;
            if (canConnect)
            {
                try
                {
                    productCount = await _db.Products.CountAsync();
                }
                catch
                {
                    // ignored: table may not exist before migrations
                }
            }

            return Ok(new { canConnect, productCount });
        }

        // POST /db/map-images
        [HttpPost("map-images")]
        public async Task<IActionResult> MapImages([FromServices] IWebHostEnvironment env)
        {
            try
            {
                var webRoot = env.WebRootPath;
                var imgDir = Path.Combine(webRoot, "media", "images", "products");
                if (!Directory.Exists(imgDir))
                {
                    Directory.CreateDirectory(imgDir);
                    return NotFound("Image directory not found, but I created it for you at: " + imgDir);
                }

                var files = Directory.GetFiles(imgDir, "*.*", SearchOption.AllDirectories)
                    .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png") || f.EndsWith(".webp"))
                    .ToList();

                if (files.Count == 0)
                {
                    return Ok(new { message = "No image files found in " + imgDir });
                }

                int updated = 0, notFound = 0;
                var log = new List<string>();

                foreach (var f in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(f);
                    var relPath = f.Replace(webRoot, "").Replace('\\', '/');
                    var ext = Path.GetExtension(f);
                    var dirInfo = new DirectoryInfo(Path.GetDirectoryName(f));
                    var brandName = dirInfo.Name.ToLower();

                    // Tên file kiểu: adidas1, nike20...
                    var m = System.Text.RegularExpressions.Regex.Match(fileName, @"([a-zA-Z]+)(\d+)$");
                    if (!m.Success)
                    {
                        notFound++;
                        log.Add($"Filename not in format brand+id: {fileName}");
                        continue;
                    }
                    var brandPart = m.Groups[1].Value.ToLower();
                    var idStr = m.Groups[2].Value;
                    if (!int.TryParse(idStr, out int parsedId))
                    {
                        notFound++;
                        log.Add($"Cannot parse id from {fileName}");
                        continue;
                    }

                    var brand = await _db.Brands.FirstOrDefaultAsync(b => b.Name.ToLower() == brandPart);
                    if (brand == null)
                    {
                        // auto create new Brand if missing
                        brand = new Brand { Name = brandPart };
                        _db.Brands.Add(brand);
                        await _db.SaveChangesAsync();
                        log.Add($"Created new brand: {brandPart}");
                    }

                    var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == parsedId && p.BrandId == brand.Id);
                    if (product != null)
                    {
                        // Cập nhật url cho sản phẩm gốc
                        product.ImageUrl = relPath;
                        updated++;
                        log.Add($"Updated Product Id={parsedId}, Brand={brandPart} to ImageUrl={relPath}");
                    }
                    else
                    {
                        // Tạo sản phẩm mới nếu không tìm thấy
                        product = new Product {
                            Name = fileName,
                            Description = "Auto-created from image upload.",
                            Price = 0,
                            ImageUrl = relPath,
                            BrandId = brand.Id,
                            Category = char.ToUpper(brandPart[0]) + brandPart.Substring(1), // fallback category/brand name
                        };
                        _db.Products.Add(product);
                        updated++;
                        log.Add($"Created new Product {fileName} in Brand={brandPart} with ImageUrl={relPath}");
                    }
                }
                await _db.SaveChangesAsync();

                return Ok(new { filesFound = files.Count, updated, notFound, log });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = "An error occurred on the server.", 
                    message = ex.Message, 
                    details = ex.ToString() 
                });
            }
        }

        // POST /db/selftest - verify we can INSERT/DELETE to Users table
        [HttpPost("selftest")]
        public async Task<IActionResult> SelfTest()
        {
            var result = new { canConnect = false, wrote = false, cleaned = false, message = string.Empty };
            try
            {
                var canConnect = await _db.Database.CanConnectAsync();
                if (!canConnect)
                {
                    return Ok(new { canConnect, wrote = false, cleaned = false, message = "Cannot connect to database" });
                }

                // Ensure RoleId 2 exists
                if (!await _db.Roles.AnyAsync(r => r.Id == 2))
                {
                    _db.Roles.Add(new Role { Id = 2, Name = "Customer" });
                    await _db.SaveChangesAsync();
                }

                var email = $"selftest_{Guid.NewGuid():N}@example.com";
                var user = new User { Email = email, PasswordHash = "test", FullName = "Self Test", RoleId = 2 };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                var wrote = user.Id > 0;

                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                return Ok(new { canConnect = true, wrote, cleaned = true, message = "OK" });
            }
            catch (Exception ex)
            {
                return Ok(new { canConnect = true, wrote = false, cleaned = false, message = ex.GetType().Name + ": " + (ex.InnerException?.Message ?? ex.Message) });
            }
        }
    }
}


