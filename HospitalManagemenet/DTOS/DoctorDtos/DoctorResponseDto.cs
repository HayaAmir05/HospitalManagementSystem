using HospitalManagemenet.DTOS;

namespace HospitalManagemenet.DTOs.Doctor
{
    public class DoctorResponseDto : PersonDto
    {
        public int Id { get; set; }

        public string Specialization { get; set; } = null!;

        public int Experience { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}