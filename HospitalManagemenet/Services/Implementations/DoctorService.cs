using HospitalManagemenet.Data;
using HospitalManagemenet.DTOs.Doctor;
using HospitalManagemenet.Models;
using HospitalManagemenet.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagemenet.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;

        public DoctorService(AppDbContext context)
        {
            _context = context;
        }


        //All Doctors
        public async Task<List<DoctorResponseDto>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
       .Select(d => new DoctorResponseDto
       {
           Id = d.Id,
           Name = d.Name,
           Email = d.Email,
           Age = d.Age,
           Contact = d.Contact,
           Specialization = d.Specialization,
           Experience = d.Experience,
           CreatedBy = d.createdBy,
           CreatedAt = d.CreatedAt
       })
       .ToListAsync();
        }


        //One Doctor by Id
        public async Task<DoctorResponseDto?> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctors
                .Select(d => new DoctorResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Email = d.Email,
                    Age = d.Age,
                    Contact = d.Contact,
                    Specialization = d.Specialization,
                    Experience = d.Experience,
                    CreatedBy = d.createdBy,
                    CreatedAt = d.CreatedAt
                })
                .FirstOrDefaultAsync(d => d.Id == id);
        }


        public async Task<DoctorOperationResponseDto> CreateDoctorAsync(DoctorRequestDto request)
        {
            
            if (request.Age < 23 || request.Age > 80)
            {
                return new DoctorOperationResponseDto
                {
                    Success = false,
                    Errors = new Dictionary<string, string[]>
                    {
                        { "Age", new[] { "Doctor's age must be between 23 and 80." } }
                    }
                };
            }

            var doctor = new Doctor
            {
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                Contact = request.Contact,
                Specialization = request.Specialization,
                Experience = request.Experience,
                createdBy = "API",
                CreatedAt = DateTime.Now
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return new DoctorOperationResponseDto
            {
                Success = true,
                Message = "Doctor created successfully."
            };
        }

        public async Task<DoctorOperationResponseDto> UpdateDoctorAsync(int id,DoctorRequestDto request)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return new DoctorOperationResponseDto
                {
                    Success = false,
                    Message = "Doctor not found."
                };
            }

            if (request.Age < 23 || request.Age > 80)
            {
                return new DoctorOperationResponseDto
                {
                    Success = false,
                    Errors = new Dictionary<string, string[]>
                    {
                        {"Age", new[] { "Doctor's age must be between 23 and 80." } }
                     }
                };
            }

            doctor.Name = request.Name;
            doctor.Email = request.Email;
            doctor.Age = request.Age;
            doctor.Contact = request.Contact;
            doctor.Specialization = request.Specialization;
            doctor.Experience = request.Experience;

            await _context.SaveChangesAsync();

            return new DoctorOperationResponseDto
            {
                Success = true,
                Message = "Doctor updated successfully."
            };
        }


        public async Task<DoctorOperationResponseDto> DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            { 
                return new DoctorOperationResponseDto
                {
                    Success = false,
                    Message = "Doctor not found."
                };
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return new DoctorOperationResponseDto
            {
                Success = true,
                Message = "Doctor deleted successfully."
            };
        }
    }
}
