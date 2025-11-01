using Microsoft.AspNetCore.Mvc;
using BTL_WEBDEV2025.Data;
using System.ComponentModel.DataAnnotations;

namespace BTL_WEBDEV2025.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public UserController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        // GET: /User
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var email = HttpContext.Session.GetString("UserEmail") ?? "";
            var name = HttpContext.Session.GetString("UserName") ?? email;
            ViewBag.Email = email;
            ViewBag.Name = name;
            var user = _db.Users.FirstOrDefault(u => u.Id == userId.Value);
            ViewBag.AvatarUrl = user?.AvatarUrl;
            ViewBag.Phone = user?.PhoneNumber;
            ViewBag.Dob = user?.DateOfBirth;
            ViewBag.Address = user?.Address;
            ViewBag.Gender = user?.Gender;
            // Load orders for this user
            var orders = _db.Orders
                .Where(o => o.UserId == userId.Value)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderVm
                {
                    Id = o.Id,
                    CreatedAt = o.CreatedAt,
                    TotalAmount = o.TotalAmount,
                    Items = o.OrderDetails.Select(d => new OrderItemVm
                    {
                        ProductId = d.ProductId,
                        ProductName = d.Product != null ? d.Product.Name : ("#" + d.ProductId),
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice
                    }).ToList()
                }).ToList();
            return View(orders);
        }

        public class UpdateProfileDto
        {
            [Required]
            public string FullName { get; set; } = string.Empty;
            public string? Password { get; set; }
            public string? PhoneNumber { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string? Gender { get; set; }
            public string? Address { get; set; }
        }

        public class OrderItemVm
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public class OrderVm
        {
            public int Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public decimal TotalAmount { get; set; }
            public List<OrderItemVm> Items { get; set; } = new();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile(UpdateProfileDto dto)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");
            if (!ModelState.IsValid)
            {
                TempData["UserMsgError"] = "Please provide a valid name.";
                return RedirectToAction("Index");
            }
            var user = _db.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");

            user.FullName = dto.FullName.Trim();
            user.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim();
            user.DateOfBirth = dto.DateOfBirth;
            user.Gender = string.IsNullOrWhiteSpace(dto.Gender) ? null : dto.Gender.Trim();
            user.Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = dto.Password; // plain per current project
            }
            _db.SaveChanges();

            HttpContext.Session.SetString("UserName", user.FullName);
            TempData["UserMsgSuccess"] = "Profile updated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");
            var user = _db.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");

            _db.Users.Remove(user);
            _db.SaveChanges();

            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserName");
            TempData["AuthMessage"] = "Account deleted.";
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");
            if (avatar == null || avatar.Length == 0)
            {
                TempData["UserMsgError"] = "Please choose an image file.";
                return RedirectToAction("Index");
            }

            var ext = Path.GetExtension(avatar.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowed.Contains(ext))
            {
                TempData["UserMsgError"] = "Only JPG/PNG/WEBP are allowed.";
                return RedirectToAction("Index");
            }

            var webRoot = _env.WebRootPath;
            var dir = Path.Combine(webRoot, "media", "avatars", userId.Value.ToString());
            Directory.CreateDirectory(dir);
            var fileName = "avatar-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ext;
            var savePath = Path.Combine(dir, fileName);
            using (var stream = System.IO.File.Create(savePath))
            {
                await avatar.CopyToAsync(stream);
            }
            var relUrl = "/media/avatars/" + userId.Value + "/" + fileName;

            var user = _db.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user == null) return RedirectToAction("Login", "Account");
            user.AvatarUrl = relUrl;
            _db.SaveChanges();

            TempData["UserMsgSuccess"] = "Avatar updated.";
            return RedirectToAction("Index");
        }
    }
}


