using HospitalManagemenet.Models;

namespace HospitalManagemenet.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string email, string password);

        Task<bool> RegisterAsync(User user);

        Task<bool> RegisterAsync(RegisterRequestDto request);
    }
}