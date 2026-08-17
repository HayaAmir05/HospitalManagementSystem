using System.ComponentModel.DataAnnotations;

namespace HospitalManagemenet.DTOS
{
    public class PersonDto
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[A-Za-z\s.'-]{2,50}$",
            ErrorMessage = "Name must contain only letters and be between 2 and 50 characters.")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(0, 130, ErrorMessage = "Age must be between 0 and 130.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        [RegularExpression(@"^(03\d{9}|\+923\d{9})$",
            ErrorMessage = "Enter a valid Pakistani number (e.g., 03001234567 or +923001234567).")]
        public string Contact { get; set; } = null!;
    }
}