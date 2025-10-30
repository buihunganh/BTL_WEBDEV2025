using Microsoft.AspNetCore.Mvc;

namespace BTL_WEBDEV2025.Controllers
{
    public class UserController : Controller
    {
        // GET: /User
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Email = HttpContext.Session.GetString("UserEmail") ?? "";
            return View();
        }
    }
}


