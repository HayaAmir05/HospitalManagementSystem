
using HospitalManagemenet.Data;
using HospitalManagemenet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

public class PatientsController : Controller
{
    private readonly AppDbContext _context;

    public PatientsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PATIENTS
    public ActionResult Index()    
    {
        return View( _context.Patients.ToList());
    }

    // GET: PATIENTS/Details/5
    public ActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var patient =  _context.Patients
            .FirstOrDefault(m => m.Id == id);
        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }

    // GET: PATIENTS/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: PATIENTS/Create
   
    [HttpPost]
   
    public ActionResult Create(Patient patient)
    {
        try
        {
            
            patient.createdBy = HttpContext.Session.GetString("UserName") ?? "System";
            patient.CreatedAt = DateTime.Now;

            // Because these are set AFTER model binding/validation already ran,
            // remove any stale validation errors that were recorded for them
            ModelState.Remove("createdBy");
            ModelState.Remove("CreatedAt");

            if (patient.Age < 0 || patient.Age > 130)
            {
                ModelState.AddModelError("Age", "Patient's age must be between 0 and 130.");
            }

            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                ModelState.AddModelError("Email", "Email is required for patients.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(patient);
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
            ModelState.AddModelError(string.Empty, "An error occurred while creating the patient: " + ex.Message);
        }
        return View(patient);
    }

    // GET: PATIENTS/Edit/5
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var patient =  _context.Patients.Find(id);
        if (patient == null)
        {
            return NotFound();
        }
        return View(patient);
    }

    // POST: PATIENTS/Edit/5
   
    [HttpPost]

    public ActionResult Edit(int? id,Patient patient)
    {
        if (id != patient.Id)
        {
            return NotFound();
        }

        Patient patientInDb = _context.Patients.AsNoTracking().FirstOrDefault(p => p.Id == patient.Id);
        patient.createdBy = patientInDb.createdBy;
        patient.CreatedAt = patientInDb.CreatedAt;

        ModelState.Remove("createdBy");
        ModelState.Remove("CreatedAt");

        if (patient.Age < 0 || patient.Age > 130)
        {
            ModelState.AddModelError("Age", "Patient's age must be between 0 and 130.");
        }

        if (string.IsNullOrWhiteSpace(patient.Email))
        {
            ModelState.AddModelError("Email", "Email is required for patients.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(patient);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(patient.Id))
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
        return View(patient);
    }

    // GET: PATIENTS/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var patient =  _context.Patients
            .FirstOrDefault(m => m.Id == id);
        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }

    // POST: PATIENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    
    public ActionResult DeleteConfirmed(int? id)
    {
        var patient =  _context.Patients.Find(id);
        if (patient != null)
        {
            _context.Patients.Remove(patient);
        }

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    } 

    private bool PatientExists(int? id)
    {
        return _context.Patients.Any(e => e.Id == id);
    }
}
