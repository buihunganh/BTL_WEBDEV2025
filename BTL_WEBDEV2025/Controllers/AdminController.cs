using BTL_WEBDEV2025.Models;
using BTL_WEBDEV2025.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBDEV2025.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly AppDbContext _db;

        public AdminController(ILogger<AdminController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET: Admin
        public IActionResult Index()
        {
            // Simple authentication check (có thể mở rộng với ASP.NET Identity)
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }
            
            // Get products from database (use navigation properties)
            var products = _db.Products
                .Include(p => p.CategoryRef)
                .Include(p => p.Brand)
                .ToList();
            return View(products);
        }

        // GET: Admin/Login
        public IActionResult Login()
        {
            return View();
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

