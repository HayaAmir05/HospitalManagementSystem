namespace HospitalManagement.Mobile.Models.AppointmentDtos;

public class AppointmentRequestDto
{
    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Status { get; set; } = string.Empty;
}