using HospitalManagement.Mobile.Models;
using HospitalManagement.Mobile.Models.DoctorDtos;

namespace HospitalManagement.Mobile.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDto?> GetDoctorByIdAsync(int id);
        Task<DoctorOperationResponseDto> CreateDoctorAsync(DoctorRequestDto request);
        Task<DoctorOperationResponseDto> UpdateDoctorAsync(int id, DoctorRequestDto request);
        Task<DoctorOperationResponseDto> DeleteDoctorAsync(int id);
    }
}