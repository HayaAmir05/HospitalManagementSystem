using HospitalManagemenet.Data;
using HospitalManagemenet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class DoctorsController : Controller
{
    private readonly AppDbContext _context;

    public DoctorsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Doctors
    public async Task<IActionResult> Index()
    {
        try
        {
            return View(await _context.Doctors.ToListAsync());
        }
        catch
        {
            return StatusCode(500);
        }
    }

    // GET: Doctors/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    // GET: Doctors/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Doctors/Create
    [HttpPost]
    public async Task<IActionResult> Create(Doctor doctor)
    {
        try
        {
            doctor.createdBy = HttpContext.Session.GetString("UserName") ?? "System";
            doctor.CreatedAt = DateTime.Now;

            ModelState.Remove("createdBy");
            ModelState.Remove("CreatedAt");

            if (doctor.Age < 23 || doctor.Age > 80)
            {
                ModelState.AddModelError("Age", "Doctor's age must be between 23 and 80.");
            }

            if (!ModelState.IsValid)
                return View(doctor);

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError("", "An unexpected error occurred.");
            return View(doctor);
        }
    }

    // GET: Doctors/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }
        catch
        {
            return StatusCode(500);
        }
    }
    

    // POST: Doctors/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Doctor doctor)
    {
        try
        {
            if (id != doctor.Id)
                return NotFound();

            var doctorInDb = await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == doctor.Id);

            if (doctorInDb == null)
                return NotFound();

            doctor.createdBy = doctorInDb.createdBy;
            doctor.CreatedAt = doctorInDb.CreatedAt;

            ModelState.Remove("createdBy");
            ModelState.Remove("CreatedAt");

            if (doctor.Age < 23 || doctor.Age > 80)
            {
                ModelState.AddModelError("Age", "Doctor's age must be between 23 and 80.");
            }

            if (!ModelState.IsValid)
                return View(doctor);

            _context.Update(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Doctors.Any(d => d.Id == doctor.Id))
                return NotFound();

            throw;
        }
        catch
        {
            ModelState.AddModelError("", "An unexpected error occurred.");
            return View(doctor);
        }
    }

    // GET: Doctors/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    // POST: Doctors/Delete/5
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return StatusCode(500);
        }
    }
}