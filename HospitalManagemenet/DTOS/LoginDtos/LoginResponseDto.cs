namespace HospitalManagemenet.DTOS.LoginDtos
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? UserName { get; set; }

        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}
