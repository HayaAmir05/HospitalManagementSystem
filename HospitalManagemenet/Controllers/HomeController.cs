using HospitalManagemenet.Data;
using HospitalManagemenet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HospitalManagemenet.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) => _context = context;

        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
                return RedirectToAction("Login", "Auth");

            ViewBag.UserName = userName;
            ViewBag.Role = HttpContext.Session.GetString("Role");

            ViewBag.PatientCount = _context.Patients.Count();
            ViewBag.DoctorCount = _context.Doctors.Count();
            ViewBag.AppointmentCount = _context.Appointments.Count();

            return View();
        }
    }
}
