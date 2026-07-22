using HospitalManagemenet.Data;
using HospitalManagemenet.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagemenet.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel user)
        {
            // Login logic 
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
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
        public IActionResult Register(User user)
        {
            user.createdBy = "Self-Registered";
            user.CreatedAt = DateTime.Now;
            ModelState.Remove("createdBy");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }



            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
             
            return RedirectToAction("Login");

        }
    }
}
