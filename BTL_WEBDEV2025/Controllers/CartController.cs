using BTL_WEBDEV2025.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BTL_WEBDEV2025.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private const string CartSessionKey = "ShoppingCart";

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }

        // GET: Cart
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                ViewBag.RequireLogin = true;
                return View(new List<ShoppingCartItem>());
            }

            var cartItems = GetCartItems();
            return View(cartItems);
        }

        // POST: Cart/Add
        [HttpPost]
        public IActionResult AddToCart(int productId, string productName, decimal price, string imageUrl, int quantity = 1)
        {
            var cartItems = GetCartItems();
            
            var existingItem = cartItems.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new ShoppingCartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity,
                    ImageUrl = imageUrl
                });
            }

            SaveCartItems(cartItems);
            
            return Json(new { success = true, count = cartItems.Count });
        }

        // POST: Cart/Remove
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cartItems = GetCartItems();
            cartItems.RemoveAll(x => x.ProductId == productId);
            SaveCartItems(cartItems);
            
            return Json(new { success = true, count = cartItems.Count });
        }

        // POST: Cart/Update
        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var cartItems = GetCartItems();
            var item = cartItems.FirstOrDefault(x => x.ProductId == productId);
            
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    cartItems.Remove(item);
                }
            }
            
            SaveCartItems(cartItems);
            return Json(new { success = true, count = cartItems.Count });
        }

        // GET: Cart/Count
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cartItems = GetCartItems();
            return Json(new { count = cartItems.Sum(x => x.Quantity) });
        }

        // POST: Cart/Clear
        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
            return Json(new { success = true });
        }

        private List<ShoppingCartItem> GetCartItems()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<ShoppingCartItem>();
            }
            
            try
            {
                return JsonSerializer.Deserialize<List<ShoppingCartItem>>(cartJson) ?? new List<ShoppingCartItem>();
            }
            catch
            {
                return new List<ShoppingCartItem>();
            }
        }

        private void SaveCartItems(List<ShoppingCartItem> items)
        {
            var cartJson = JsonSerializer.Serialize(items);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }
    }
}

