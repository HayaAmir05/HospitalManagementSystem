using System.ComponentModel.DataAnnotations;

namespace HospitalManagemenet.DTOS.LoginDtos
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Hospital email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = "";
    }
}
