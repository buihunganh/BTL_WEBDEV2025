using System.Diagnostics;
using BTL_WEBDEV2025.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BTL_WEBDEV2025.Data;

namespace BTL_WEBDEV2025.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;
        private readonly List<Product> _fallbackProducts;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
            _fallbackProducts = InitializeProducts();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Web API endpoint for fetching products
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = TryGetProductsFromDb();
            return Json(products);
        }

        // AJAX endpoint for search functionality
        [HttpPost]
        public IActionResult SearchProducts([FromBody] SearchRequest request)
        {
            if (string.IsNullOrEmpty(request?.Query))
            {
                return Json(new { products = new List<Product>() });
            }

            var all = TryGetProductsFromDb();
            var results = all.Where(p => 
                p.Name.Contains(request.Query, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(request.Query, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            return Json(new { products = results });
        }

        // AJAX endpoint for getting featured products
        [HttpGet]
        public IActionResult GetFeaturedProducts()
        {
            var featuredProducts = TryGetProductsFromDb().Where(p => p.IsFeatured).ToList();
            return Json(featuredProducts);
        }

        // AJAX endpoint for getting special deals
        [HttpGet]
        public IActionResult GetSpecialDeals()
        {
            var specialDeals = TryGetProductsFromDb().Where(p => p.IsSpecialDeal).ToList();
            return Json(specialDeals);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Product> InitializeProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Air Max 270", Description = "Premium running shoes", Price = 150, ImageUrl = "https://via.placeholder.com/300", Category = "Men", IsFeatured = true },
                new Product { Id = 2, Name = "Air Force 1", Description = "Classic lifestyle shoes", Price = 90, DiscountPrice = 70, ImageUrl = "https://via.placeholder.com/300", Category = "Unisex", IsFeatured = true, IsSpecialDeal = true },
                new Product { Id = 3, Name = "Zoom Pegasus", Description = "High-performance running", Price = 120, ImageUrl = "https://via.placeholder.com/300", Category = "Men", IsFeatured = true },
                new Product { Id = 4, Name = "Revolution 6", Description = "Everyday running", Price = 60, ImageUrl = "https://via.placeholder.com/300", Category = "Women", IsFeatured = true },
                new Product { Id = 5, Name = "Court Vision", Description = "Basketball lifestyle", Price = 65, DiscountPrice = 45, ImageUrl = "https://via.placeholder.com/300", Category = "Men", IsSpecialDeal = true },
                new Product { Id = 6, Name = "React Element", Description = "Futuristic design", Price = 130, ImageUrl = "https://via.placeholder.com/300", Category = "Unisex", IsFeatured = true },
                new Product { Id = 7, Name = "Free RN", Description = "Natural motion", Price = 80, DiscountPrice = 60, ImageUrl = "https://via.placeholder.com/300", Category = "Women", IsSpecialDeal = true },
                new Product { Id = 8, Name = "Dunk Low", Description = "Skateboarding classic", Price = 100, ImageUrl = "https://via.placeholder.com/300", Category = "Unisex", IsFeatured = true }
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

    // Request model for search
    public class SearchRequest
    {
        public string Query { get; set; } = string.Empty;
    }
}
