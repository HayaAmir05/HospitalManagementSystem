namespace HospitalManagement.Mobile.Models.AppointmentDtos;

public class AppointmentOperationResponseDto
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public Dictionary<string, string[]> Errors { get; set; } = new();
}