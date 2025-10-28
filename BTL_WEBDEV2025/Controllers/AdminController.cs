using BTL_WEBDEV2025.Models;
using Microsoft.AspNetCore.Mvc;

namespace BTL_WEBDEV2025.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private List<Product> _products = new();

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
            _products = InitializeProducts();
        }

        // GET: Admin
        public IActionResult Index()
        {
            // Simple authentication check (có thể mở rộng với ASP.NET Identity)
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }
            
            return View(_products);
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
            return RedirectToAction("Login");
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
                // Save to database (demo)
                product.Id = _products.Count + 1;
                _products.Add(product);
                
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

            var product = _products.FirstOrDefault(p => p.Id == id);
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
                var existingProduct = _products.FirstOrDefault(p => p.Id == id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.DiscountPrice = product.DiscountPrice;
                    existingProduct.ImageUrl = product.ImageUrl;
                    existingProduct.Category = product.Category;
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

            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
                TempData["Success"] = "Product deleted successfully";
            }
            
            return RedirectToAction("Index");
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        private List<Product> InitializeProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Air Max 270", Description = "Premium running shoes", Price = 150, ImageUrl = "https://via.placeholder.com/300", Category = "Men", IsFeatured = true },
                new Product { Id = 2, Name = "Air Force 1", Description = "Classic lifestyle shoes", Price = 90, DiscountPrice = 70, ImageUrl = "https://via.placeholder.com/300", Category = "Unisex", IsFeatured = true, IsSpecialDeal = true },
                new Product { Id = 3, Name = "Zoom Pegasus", Description = "High-performance running", Price = 120, ImageUrl = "https://via.placeholder.com/300", Category = "Men", IsFeatured = true }
            };
        }
    }
}

