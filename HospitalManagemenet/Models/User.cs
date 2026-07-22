using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagemenet.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[A-Za-z\s.'-]{2,50}$",
            ErrorMessage = "Name must contain only letters and be between 2 and 50 characters.")]
        public string Name { get; set; } = null!; 


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@hosp\.com$", ErrorMessage = "Email must be a valid hospital email address")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "Contact is required")]
        [RegularExpression(@"^(03\d{9}|\+923\d{9})$", ErrorMessage = "Enter a valid Pakistani number (e.g., 03001234567 or +923001234567).")]
        public string Contact { get; set; } = null!;

        [NotMapped]
        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;


        public string createdBy { get; set; }
        public DateTime CreatedAt { get; set; }



    }
}
