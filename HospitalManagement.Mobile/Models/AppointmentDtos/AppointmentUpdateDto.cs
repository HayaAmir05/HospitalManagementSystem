

namespace HospitalManagement.Mobile.Models.AppointmentDtos;

public class AppointmentUpdateDto
{
    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Status { get; set; } = string.Empty;
}
