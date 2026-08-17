using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HospitalManagement.Mobile.Models
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[A-Za-z\s.'-]{2,50}$",
        ErrorMessage = "Name must contain only letters and be between 2 and 50 characters.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@hosp\.com$",
            ErrorMessage = "Email must be a valid hospital email address.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and contain uppercase, lowercase and a number.")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Contact is required")]
        [RegularExpression(@"^(03\d{9}|\+923\d{9})$",
            ErrorMessage = "Enter a valid Pakistani number.")]
        public string Contact { get; set; } = "";

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = "";

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
