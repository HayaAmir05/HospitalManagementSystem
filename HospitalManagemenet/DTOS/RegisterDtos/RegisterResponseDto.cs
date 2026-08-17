namespace HospitalManagemenet.DTOS.RegisterDtos
{
    public class RegisterResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; } = "";
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }
}