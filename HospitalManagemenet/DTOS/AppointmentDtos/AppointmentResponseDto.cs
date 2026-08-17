namespace HospitalManagemenet.DTOs.Appointment
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; } = null!;

        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = null!;

        public DateTime AppointmentDate { get; set; }

        public string Status { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public bool IsOverdue { get; set; }

        public string? StatusMessage { get; set; }
    }
}