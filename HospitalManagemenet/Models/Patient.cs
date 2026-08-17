using System.ComponentModel.DataAnnotations;

namespace HospitalManagemenet.Models
{
    public class Patient : Person
    {
        [Required(ErrorMessage = "Disease is required")]
        public string Disease { get; set; } = null!;


        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = null!;

        // Navigation Property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
