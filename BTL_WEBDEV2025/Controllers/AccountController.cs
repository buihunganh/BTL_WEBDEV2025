using BTL_WEBDEV2025.Models;
using Microsoft.AspNetCore.Mvc;

namespace BTL_WEBDEV2025.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Demo: luôn chuyển sang đăng ký với email đã nhập
            return RedirectToAction("Register", new { email = model.Email });
        }

        [HttpGet]
        public IActionResult Register(string? email)
        {
            var vm = new RegisterViewModel();
            if (!string.IsNullOrWhiteSpace(email)) vm.Email = email;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            // additional server-side validation for DOB combination
            if (model.BirthDay.HasValue || model.BirthMonth.HasValue || model.BirthYear.HasValue)
            {
                if (!(model.BirthDay.HasValue && model.BirthMonth.HasValue && model.BirthYear.HasValue))
                {
                    ModelState.AddModelError("BirthDay", "Please complete date of birth (DD/MM/YYYY)");
                }
                else
                {
                    try
                    {
                        var dateOfBirth = new DateTime(model.BirthYear!.Value, model.BirthMonth!.Value, model.BirthDay!.Value);
                        // age check >= 13
                        var today = DateTime.Today;
                        var age = today.Year - dateOfBirth.Year;
                        if (dateOfBirth.Date > today.AddYears(-age)) age--;
                        if (age < 13)
                        {
                            ModelState.AddModelError("BirthDay", "You must be at least 13 years old.");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("BirthDay", "Invalid date of birth");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Demo: luôn coi đăng ký thành công
            TempData["AuthMessage"] = "Account created (demo).";
            return RedirectToAction("Index", "Home");
        }
    }
}