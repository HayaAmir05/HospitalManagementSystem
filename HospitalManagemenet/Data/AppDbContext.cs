using HospitalManagemenet.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagemenet.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext>options):DbContext(options)
    {
        // public DbSet<User> Users { get; set; }
        //{

        public DbSet<User> Users { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
    
}
}
