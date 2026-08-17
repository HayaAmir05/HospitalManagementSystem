using HospitalManagemenet.Data;
using HospitalManagemenet.DTOs.Patient;
using HospitalManagemenet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagemenet.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientApiController(AppDbContext context)
        {
            _context = context;
        }


        // GET: /api/PatientApi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var patients = await _context.Patients
                    .Select(p => new PatientResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Email = p.Email,
                        Age = p.Age,
                        Contact = p.Contact,
                        Disease = p.Disease,
                        Address = p.Address,
                        createdBy = p.createdBy,
                        CreatedAt = p.CreatedAt
                    })
                    .ToListAsync();

                return Ok(patients);
            }
            catch
            {
                return StatusCode(
                    500,
                    "An unexpected error occurred.");
            }
        }


        // GET: /api/PatientApi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var patient = await _context.Patients
                    .Select(p => new PatientResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Email = p.Email,
                        Age = p.Age,
                        Contact = p.Contact,
                        Disease = p.Disease,
                        Address = p.Address,
                        createdBy = p.createdBy,
                        CreatedAt = p.CreatedAt
                    })
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (patient == null)
                    return NotFound();

                return Ok(patient);
            }
            catch
            {
                return StatusCode(
                    500,
                    "An unexpected error occurred.");
            }
        }


        // POST: /api/PatientApi
        [HttpPost]
        public async Task<IActionResult> Create(PatientRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        new PatientOperationResponseDto
                        {
                            Success = false,

                            Errors = ModelState
                                .Where(x => x.Value!.Errors.Count > 0)
                                .ToDictionary(
                                    x => x.Key,
                                    x => x.Value!.Errors
                                        .Select(e => e.ErrorMessage)
                                        .ToArray())
                        });
                }

                var patient = new Patient
                {
                    Name = request.Name,
                    Email = request.Email,
                    Age = request.Age,
                    Contact = request.Contact,
                    Disease = request.Disease,
                    Address = request.Address,

                    createdBy = "API",
                    CreatedAt = DateTime.Now
                };

                _context.Patients.Add(patient);

                await _context.SaveChangesAsync();

                return Ok(
                    new PatientOperationResponseDto
                    {
                        Success = true,
                        Message = "Patient created successfully."
                    });
            }
            catch
            {
                return StatusCode(
                    500,
                    new PatientOperationResponseDto
                    {
                        Success = false,
                        Message = "An unexpected error occurred."
                    });
            }
        }


        // PUT: /api/PatientApi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            PatientRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        new PatientOperationResponseDto
                        {
                            Success = false,

                            Errors = ModelState
                                .Where(x => x.Value!.Errors.Count > 0)
                                .ToDictionary(
                                    x => x.Key,
                                    x => x.Value!.Errors
                                        .Select(e => e.ErrorMessage)
                                        .ToArray())
                        });
                }

                var patient = await _context.Patients.FindAsync(id);

                if (patient == null)
                {
                    return NotFound(
                        new PatientOperationResponseDto
                        {
                            Success = false,
                            Message = "Patient not found."
                        });
                }

                patient.Name = request.Name;
                patient.Email = request.Email;
                patient.Age = request.Age;
                patient.Contact = request.Contact;
                patient.Disease = request.Disease;
                patient.Address = request.Address;

                await _context.SaveChangesAsync();

                return Ok(
                    new PatientOperationResponseDto
                    {
                        Success = true,
                        Message = "Patient updated successfully."
                    });
            }
            catch
            {
                return StatusCode(
                    500,
                    new PatientOperationResponseDto
                    {
                        Success = false,
                        Message = "An unexpected error occurred."
                    });
            }
        }


        // DELETE: /api/PatientApi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);

                if (patient == null)
                {
                    return NotFound(
                        new PatientOperationResponseDto
                        {
                            Success = false,
                            Message = "Patient not found."
                        });
                }

                _context.Patients.Remove(patient);

                await _context.SaveChangesAsync();

                return Ok(
                    new PatientOperationResponseDto
                    {
                        Success = true,
                        Message = "Patient deleted successfully."
                    });
            }
            catch
            {
                return StatusCode(
                    500,
                    new PatientOperationResponseDto
                    {
                        Success = false,
                        Message = "An unexpected error occurred."
                    });
            }
        }
    }
}