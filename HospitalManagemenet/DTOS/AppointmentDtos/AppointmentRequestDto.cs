using System.ComponentModel.DataAnnotations;

namespace HospitalManagemenet.DTOs.Appointment
{
    public class AppointmentRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Patient is required.")]
        public int PatientId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Doctor is required.")]
        public int DoctorId { get; set; }


        [Required(ErrorMessage = "Appointment date is required.")]
        public DateTime AppointmentDate { get; set; }

    
    }
}