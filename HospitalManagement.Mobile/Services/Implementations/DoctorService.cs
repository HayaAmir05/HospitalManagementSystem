using HospitalManagement.Mobile.Models.DoctorDtos;
using HospitalManagement.Mobile.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HospitalManagement.Mobile.Services;

public class DoctorService : IDoctorService
{
    private readonly HttpClient _httpClient;
    

    public DoctorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var token = Preferences.Get("auth_token", string.Empty);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

    }

    public async Task<List<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _httpClient.GetFromJsonAsync<List<DoctorDto>>("api/DoctorApi");
        return doctors ?? new List<DoctorDto>();

    }
    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/DoctorApi/{id}");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<DoctorDto>();

    }

    public async Task<DoctorOperationResponseDto> CreateDoctorAsync(DoctorRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/DoctorApi", request);
        var result = await response.Content.ReadFromJsonAsync<DoctorOperationResponseDto>();
        return result ?? new DoctorOperationResponseDto { Success = false, Message = "No response received." };
    }

    public async Task<DoctorOperationResponseDto> UpdateDoctorAsync(int id, DoctorRequestDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/DoctorApi/{id}", request);
        var result = await response.Content.ReadFromJsonAsync<DoctorOperationResponseDto>();
        return result ?? new DoctorOperationResponseDto { Success = false, Message = "No response received." };
    }

    public async Task<DoctorOperationResponseDto> DeleteDoctorAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/DoctorApi/{id}");
        var result = await response.Content.ReadFromJsonAsync<DoctorOperationResponseDto>();
        return result ?? new DoctorOperationResponseDto { Success = false, Message = "No response received." };
    }






}