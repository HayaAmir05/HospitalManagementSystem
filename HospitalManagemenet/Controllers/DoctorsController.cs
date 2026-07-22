
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagemenet.Models;
using HospitalManagemenet.Data;

public class DoctorsController : Controller
{
    private readonly AppDbContext _context;

    public DoctorsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: DOCTORS
    public  ActionResult Index()    
    {
        return View(_context.Doctors.ToList());
    }

    // GET: DOCTORS/Details/5
    public ActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var doctor = _context.Doctors
            .FirstOrDefault(m => m.Id == id);
        if (doctor == null)
        {
            return NotFound();
        }

        return View(doctor);
    }

    // GET: DOCTORS/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: DOCTORS/Create
    
    [HttpPost]
    public ActionResult Create(Doctor doctor)
    {
        try
        {
            // Set audit fields server-side — never trust these from the form
            doctor.createdBy = HttpContext.Session.GetString("UserName") ?? "System";
            doctor.CreatedAt = DateTime.Now;

            // Because these are set AFTER model binding/validation already ran,
            // remove any stale validation errors that were recorded for them
            ModelState.Remove("createdBy");
            ModelState.Remove("CreatedAt");
            if (doctor.Age < 23 || doctor.Age > 80)
            {
                ModelState.AddModelError("Age", "Doctor's age must be between 23 and 80.");
            }


            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ModelState.AddModelError(string.Empty, "Validation failed: " + string.Join(" | ", errors));
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while creating the doctor: " + ex.Message);
        }

        return View(doctor);
    }

    // GET: DOCTORS/Edit/5
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var doctor = _context.Doctors.Find(id);
        if (doctor == null)
        {
            return NotFound();
        }
        return View(doctor);
    }

    // POST: DOCTORS/Edit/5
    
    [HttpPost]
   
    public ActionResult Edit(int? id, Doctor doctor)
    {
        if (id != doctor.Id)
        {
            return NotFound();
        }
        Doctor doctorInDb = _context.Doctors.AsNoTracking().FirstOrDefault(d => d.Id == doctor.Id);
        doctor.createdBy = doctorInDb.createdBy;
        doctor.CreatedAt = doctorInDb.CreatedAt;

        ModelState.Remove("createdBy");
        ModelState.Remove("CreatedAt");

        if (doctor.Age < 23 || doctor.Age > 80)
        {
            ModelState.AddModelError("Age", "Doctor's age must be between 23 and 80.");
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(doctor);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(doctor.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(doctor);
    }

    // GET: DOCTORS/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var doctor =  _context.Doctors
            .FirstOrDefault(m => m.Id == id);
        if (doctor == null)
        {
            return NotFound();
        }

        return View(doctor);
    }

    // POST: DOCTORS/Delete/5
    [HttpPost, ActionName("Delete")]
   
    public ActionResult DeleteConfirmed(int? id)
    {
        var doctor = _context.Doctors.Find(id);
        if (doctor != null)
        {
            _context.Doctors.Remove(doctor);
        }

         _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private bool DoctorExists(int? id)
    {
        return _context.Doctors.Any(e => e.Id == id);
    }
}
