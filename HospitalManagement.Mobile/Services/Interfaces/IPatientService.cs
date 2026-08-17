using HospitalManagement.Mobile.Models.PatientDtos;

namespace HospitalManagement.Mobile.Services.Interfaces;

public interface IPatientService
{
    Task<List<PatientResponseDto>> GetAllPatientsAsync();

    Task<PatientResponseDto?> GetPatientByIdAsync(int id);

    Task<PatientOperationResponseDto> CreatePatientAsync(
        PatientRequestDto request);

    Task<PatientOperationResponseDto> UpdatePatientAsync(
        int id,
        PatientRequestDto request);

    Task<PatientOperationResponseDto> DeletePatientAsync(int id);
}