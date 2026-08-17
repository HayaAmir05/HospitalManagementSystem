using HospitalManagement.Mobile.Views;
using HospitalManagement.Mobile.Views.Appointment;
using HospitalManagement.Mobile.Views.Doctor;
using HospitalManagement.Mobile.Views.Patient;

namespace HospitalManagement.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));


            Routing.RegisterRoute(nameof(DoctorsPage), typeof(DoctorsPage));
            Routing.RegisterRoute(nameof(AddDoctorPage), typeof(AddDoctorPage));
            Routing.RegisterRoute(nameof(EditDoctorPage), typeof(EditDoctorPage));
            Routing.RegisterRoute(nameof(DoctorDetailsPage), typeof(DoctorDetailsPage));

            Routing.RegisterRoute(nameof(PatientsPage), typeof(PatientsPage));
            Routing.RegisterRoute(nameof(PatientDetailsPage), typeof(PatientDetailsPage));
            Routing.RegisterRoute(nameof(EditPatientPage), typeof(EditPatientPage));
            Routing.RegisterRoute(nameof(AddPatientPage), typeof(AddPatientPage));


            Routing.RegisterRoute(nameof(AppointmentsPage), typeof(AppointmentsPage));
            Routing.RegisterRoute(nameof(AddAppointmentPage), typeof(AddAppointmentPage));
            Routing.RegisterRoute(nameof(AppointmentDetailsPage), typeof(AppointmentDetailsPage));
            Routing.RegisterRoute(nameof(EditAppointmentPage), typeof(EditAppointmentPage));



        }
    }
}
