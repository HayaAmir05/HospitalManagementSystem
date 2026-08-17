using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Mobile.Models
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; } = "";

        public string? UserName { get; set; } = "";

        public string? Role { get; set; } = "";
        public string? Token { get; set; }
    }
}
