using HospitalManagemenet.DTOS.LoginDtos;
using HospitalManagemenet.DTOS.RegisterDtos;
using HospitalManagemenet.Services.Implementations;
using HospitalManagemenet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagemenet.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtService _jwtService;
        public AuthApiController(IAuthService authService, JwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userInDb = await _authService.ValidateUserAsync(request.Email, request.Password);

            if (userInDb == null)
            {
                return Unauthorized(new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }
            var token = _jwtService.GenerateToken(userInDb);

            return Ok(new LoginResponseDto
            {
                Success = true,
                UserName = userInDb.Name,
                Role = userInDb.Role,
                Token = token
            });
        }
       
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RegisterResponseDto
                {
                    Success = false,
                    Message = "Validation failed.",
                    Errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Value.Errors
                                .Select(e => e.ErrorMessage)
                                .ToArray())
                });
            }

            bool success = await _authService.RegisterAsync(request);

            if (!success)
            {
                return Conflict(new RegisterResponseDto
                {
                    Success = false,
                    Message = "Email already exists"
                });
            }

            return Ok(new RegisterResponseDto
            {
                Success = true,
                Message = "User registered successfully "
            });

        }
    }
}
