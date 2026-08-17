using HospitalManagemenet.DTOs.Doctor;
using HospitalManagemenet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagemenet.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DoctorApiController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorApiController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }


        //GET    /api/doctor   

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var doctors = await _doctorService.GetAllDoctorsAsync();

                return Ok(doctors);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }



        //GET    /api/doctor/{id} 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                if (doctor == null)
                    return NotFound();

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                // TODO: Log the exception later

                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        //POST   /api/doctor 
        [HttpPost]
        public async Task<IActionResult> Create(DoctorRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new DoctorOperationResponseDto
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

                if (request.Age < 23 || request.Age > 80)
                {
                    return BadRequest(new DoctorOperationResponseDto
                    {
                        Success = false,
                        Errors = new Dictionary<string, string[]>
                        {
                            {
                                "Age",
                                new[] { "Doctor's age must be between 23 and 80." }
                            }
                        }
                    });
                }

                var result = await _doctorService.CreateDoctorAsync(request);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new DoctorOperationResponseDto
                {
                    Success = false,
                    Message = "An unexpected error occurred."
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DoctorRequestDto request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(new DoctorOperationResponseDto
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


                if (request.Age < 23 || request.Age > 80)
                {
                    return BadRequest(new DoctorOperationResponseDto
                    {
                        Success = false,
                        Errors = new Dictionary<string, string[]>
                        {
                            {
                                "Age",
                                new[] { "Doctor's age must be between 23 and 80." }
                            }
                        }
                    });
                }

                var result = await _doctorService.UpdateDoctorAsync(id, request);

                if (!result.Success)
                {
                    if (result.Message == "Doctor not found.")
                        return NotFound(result);

                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new DoctorOperationResponseDto
                {
                    Success = false,
                    Message = "An unexpected error occurred."
                });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _doctorService.DeleteDoctorAsync(id);

                if (!result.Success)
                    return NotFound(result);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new DoctorOperationResponseDto
                {
                    Success = false,
                    Message = "An unexpected error occurred."
                });
            }
        }


    }
}
