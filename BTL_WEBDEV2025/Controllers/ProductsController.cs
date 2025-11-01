using BTL_WEBDEV2025.Models;
using Microsoft.AspNetCore.Mvc;
using BTL_WEBDEV2025.Data;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBDEV2025.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly AppDbContext _db;
        private readonly List<Product> _fallbackProducts;

        public ProductsController(ILogger<ProductsController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
            _fallbackProducts = InitializeProducts();
        }

        // GET: Products
        public IActionResult Index()
        {
            var all = TryGetProductsFromDb();
            var brand = HttpContext.Request.Query["brand"].ToString();
            if (!string.IsNullOrWhiteSpace(brand))
            {
                all = all.Where(p => p.Brand != null ? p.Brand.Name.Equals(brand, StringComparison.OrdinalIgnoreCase) : false).ToList();
            }
            return View(all);
        }

        // GET: Products/Brand/{name}
        public IActionResult Brand(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return RedirectToAction("Index");
            var all = TryGetProductsFromDb().Where(p => p.Brand != null && p.Brand.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.BrandName = name;
            return View("Index", all);
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = TryGetProductsFromDb().FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Men
        public IActionResult Men()
        {
            var brands = _db.Brands.Select(b => b.Name).ToList();
            var products = _db.Products
                .Include(p => p.Brand)
                .Where(p => p.CategoryId == 1 || p.CategoryId == 4)
                .ToList();
            ViewBag.Brands = brands;
            return View(products);
        }

        // GET: Products/Women
        public IActionResult Women()
        {
            var brands = _db.Brands.Select(b => b.Name).ToList();
            var products = _db.Products
                .Include(p => p.Brand)
                .Where(p => p.CategoryId == 2 || p.CategoryId == 4)
                .ToList();
            ViewBag.Brands = brands;
            return View(products);
        }

        // GET: Products/Kid
        public IActionResult Kid()
        {
            var brands = _db.Brands.Select(b => b.Name).ToList();
            var products = _db.Products
                .Include(p => p.Brand)
                .Where(p => p.CategoryId == 3 || p.CategoryId == 4)
                .ToList();
            ViewBag.Brands = brands;
            return View(products);
        }

        // GET: Products/Sale
        public IActionResult Sale()
        {
            var saleProducts = TryGetProductsFromDb().Where(p => p.DiscountPrice.HasValue).ToList();
            return View(saleProducts);
        }

        // AJAX endpoint for filtering
        [HttpGet]
        public IActionResult Filter(string category)
        {
            var all = TryGetProductsFromDb();
            var filteredProducts = string.IsNullOrEmpty(category) 
                ? all 
                : all.Where(p => p.Category == category).ToList();
            
            return Json(filteredProducts);
        }

        private List<string> GetAllBrandsForFilter() =>
            _db.Brands.Select(b => b.Name).ToList();

        private List<Product> InitializeProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Air Max 270", Description = "Premium running shoes with Air Max technology", Price = 150, ImageUrl = "https://via.placeholder.com/300", Category = "Men", IsFeatured = true },
                new Product { Id = 2, Name = "Air Force 1", Description = "Classic lifestyle shoes", Price = 90, DiscountPrice = 70, ImageUrl = "https://via.placeholder.com/300", Category = "Unisex", IsFeatured = true, IsSpecialDeal = true },
                new Product { Id = 3, Name = "Zoom Pegasus", Description = "High-performance running shoes", Price = 120, ImageUrl = "https://via.placeholder.com/300", Category = "Men", IsFeatured = true },
                new Product { Id = 4, Name = "Revolution 6", Description = "Everyday running for women", Price = 60, ImageUrl = "https://via.placeholder.com/300", Category = "Women", IsFeatured = true },
                new Product { Id = 5, Name = "Court Vision", Description = "Basketball lifestyle shoes", Price = 65, DiscountPrice = 45, ImageUrl = "https://via.placeholder.com/300", Category = "Men", IsSpecialDeal = true },
                new Product { Id = 6, Name = "React Element", Description = "Futuristic design sneakers", Price = 130, ImageUrl = "https://via.placeholder.com/300", Category = "Unisex", IsFeatured = true },
                new Product { Id = 7, Name = "Free RN", Description = "Natural motion running shoes", Price = 80, DiscountPrice = 60, ImageUrl = "https://via.placeholder.com/300", Category = "Women", IsSpecialDeal = true },
                new Product { Id = 8, Name = "Dunk Low", Description = "Skateboarding classic", Price = 100, ImageUrl = "https://via.placeholder.com/300", Category = "Unisex", IsFeatured = true },
                new Product { Id = 9, Name = "Kids Air Max", Description = "Comfortable running for kids", Price = 70, ImageUrl = "https://via.placeholder.com/300", Category = "Kid", IsFeatured = false },
                new Product { Id = 10, Name = "Kids Basketball", Description = "Basketball shoes for young athletes", Price = 50, DiscountPrice = 35, ImageUrl = "https://via.placeholder.com/300", Category = "Kid", IsSpecialDeal = true }
            };
        }

        private List<Product> TryGetProductsFromDb()
        {
            try
            {
                if (_db.Database.CanConnect())
                {
                    var list = _db.Products.ToList();
                    if (list.Count > 0) return list;
                }
            }
            catch
            {
                // ignore and fallback
            }
            return _fallbackProducts;
        }
    }
}

