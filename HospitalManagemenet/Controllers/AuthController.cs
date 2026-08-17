using HospitalManagemenet.Models;
using HospitalManagemenet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagemenet.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var userInDb = await _authService.ValidateUserAsync(user.Email, user.Password);

            if (userInDb != null)
            {
                HttpContext.Session.SetString("UserName", userInDb.Name);
                HttpContext.Session.SetString("Role", userInDb.Role);

                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Invalid email or password";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {

            ModelState.Remove("createdBy");
            ModelState.Remove("CreatedAt");

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            bool success = await _authService.RegisterAsync(user);

            if (!success)
            {
                ModelState.AddModelError("Email",
                    "An account with this email already exists.");

                return View(user);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");

        }
    }
}
