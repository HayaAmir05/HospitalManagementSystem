using HospitalManagemenet.Data;
using HospitalManagemenet.Models;
using HospitalManagemenet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagemenet.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(
                u => u.Email == email &&
                     u.Password == password);
        }

        public async Task<bool> RegisterAsync(User user)
        {
            bool exists = await _context.Users
                .AnyAsync(u => u.Email == user.Email);

            if (exists)
                return false;

            user.createdBy = "Self-Registered";
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            bool exists = await _context.Users
                .AnyAsync(u => u.Email == request.Email);

            if (exists)
                return false;

            User user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role,
                Contact = request.Contact,

                CreatedAt = DateTime.Now,
                createdBy = "Self-Registered"
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }

    }
}