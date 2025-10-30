using BTL_WEBDEV2025.Models;
using BTL_WEBDEV2025.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BTL_WEBDEV2025.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AdminController(ILogger<AdminController> logger, AppDbContext db, IWebHostEnvironment env)
        {
            _logger = logger;
            _db = db;
            _env = env;
        }

        // GET: Admin
        public IActionResult Index()
        {
            // Simple authentication check (có thể mở rộng với ASP.NET Identity)
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }
            
            // View hiện render theo tab qua query string; dữ liệu sẽ nạp bằng AJAX từ API bên dưới
            return View();
        }

        // GET: Admin/Login
        public IActionResult Login()
        {
            return View();
        }

        // =====================
        // JSON API for Dashboard
        // =====================
        [HttpGet("/admin/api/stats")]
        public async Task<IActionResult> GetStats()
        {
            if (!IsAdmin()) return Unauthorized();

            // Doanh số (USD) = tổng TotalAmount trong Orders
            var totalSales = await _db.Orders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0m;
            var totalOrders = await _db.Orders.CountAsync();
            // Khách hàng = Users trừ admin (RoleId != 1)
            var totalCustomers = await _db.Users.CountAsync(u => u.RoleId != 1);

            return Ok(new
            {
                totalSales,
                totalOrders,
                totalCustomers
            });
        }

        // =====================
        // PRODUCTS CRUD (JSON)
        // =====================
        [HttpGet("/admin/api/products")]
        public async Task<IActionResult> GetProducts()
        {
            if (!IsAdmin()) return Unauthorized();
            var list = await _db.Products
                .Include(p => p.CategoryRef)
                .Include(p => p.Brand)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    Category = p.CategoryRef != null ? p.CategoryRef.Name : (string.IsNullOrEmpty(p.Category) ? "" : p.Category),
                    p.ImageUrl
                }).ToListAsync();
            return Ok(list);
        }

        public class ProductUpsertDto
        {
            public int? Id { get; set; }
            [Required]
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            [Range(0, double.MaxValue)]
            public decimal Price { get; set; }
            public string? ImageUrl { get; set; }
            public string? Category { get; set; }
        }

        [HttpPost("/admin/api/products/create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductUpsertDto dto)
        {
            if (!IsAdmin()) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description ?? string.Empty,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl ?? string.Empty,
                Category = dto.Category ?? string.Empty
            };
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return Ok(new { product.Id });
        }

        [HttpPost("/admin/api/products/update/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpsertDto dto)
        {
            if (!IsAdmin()) return Unauthorized();
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            product.Name = dto.Name ?? product.Name;
            product.Description = dto.Description ?? product.Description;
            product.Price = dto.Price;
            product.ImageUrl = dto.ImageUrl ?? product.ImageUrl;
            if (!string.IsNullOrWhiteSpace(dto.Category)) product.Category = dto.Category!;
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("/admin/api/products/delete/{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Ok();
        }

        // =====================
        // CUSTOMERS CRUD (Users)
        // =====================
        [HttpGet("/admin/api/customers")]
        public async Task<IActionResult> GetCustomers()
        {
            if (!IsAdmin()) return Unauthorized();
            var list = await _db.Users
                .Where(u => u.RoleId != 1)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.PhoneNumber,
                    u.DateOfBirth
                }).ToListAsync();
            return Ok(list);
        }

        public class CustomerUpsertDto
        {
            public int? Id { get; set; }
            [Required, EmailAddress]
            public string Email { get; set; } = string.Empty;
            [Required]
            public string FullName { get; set; } = string.Empty;
            public string? PhoneNumber { get; set; }
            public DateTime? DateOfBirth { get; set; }
        }

        [HttpPost("/admin/api/customers/create")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerUpsertDto dto)
        {
            if (!IsAdmin()) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                PasswordHash = "temp", // nên thay bằng quy trình tạo tài khoản chuẩn
                RoleId = 2
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(new { user.Id });
        }

        [HttpPost("/admin/api/customers/update/{id:int}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerUpsertDto dto)
        {
            if (!IsAdmin()) return Unauthorized();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.RoleId != 1);
            if (user == null) return NotFound();
            user.Email = dto.Email;
            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.DateOfBirth = dto.DateOfBirth;
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("/admin/api/customers/delete/{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.RoleId != 1);
            if (user == null) return NotFound();
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return Ok();
        }

        // POST: Admin/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Demo authentication (nên sử dụng ASP.NET Identity trong production)
            if (username == "admin" && password == "admin123")
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index");
            }
            
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        // POST: Admin/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            // Always return to unified Account login page
            return RedirectToAction("Login", "Account");
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }
            
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description,Price,DiscountPrice,ImageUrl,Category")] Product product)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                // Save to database
                _db.Products.Add(product);
                _db.SaveChanges();
                
                TempData["Success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            
            return View(product);
        }

        // GET: Admin/Edit/5
        public IActionResult Edit(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            
            return View(product);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,Price,DiscountPrice,ImageUrl,Category")] Product product)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingProduct = _db.Products.FirstOrDefault(p => p.Id == id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.DiscountPrice = product.DiscountPrice;
                    existingProduct.ImageUrl = product.ImageUrl;
                    existingProduct.CategoryId = product.CategoryId;
                    _db.SaveChanges();
                }
                
                TempData["Success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            
            return View(product);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }

            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _db.Products.Remove(product);
                _db.SaveChanges();
                TempData["Success"] = "Product deleted successfully";
            }
            
            return RedirectToAction("Index");
        }

        // GET: Admin/BulkImages
        public IActionResult BulkImages()
        {
            if (!IsAdmin()) return RedirectToAction("Login");
            return View();
        }

        // POST: Admin/BulkImages
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkImages(List<IFormFile> files, string? brand, string? category, bool updateOnly = false)
        {
            if (!IsAdmin()) return RedirectToAction("Login");
            if (files == null || files.Count == 0)
            {
                TempData["Success"] = "No files selected.";
                return RedirectToAction("BulkImages");
            }

            var webRoot = _env.WebRootPath;
            var safeSegment = !string.IsNullOrWhiteSpace(brand) ? brand.Trim() : (!string.IsNullOrWhiteSpace(category) ? category.Trim() : "uploads");
            foreach (var c in Path.GetInvalidFileNameChars()) safeSegment = safeSegment.Replace(c, '-');
            var destDir = Path.Combine(webRoot, "media", "products", safeSegment);
            Directory.CreateDirectory(destDir);

            int saved = 0, updated = 0, created = 0;
            foreach (var file in files)
            {
                if (file.Length <= 0) continue;
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".webp") continue;

                var baseName = Path.GetFileNameWithoutExtension(file.FileName);
                // normalize filename
                var normalized = new string(baseName.Select(ch => char.IsLetterOrDigit(ch) ? char.ToLowerInvariant(ch) : '-').ToArray());
                normalized = System.Text.RegularExpressions.Regex.Replace(normalized, "-+", "-").Trim('-');
                var fileName = normalized + "-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ext;
                var savePath = Path.Combine(destDir, fileName);
                using (var stream = System.IO.File.Create(savePath))
                {
                    await file.CopyToAsync(stream);
                }
                saved++;

                var relUrl = "/media/products/" + safeSegment + "/" + fileName;

                // Try map to product by trailing id pattern "...-123"
                int? parsedId = null;
                var m = System.Text.RegularExpressions.Regex.Match(normalized, @"-(\d+)$");
                if (m.Success && int.TryParse(m.Groups[1].Value, out var idVal)) parsedId = idVal;

                Product? product = null;
                if (parsedId.HasValue)
                {
                    product = _db.Products.FirstOrDefault(p => p.Id == parsedId.Value);
                }
                if (product == null)
                {
                    var nameCandidate = baseName;
                    product = _db.Products.FirstOrDefault(p => p.Name == nameCandidate);
                }

                if (product != null)
                {
                    product.ImageUrl = relUrl;
                    _db.SaveChanges();
                    updated++;
                }
                else if (!updateOnly)
                {
                    var newProduct = new Product
                    {
                        Name = baseName,
                        Description = "",
                        Price = 0,
                        DiscountPrice = null,
                        ImageUrl = relUrl,
                        Category = string.IsNullOrWhiteSpace(category) ? "Unisex" : category!,
                        IsFeatured = false,
                        IsSpecialDeal = false
                    };
                    _db.Products.Add(newProduct);
                    _db.SaveChanges();
                    created++;
                }
            }

            TempData["Success"] = $"Uploaded {saved} files. Updated {updated} products, created {created}.";
            return RedirectToAction("Index");
        }

        private bool IsAdmin()
        {
            // Check session IsAdmin flag (for Admin/Login route)
            if (HttpContext.Session.GetString("IsAdmin") == "true")
            {
                return true;
            }
            
            // Check UserId and RoleId from Account login
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId.Value);
                if (user != null && user.RoleId == 1) // RoleId 1 = Admin
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}

