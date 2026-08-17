using HospitalManagemenet.DTOs.Doctor;


namespace HospitalManagemenet.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorResponseDto>> GetAllDoctorsAsync();

        Task<DoctorResponseDto?> GetDoctorByIdAsync(int id);

        Task<DoctorOperationResponseDto> CreateDoctorAsync(DoctorRequestDto request);

        Task<DoctorOperationResponseDto> UpdateDoctorAsync(int id, DoctorRequestDto request);

        Task<DoctorOperationResponseDto> DeleteDoctorAsync(int id);
    }
}
