using System.ComponentModel.DataAnnotations;

namespace HospitalManagemenet.Models
{
    public class Doctor : Person
    {
        [Required(ErrorMessage ="Specialization is required")]
        public string Specialization { get; set; } = null!;

        [Required(ErrorMessage = "Experience is required")]
        [Range(0,int.MaxValue, ErrorMessage =  "Experiance should be a positive number")]
        public int Experience { get; set; }

        // Navigation Property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
