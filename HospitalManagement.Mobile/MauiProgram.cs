using HospitalManagement.Mobile.Services;
using HospitalManagement.Mobile.Services.Implementation;

using HospitalManagement.Mobile.Services.Interfaces;
using HospitalManagement.Mobile.Views.Appointment;
using HospitalManagement.Mobile.Views.Doctor;
using HospitalManagement.Mobile.Views.Patient;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<App>();
            builder.Services.AddSingleton<AppShell>();


            builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7009/");
            });

            builder.Services.AddHttpClient<IDoctorService, DoctorService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7009/");
            });

            builder.Services.AddHttpClient<IPatientService, PatientService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7009/");
            });

            builder.Services.AddHttpClient<IAppointmentService, AppointmentService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7009/");
            });

            builder.Services.AddTransient<DoctorsPage>();
            builder.Services.AddTransient<DoctorDetailsPage>();
            builder.Services.AddTransient<AddDoctorPage>();
            builder.Services.AddTransient<EditDoctorPage>();



            builder.Services.AddTransient<PatientsPage>();
            builder.Services.AddTransient<PatientDetailsPage>();
            builder.Services.AddTransient<AddPatientPage>();
            builder.Services.AddTransient<EditPatientPage>();

            builder.Services.AddTransient<AppointmentsPage>();
            builder.Services.AddTransient<AddAppointmentPage>();
            builder.Services.AddTransient<AppointmentDetailsPage>();
            builder.Services.AddTransient<EditAppointmentPage>();

            return builder.Build();
        }
    }
}
