namespace HospitalManagemenet.DTOs.Patient
{
    public class PatientOperationResponseDto
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public Dictionary<string, string[]>? Errors { get; set; }
    }
}