namespace HospitalManagemenet.Models
{
    public class Doctor : Person
    {
        public string Specialization { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Experience { get; set; }

        // Navigation Property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
