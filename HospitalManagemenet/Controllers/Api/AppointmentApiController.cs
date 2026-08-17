  using HospitalManagemenet.Data;
using HospitalManagemenet.DTOs.Appointment;
using HospitalManagemenet.DTOS.Appointment;
using HospitalManagemenet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagemenet.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentApiController(AppDbContext context)
        {
            _context = context;
        }


        // GET: /api/AppointmentApi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Select(a => new AppointmentResponseDto
                    {
                        Id = a.Id,

                        PatientId = a.PatientId,
                        PatientName = a.Patient.Name,

                        DoctorId = a.DoctorId,
                        DoctorName = a.Doctor.Name,

                        AppointmentDate = a.AppointmentDate,

                        Status = a.Status,

                        CreatedBy = a.createdBy,
                        CreatedAt = a.CreatedAt
                    })
                    .ToListAsync();

                return Ok(appointments);
            }
            catch
            {
                return StatusCode(500,
                   new AppointmentOperationResponseDto
                {
                    Success = false,
                    Message = "An unexpected error occurred."
                });
            }
        }


        // GET: /api/AppointmentApi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.Id == id)
                    .Select(a => new AppointmentResponseDto
                    {
                        Id = a.Id,

                        PatientId = a.PatientId,
                        PatientName = a.Patient.Name,

                        DoctorId = a.DoctorId,
                        DoctorName = a.Doctor.Name,

                        AppointmentDate = a.AppointmentDate,

                        Status = a.Status,

                        CreatedBy = a.createdBy,
                        CreatedAt = a.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (appointment == null)
                {
                    return NotFound(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,
                            Message = "Appointment not found."
                        });
                }

                return Ok(appointment);
            }
            catch
            {
                return StatusCode(
                    500,
                    new AppointmentOperationResponseDto
                    {
                        Success = false,
                        Message = "An unexpected error occurred."
                    });
            }
        }


        // POST: /api/AppointmentApi
        [HttpPost]
        public async Task<IActionResult> Create(
            AppointmentRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        new AppointmentOperationResponseDto
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


                // Appointment date cannot be in the past

                if (request.AppointmentDate.Date < DateTime.Today)
                {
                    return BadRequest(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,

                            Errors = new Dictionary<string, string[]>
                            {
                                {
                                    "AppointmentDate",
                                    new[]
                                    {
                                        "Appointment date cannot be in the past."
                                    }
                                }
                            }
                        });
                }


                // Check patient exists

                var patient =
                    await _context.Patients
                        .FindAsync(request.PatientId);

                if (patient == null)
                {
                    return BadRequest(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,

                            Errors = new Dictionary<string, string[]>
                            {
                                {
                                    "PatientId",
                                    new[]
                                    {
                                        "Selected patient does not exist."
                                    }
                                }
                            }
                        });
                }


                // Check doctor exists

                var doctor =
                    await _context.Doctors
                        .FindAsync(request.DoctorId);

                if (doctor == null)
                {
                    return BadRequest(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,

                            Errors = new Dictionary<string, string[]>
                            {
                                {
                                    "DoctorId",
                                    new[]
                                    {
                                        "Selected doctor does not exist."
                                    }
                                }
                            }
                        });
                }


                var appointment = new Appointment
                {
                    PatientId = request.PatientId,

                    DoctorId = request.DoctorId,

                    AppointmentDate =
                        request.AppointmentDate,

                    // Always Pending when created
                    Status = "Pending",

                    createdBy = "API",

                    CreatedAt = DateTime.Now
                };


                _context.Appointments.Add(appointment);

                await _context.SaveChangesAsync();


                return Ok(
                    new AppointmentOperationResponseDto
                    {
                        Success = true,
                        Message = "Appointment created successfully."
                    });
            }
            catch
            {
                return StatusCode(
                    500,
                    new AppointmentOperationResponseDto
                    {
                        Success = false,
                        Message = "An unexpected error occurred."
                    });
            }
        }


        // PUT: /api/AppointmentApi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            AppointmentUpdateDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        new AppointmentOperationResponseDto
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


                var appointment =
                    await _context.Appointments
                        .FindAsync(id);

                if (appointment == null)
                {
                    return NotFound(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,
                            Message = "Appointment not found."
                        });
                }


                // Date cannot be moved backwards

                if (request.AppointmentDate.Date <
                    appointment.AppointmentDate.Date)
                {
                    return BadRequest(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,

                            Errors = new Dictionary<string, string[]>
                            {
                                {
                                    "AppointmentDate",
                                    new[]
                                    {
                                        "Appointment date cannot be moved to an earlier date. You can only delay the appointment."
                                    }
                                }
                            }
                        });
                }


                // Past appointment cannot remain Pending

                if (request.AppointmentDate.Date < DateTime.Today &&
                    request.Status == "Pending")
                {
                    return BadRequest(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,

                            Errors = new Dictionary<string, string[]>
                            {
                                {
                                    "Status",
                                    new[]
                                    {
                                        "Past appointments cannot remain Pending. Mark the appointment as Completed or Cancelled."
                                    }
                                }
                            }
                        });
                }

                //Future appointments cannot be Completed
                if (request.AppointmentDate.Date > DateTime.Today && request.Status == "Completed")
                {
                    return BadRequest(new AppointmentOperationResponseDto
                    {
                        Success = false,
                        Errors = new Dictionary<string, string[]>
                        {
                            {
                                "Status",
                                new[]
                                {
                                    "Future appointments cannot be marked as Completed. You can only mark them as Pending or Cancelled."
                                }

                            }
                        }
                    });
                }


                // Check patient exists

                var patient =
                    await _context.Patients
                        .FindAsync(request.PatientId);

                if (patient == null)
                {
                    return BadRequest(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,

                            Errors = new Dictionary<string, string[]>
                            {
                                {
                                    "PatientId",
                                    new[]
                                    {
                                        "Selected patient does not exist."
                                    }
                                }
                            }
                        });
                }


                // Check doctor exists

                var doctor =
                    await _context.Doctors
                        .FindAsync(request.DoctorId);

                if (doctor == null)
                {
                    return BadRequest(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,

                            Errors = new Dictionary<string, string[]>
                            {
                                {
                                    "DoctorId",
                                    new[]
                                    {
                                        "Selected doctor does not exist."
                                    }
                                }
                            }
                        });
                }


                appointment.PatientId =
                    request.PatientId;

                appointment.DoctorId =
                    request.DoctorId;

                appointment.AppointmentDate =
                    request.AppointmentDate;

                appointment.Status =
                    request.Status;


                await _context.SaveChangesAsync();


                return Ok(
                    new AppointmentOperationResponseDto
                    {
                        Success = true,
                        Message = "Appointment updated successfully."
                    });
            }
            catch
            {
                return StatusCode(
                    500,
                    new AppointmentOperationResponseDto
                    {
                        Success = false,
                        Message = "An unexpected error occurred."
                    });
            }
        }


        // DELETE: /api/AppointmentApi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var appointment =
                    await _context.Appointments
                        .FindAsync(id);

                if (appointment == null)
                {
                    return NotFound(
                        new AppointmentOperationResponseDto
                        {
                            Success = false,
                            Message = "Appointment not found."
                        });
                }


                _context.Appointments.Remove(appointment);

                await _context.SaveChangesAsync();


                return Ok(
                    new AppointmentOperationResponseDto
                    {
                        Success = true,
                        Message = "Appointment deleted successfully."
                    });
            }
            catch
            {
                return StatusCode(
                    500,
                    new AppointmentOperationResponseDto
                    {
                        Success = false,
                        Message = "An unexpected error occurred."
                    });
            }
        }
    }
}