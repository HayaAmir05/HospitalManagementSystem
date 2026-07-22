
using HospitalManagemenet.Data;
using HospitalManagemenet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

public class AppointmentsController : Controller
{
    private readonly AppDbContext _context;

    public AppointmentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: APPOINTMENTS
    public ActionResult Index()    
    {
        var appointments = _context.Appointments
        .Include(a => a.Patient)
        .Include(a => a.Doctor)
        .ToList();
        return View(appointments);
    }

    // GET: APPOINTMENTS/Details/5
    public ActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var appointment = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefault(m => m.Id == id);

        if (appointment == null)
        {
            return NotFound();
        }

        return View(appointment);
    }

    // GET: APPOINTMENTS/Create
    public IActionResult Create()
    {
        ViewBag.PatientId = new SelectList(_context.Patients, "Id", "Name");
        ViewBag.DoctorId = new SelectList(_context.Doctors, "Id", "Name");
        ViewBag.StatusList = new SelectList(new[] { "Pending", "Completed", "Cancelled" });
        return View();
    }

    // POST: APPOINTMENTS/Create

    [HttpPost]
  
    public ActionResult Create( Appointment appointment)
    {

        try
        {
            appointment.createdBy = HttpContext.Session.GetString("UserName") ?? "System";
            appointment.CreatedAt = DateTime.Now;
            ModelState.Remove("createdBy");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("Patient");   // navigation properties — remove stale validation on these too
            ModelState.Remove("Doctor");

            appointment.Status = "Pending";
            ModelState.Remove("Status");


            if (ModelState.IsValid)
            {
                _context.Add(appointment);
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
            ModelState.AddModelError(string.Empty, "An error occurred while creating the appointment: " + ex.Message);
        }

        
        return View(appointment);
    }

    // GET: APPOINTMENTS/Edit/5
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var appointment = _context.Appointments.Find(id);
        if (appointment == null)
        {
            return NotFound();
        }

        ViewBag.PatientId = new SelectList(_context.Patients, "Id", "Name", appointment.PatientId);
        ViewBag.DoctorId = new SelectList(_context.Doctors, "Id", "Name", appointment.DoctorId);
        ViewBag.StatusList = new SelectList(new[] { "Pending", "Completed", "Cancelled" }, appointment.Status);

        return View(appointment);
    }

    // POST: APPOINTMENTS/Edit/5

    [HttpPost]

    public ActionResult Edit(int id,  Appointment appointment)
    {
        if (id != appointment.Id)
        {
            return NotFound();
        }

        var existing = _context.Appointments.AsNoTracking().FirstOrDefault(a => a.Id == id);
        if (existing == null)
        {
            return NotFound();
        }

        appointment.createdBy = existing.createdBy;
        appointment.CreatedAt = existing.CreatedAt;

        ModelState.Remove("createdBy");
        ModelState.Remove("CreatedAt");
        ModelState.Remove("Patient");
        ModelState.Remove("Doctor");

        // New rule: date can only stay the same or move forward, never backward, relative to its ORIGINAL value
        if (appointment.AppointmentDate.Date < existing.AppointmentDate.Date)
        {
            ModelState.AddModelError("AppointmentDate", "Appointment date cannot be moved to an earlier date than originally scheduled. You can only delay, not reschedule backward.");
        }

        if (appointment.AppointmentDate.Date < DateTime.Today && appointment.Status == "Pending")
        {
            ModelState.AddModelError("Status", "Past appointments cannot remain Pending — mark as Completed or Cancelled.");
        }

        

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(appointment);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(appointment.Id))
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

        ViewBag.PatientId = new SelectList(_context.Patients, "Id", "Name", appointment.PatientId);
        ViewBag.DoctorId = new SelectList(_context.Doctors, "Id", "Name", appointment.DoctorId);
        ViewBag.StatusList = new SelectList(new[] { "Pending", "Completed", "Cancelled" }, appointment.Status);
        return View(appointment);
    }
    // GET: APPOINTMENTS/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var appointment = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefault(m => m.Id == id);

        if (appointment == null)
        {
            return NotFound();
        }

        return View(appointment);
    }

    // POST: APPOINTMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
   
    public ActionResult DeleteConfirmed(int? id)
    { 
        var appointment =  _context.Appointments.Find(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
        }

         _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private bool AppointmentExists(int? id)
    {
        return _context.Appointments.Any(e => e.Id == id);
    }
}
