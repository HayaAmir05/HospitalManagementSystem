using HospitalManagemenet.DTOS;

public class PatientResponseDto : PersonDto
{
    public int Id { get; set; }
    public string Disease { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string createdBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}