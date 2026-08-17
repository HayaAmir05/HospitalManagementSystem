using HospitalManagemenet.DTOS;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagemenet.DTOs.Doctor
{
    public class DoctorRequestDto : PersonDto
    {

        [Required(ErrorMessage = "Age is required")]
        [Range(23, 80, ErrorMessage = "Doctor's age must be between 23 and 80.")]
        public new int Age
        {
            get => base.Age;
            set => base.Age = value;
        }


        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; } = null!;

        [Required(ErrorMessage = "Experience is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Experience should be a positive number")]
        public int Experience { get; set; }
    }
}