using BTL_WEBDEV2025.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using BTL_WEBDEV2025.Data;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBDEV2025.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private const string CartSessionKey = "ShoppingCart";

        // Simple in-memory payment tracking for demo
        private static readonly ConcurrentDictionary<string, bool> _paymentStatus = new ConcurrentDictionary<string, bool>();
        // Map token (GUID) -> numeric order id (for reference)
        private static readonly ConcurrentDictionary<string, int> _orderTokenMap = new ConcurrentDictionary<string, int>();

        private readonly AppDbContext _db;

        public CartController(ILogger<CartController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
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
        public IActionResult AddToCart(int productId, string productName, decimal price, string imageUrl, int quantity = 1, string size = "", string color = "")
        {
            var cartItems = GetCartItems();
            
            // Match by product + selected variant (size & color)
            var existingItem = cartItems.FirstOrDefault(x => x.ProductId == productId 
                                                            && string.Equals(x.Size ?? string.Empty, size ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                                                            && string.Equals(x.Color ?? string.Empty, color ?? string.Empty, StringComparison.OrdinalIgnoreCase));
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
                    ImageUrl = imageUrl,
                    Size = size ?? string.Empty,
                    Color = color ?? string.Empty
                });
            }

            SaveCartItems(cartItems);
            
            return Json(new { success = true, count = cartItems.Count });
        }

        // POST: Cart/Remove
        [HttpPost]
        public IActionResult RemoveFromCart(int productId, string size = "", string color = "")
        {
            var cartItems = GetCartItems();
            cartItems.RemoveAll(x => x.ProductId == productId
                                       && string.Equals(x.Size ?? string.Empty, size ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                                       && string.Equals(x.Color ?? string.Empty, color ?? string.Empty, StringComparison.OrdinalIgnoreCase));
            SaveCartItems(cartItems);
            
            return Json(new { success = true, count = cartItems.Count });
        }

        // POST: Cart/Update
        [HttpPost]
        public IActionResult UpdateCart(int productId, string size = "", string color = "", int quantity = 1)
        {
            var cartItems = GetCartItems();
            var item = cartItems.FirstOrDefault(x => x.ProductId == productId
                                                     && string.Equals(x.Size ?? string.Empty, size ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                                                     && string.Equals(x.Color ?? string.Empty, color ?? string.Empty, StringComparison.OrdinalIgnoreCase));
            
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

        // POST: Cart/Checkout
        [HttpPost]
        public async Task<IActionResult> Checkout([FromForm] string fullName, [FromForm] string address, [FromForm] string email, [FromForm] string phone, [FromForm] string paymentMethod)
        {
            // require authenticated user
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { success = false, needLogin = true });
            }

            var cartItems = GetCartItems();
            if (cartItems == null || !cartItems.Any())
            {
                return Json(new { success = false, message = "Cart is empty" });
            }

            // create order token used for tracking
            var orderGuid = System.Guid.NewGuid().ToString();
            // default in-memory payment status
            _paymentStatus[orderGuid] = false;

            // Calculate total
            decimal total = cartItems.Sum(x => x.Price * x.Quantity);

            // Save order and details to DB in a transaction
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // normalize payment method value and map to simple labels
                var pmNorm = (paymentMethod ?? string.Empty).Trim().ToLowerInvariant();
                var payByValue = pmNorm == "cod" ? "cash" : (pmNorm == "transfer" ? "bank" : pmNorm);

                // Determine initial status per request: cash -> Unpaid, bank/qr -> Paid
                string initialStatus;
                if (payByValue == "cash") initialStatus = "Unpaid";
                else if (payByValue == "bank" || payByValue == "qr" || payByValue == "transfer" || payByValue == "card") initialStatus = "Paid";
                else initialStatus = "New";

                var order = new Order
                {
                    UserId = userId.Value,
                    CreatedAt = DateTime.UtcNow,
                    TotalAmount = total,
                    PaymentMethod = payByValue,
                    PaymentToken = orderGuid,
                    Status = initialStatus
                };

                _db.Orders.Add(order);
                await _db.SaveChangesAsync(); // generates order.Id

                // store mapping token -> numeric order id for reference
                _orderTokenMap[orderGuid] = order.Id;

                foreach (var it in cartItems)
                {
                    var od = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = it.ProductId,
                        Quantity = it.Quantity,
                        UnitPrice = it.Price
                    };
                    _db.OrderDetails.Add(od);
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                // set in-memory payment flag to reflect initial status
                _paymentStatus[orderGuid] = initialStatus == "Paid";

                // Clear cart after creating order
                SaveCartItems(new List<ShoppingCartItem>());

                // Prepare payment instructions depending on method (keep QR flow for 'transfer')
                string paymentInstructions = string.Empty;
                if (!string.IsNullOrWhiteSpace(pmNorm) && pmNorm.Equals("transfer", StringComparison.OrdinalIgnoreCase))
                {
                    var hostForQr = Request.Host.Host;
                    var port = Request.Host.Port;
                    if (string.IsNullOrEmpty(hostForQr) || hostForQr == "localhost" || hostForQr == "127.0.0.1")
                    {
                        try
                        {
                            var entry = Dns.GetHostEntry(Dns.GetHostName());
                            var lanIp = entry.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(a))?.ToString();
                            if (!string.IsNullOrEmpty(lanIp)) hostForQr = lanIp;
                        }
                        catch
                        {
                            // ignore and fallback to Request.Host
                        }
                    }

                    var path = Url.Action("ConfirmPayment", "Cart", new { orderId = orderGuid });
                    var confirmUrl = $"{Request.Scheme}://{hostForQr}{(port.HasValue ? ":" + port.Value : "")}{path}";
                    var qrApi = "https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=" + System.Net.WebUtility.UrlEncode(confirmUrl);

                    paymentInstructions = $"<div style=\"font-weight:600;margin-bottom:8px\">Bank transfer (QR)</div>" +
                                          $"<div style=\"margin-bottom:8px\">Scan this QR with your phone to confirm payment</div>" +
                                          $"<div style=\"margin-bottom:8px\"> <img alt=\"QR\" src=\"{qrApi}\" style=\"width:160px;height:160px;object-fit:contain;border:1px solid #eaeaea;\"/> </div>" +
                                          $"<div style=\"font-size:0.9rem;color:#666\">Or open this link on your phone: <a href=\"{confirmUrl}\" target=\"_blank\">{confirmUrl}</a></div>";
                }
                else if (payByValue == "cash")
                {
                    paymentInstructions = "<div style=\"font-weight:600\">Cash on delivery</div><div>Please pay the delivery person when your order arrives.</div>";
                }
                else
                {
                    paymentInstructions = string.Empty;
                }

                return Json(new { success = true, orderId = orderGuid, paymentInstructions = paymentInstructions });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                _logger.LogError(ex, "Checkout save failed");
                return Json(new { success = false, message = "Failed to create order" });
            }
        }

        // GET: Cart/ConfirmPayment?orderId=...
        [HttpGet]
        public IActionResult ConfirmPayment(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return Content("Invalid order");
            // Return a small page which will POST to the server to mark payment as confirmed.
            var postUrl = Url.Action("ConfirmPaymentPost", "Cart");
            var html = $"<!doctype html><html><head><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><title>Confirming...</title></head><body style=\"font-family:Arial,Helvetica,sans-serif;padding:20px;text-align:center\">" +
                       $"<h3>Processing payment confirmation...</h3>" +
                       $"<script>" +
                       $"fetch('{postUrl}',{{method:'POST',headers:{{'Content-Type':'application/json'}},body:JSON.stringify({{ orderId: '{System.Net.WebUtility.HtmlEncode(orderId)}' }})}})" +
                       ".then(r=>r.json()).then(j=>{ document.body.innerHTML = '<h2>Payment confirmed</h2><p>Order {System.Net.WebUtility.HtmlEncode(orderId)} marked as paid. You can close this page.</p>'; }).catch(e=>{ document.body.innerHTML = '<h2>Error</h2><p>Could not confirm payment.</p>'; });" +
                       $"</script></body></html>";
            return Content(html, "text/html");
        }

        // POST: Cart/ConfirmPaymentPost (JSON body)
        [HttpPost]
        public async Task<IActionResult> ConfirmPaymentPost([FromBody] ConfirmPaymentRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.OrderId)) return BadRequest(new { success = false });
            _paymentStatus[req.OrderId] = true;

            // Persist status in DB using PaymentToken
            try
            {
                var order = await _db.Orders.FirstOrDefaultAsync(o => o.PaymentToken == req.OrderId);
                if (order != null)
                {
                    order.Status = "Paid";
                    _db.Orders.Update(order);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ConfirmPaymentPost error");
            }

            if (_orderTokenMap.TryGetValue(req.OrderId, out var numericOrderId))
            {
                _logger.LogInformation("Payment confirmed for order token {Token} -> OrderId {OrderId}", req.OrderId, numericOrderId);
            }

            return Json(new { success = true });
        }

        // GET: Cart/CheckPaymentStatus?orderId=...
        [HttpGet]
        public IActionResult CheckPaymentStatus(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return Json(new { paid = false });
            var paid = _paymentStatus.TryGetValue(orderId, out var v) && v;
            return Json(new { paid = paid });
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

public class ConfirmPaymentRequest { public string? OrderId { get; set; } }

