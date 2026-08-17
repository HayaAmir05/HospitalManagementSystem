using HospitalManagement.Mobile.Models.PatientDtos;
using HospitalManagement.Mobile.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HospitalManagement.Mobile.Services;

public class PatientService : IPatientService
{
    private readonly HttpClient _httpClient;

    public PatientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var token = Preferences.Get("auth_token", string.Empty);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

    }

    public async Task<List<PatientResponseDto>> GetAllPatientsAsync()
    {
        var patients =
            await _httpClient.GetFromJsonAsync<List<PatientResponseDto>>(
                "api/PatientApi");

        return patients ?? new List<PatientResponseDto>();
    }

    public async Task<PatientResponseDto?> GetPatientByIdAsync(int id)
    {
        var response =
            await _httpClient.GetAsync($"api/PatientApi/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content
            .ReadFromJsonAsync<PatientResponseDto>();
    }

    public async Task<PatientOperationResponseDto> CreatePatientAsync(
        PatientRequestDto request)
    {
        var response =
            await _httpClient.PostAsJsonAsync(
                "api/PatientApi",
                request);

        var result =
            await response.Content
                .ReadFromJsonAsync<PatientOperationResponseDto>();

        return result ??
               new PatientOperationResponseDto
               {
                   Success = false,
                   Message = "No response received."
               };
    }

    public async Task<PatientOperationResponseDto> UpdatePatientAsync(
        int id,PatientRequestDto request)
    {
        var response =
            await _httpClient.PutAsJsonAsync(
                $"api/PatientApi/{id}",
                request);

        var result =
            await response.Content
                .ReadFromJsonAsync<PatientOperationResponseDto>();

        return result ??
               new PatientOperationResponseDto
               {
                   Success = false,
                   Message = "No response received."
               };
    }

    public async Task<PatientOperationResponseDto> DeletePatientAsync(
        int id)
    {
        var response =
            await _httpClient.DeleteAsync(
                $"api/PatientApi/{id}");

        var result =
            await response.Content
                .ReadFromJsonAsync<PatientOperationResponseDto>();

        return result ??
               new PatientOperationResponseDto
               {
                   Success = false,
                   Message = "No response received."
               };
    }
}