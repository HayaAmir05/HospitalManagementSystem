
using HospitalManagement.Mobile.Models;
namespace HospitalManagement.Mobile.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
    }

}
