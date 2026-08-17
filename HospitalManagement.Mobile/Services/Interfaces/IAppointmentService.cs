using HospitalManagement.Mobile.Models.AppointmentDtos;

namespace HospitalManagement.Mobile.Services.Interfaces;

public interface IAppointmentService
{
    Task<List<AppointmentResponseDto>> GetAllAppointmentsAsync();

    Task<AppointmentResponseDto?> GetAppointmentByIdAsync(int id);

    Task<AppointmentOperationResponseDto> CreateAppointmentAsync(AppointmentRequestDto request);

    Task<AppointmentOperationResponseDto> UpdateAppointmentAsync(int id, AppointmentUpdateDto request);

    Task<AppointmentOperationResponseDto> DeleteAppointmentAsync(int id);
}