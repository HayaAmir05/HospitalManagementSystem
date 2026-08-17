
using HospitalManagement.Mobile.Models.AppointmentDtos;
using HospitalManagement.Mobile.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HospitalManagement.Mobile.Services;

public class AppointmentService : IAppointmentService
{
    private readonly HttpClient _httpClient;

    public AppointmentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var token = Preferences.Get("auth_token", string.Empty);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<AppointmentResponseDto>> GetAllAppointmentsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AppointmentResponseDto>>("api/AppointmentApi")
               ?? new List<AppointmentResponseDto>();
    }

    public async Task<AppointmentResponseDto?> GetAppointmentByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/AppointmentApi/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<AppointmentResponseDto>();
    }


    public async Task<AppointmentOperationResponseDto> CreateAppointmentAsync(AppointmentRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/AppointmentApi", request);

        var result = await response.Content.ReadFromJsonAsync<AppointmentOperationResponseDto>();

        return result ?? new AppointmentOperationResponseDto
        {
            Success = false,
            Message = "Unable to create appointment."
        };
    }

    public async Task<AppointmentOperationResponseDto> UpdateAppointmentAsync(int id, AppointmentUpdateDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/AppointmentApi/{id}", request);

        var result = await response.Content.ReadFromJsonAsync<AppointmentOperationResponseDto>();

        return result ?? new AppointmentOperationResponseDto
        {
            Success = false,
            Message = "Unable to update appointment."
        };
    }


    public async Task<AppointmentOperationResponseDto> DeleteAppointmentAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/AppointmentApi/{id}");

        var result = await response.Content.ReadFromJsonAsync<AppointmentOperationResponseDto>();

        return result ?? new AppointmentOperationResponseDto
        {
            Success = false,
            Message = "Unable to delete appointment."
        };
    }
}