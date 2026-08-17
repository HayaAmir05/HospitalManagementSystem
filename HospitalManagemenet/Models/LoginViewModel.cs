using System.ComponentModel.DataAnnotations;

namespace HospitalManagemenet.Models
{
    public class LoginViewModel
    {


        [Required(ErrorMessage= "Hospital email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage="Password is required")]  
        public string Password { get; set; } = null!;
    }
}
