namespace HospitalManagement.Mobile.Models.PatientDtos;

public class PatientRequestDto
{
    public string Name { get; set; } = "";
    public string? Email { get; set; }
    public int Age { get; set; }
    public string Contact { get; set; } = "";

    public string Disease { get; set; } = "";
    public string Address { get; set; } = "";
}