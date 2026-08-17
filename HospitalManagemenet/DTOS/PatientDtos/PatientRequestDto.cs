using HospitalManagemenet.DTOs;
using HospitalManagemenet.DTOS;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagemenet.DTOs.Patient
{
    public class PatientRequestDto : PersonDto
    {
        [Required(ErrorMessage = "Disease is required")]
        public string Disease { get; set; } = null!;

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = null!;
    }
}