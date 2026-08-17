using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Mobile.Models.DoctorDtos
{
    public class DoctorOperationResponseDto
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
