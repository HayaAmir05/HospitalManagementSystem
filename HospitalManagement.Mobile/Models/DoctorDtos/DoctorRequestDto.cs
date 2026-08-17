using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Mobile.Models.DoctorDtos
{
    public class DoctorRequestDto
    {

        public string Name { get; set; } = "";
        public string? Email { get; set; }
        public int Age { get; set; }
        public string Contact { get; set; } = "";
        public string Specialization { get; set; } = "";
        public int Experience { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
