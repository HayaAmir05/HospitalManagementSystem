using System.ComponentModel.DataAnnotations;

namespace HospitalManagemenet.DTOS.Appointment
{
    public class AppointmentUpdateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Patient is required.")]
        public int PatientId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Doctor is required.")]
        public int DoctorId { get; set; }


        [Required(ErrorMessage = "Appointment date is required.")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(Pending|Completed|Cancelled)$", ErrorMessage = "Status must be Pending, Completed, or Cancelled.")]
        public string Status { get; set; } = null!;
    }
}
