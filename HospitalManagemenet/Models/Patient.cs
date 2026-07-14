namespace HospitalManagemenet.Models
{
    public class Patient : Person
    {
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string Disease { get; set; } = null!;
        public string Address { get; set; } = null!;
       
        // Navigation Property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
