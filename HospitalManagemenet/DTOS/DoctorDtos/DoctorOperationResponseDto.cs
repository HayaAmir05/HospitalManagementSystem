namespace HospitalManagemenet.DTOs.Doctor
{
    public class DoctorOperationResponseDto
    {

        public bool Success { get; set; }

        public string? Message { get; set; }

        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
