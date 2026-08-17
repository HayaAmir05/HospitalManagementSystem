using HospitalManagement.Mobile.Models;
using HospitalManagement.Mobile.Services.Interfaces;
using System.ComponentModel.Design;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HospitalManagement.Mobile.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            var token = Preferences.Get("auth_token", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/login",
                request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDto>()
                       ?? new LoginResponseDto
                       {
                           Success = false,
                           Message = "No response received."
                       };
            }

            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/register",
                request);

            return await response.Content.ReadFromJsonAsync<RegisterResponseDto>()
                ?? new RegisterResponseDto
                {
                    Success = false,
                    Message = "No response received."
                };


        }
    }
}