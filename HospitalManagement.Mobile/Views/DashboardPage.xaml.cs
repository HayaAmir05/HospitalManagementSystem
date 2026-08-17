using HospitalManagement.Mobile.Views.Appointment;
using HospitalManagement.Mobile.Views.Patient;

namespace HospitalManagement.Mobile;

[QueryProperty(nameof(UserName), "userName")]
[QueryProperty(nameof(Role), "role")]
public partial class DashboardPage : ContentPage
{
    public string UserName { get; set; } = "";
    public string Role { get; set; } = "";

    public DashboardPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        UserNameLabel.Text =
            string.IsNullOrWhiteSpace(UserName)
                ? "User"
                : UserName;

        UserRoleLabel.Text =
            string.IsNullOrWhiteSpace(Role)
                ? "User"
                : Role;
    }

    private async void OnDoctorsTapped(
        object? sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(DoctorsPage));
    }

    private async void OnPatientsTapped(
        object? sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(PatientsPage));
    }

    private async void OnAppointmentsTapped(
        object? sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(AppointmentsPage));
    }

    private async void OnLogoutClicked(
        object? sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"//{nameof(LoginPage)}");
    }
}