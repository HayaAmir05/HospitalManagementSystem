using HospitalManagement.Mobile.Models.AppointmentDtos;
using HospitalManagement.Mobile.Services.Interfaces;

namespace HospitalManagement.Mobile.Views.Appointment;

[QueryProperty(nameof(AppointmentId), "appointmentId")]
public partial class AppointmentDetailsPage : ContentPage
{
    private readonly IAppointmentService _appointmentService;

    private int _appointmentId;

    private bool _isLoading;

    private AppointmentResponseDto? _appointment;


    public string AppointmentId
    {
        get => _appointmentId.ToString();

        set
        {
            if (int.TryParse(value, out int id))
            {
                _appointmentId = id;
            }
        }
    }


    public AppointmentDetailsPage(IAppointmentService appointmentService)
    {
        InitializeComponent();

        _appointmentService = appointmentService;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isLoading)
            return;

        _isLoading = true;

        await LoadAppointment();

        _isLoading = false;
    }


    private async Task LoadAppointment()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;


            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(_appointmentId);


            if (appointment == null)
            {
                await DisplayAlertAsync(
                    "Error",
                    "Appointment not found.",
                    "OK");

                await Shell.Current.GoToAsync("..");

                return;
            }


            _appointment = appointment;


            IdLabel.Text =
                appointment.Id.ToString();

            PatientLabel.Text =
                appointment.PatientName;

            DoctorLabel.Text =
                appointment.DoctorName;

            DateLabel.Text =
                appointment.AppointmentDate.ToString(
                    "dd MMM yyyy");

            StatusLabel.Text =
                appointment.Status;

            CreatedByLabel.Text =
                appointment.CreatedBy;

            CreatedAtLabel.Text =
                appointment.CreatedAt.ToString(
                    "dd MMM yyyy, hh:mm tt");


            // Show overdue warning when a pending appointment
            // has already passed.

            if (appointment.AppointmentDate.Date < DateTime.Today &&
                appointment.Status.Equals(
                    "Pending",
                    StringComparison.OrdinalIgnoreCase))
            {
                OverdueLabel.Text =
                    "⚠ This appointment date has passed and its status is still Pending. Please mark it as Completed or Cancelled.";

                OverdueBorder.IsVisible = true;
            }
            else
            {
                OverdueBorder.IsVisible = false;
            }


            // Display a different status color.

            switch (appointment.Status.ToLower())
            {
                case "pending":
                    StatusLabel.TextColor =
                        Color.FromArgb("#D97706");
                    break;

                case "completed":
                    StatusLabel.TextColor =
                        Color.FromArgb("#15803D");
                    break;

                case "cancelled":
                    StatusLabel.TextColor =
                        Color.FromArgb("#DC2626");
                    break;

                default:
                    StatusLabel.TextColor =
                        Color.FromArgb("#111827");
                    break;
            }
        }
        catch
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to load appointment information.",
                "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }


    private async void Edit_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(EditAppointmentPage)}?appointmentId={_appointmentId}");
    }


    private async void Delete_Clicked(
        object sender,
        EventArgs e)
    {
        bool confirm =
            await DisplayAlertAsync(
                "Delete Appointment",
                "Are you sure you want to delete this appointment?",
                "Delete",
                "Cancel");

        if (!confirm)
            return;


        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;


            var result =
                await _appointmentService
                    .DeleteAppointmentAsync(_appointmentId);


            if (!result.Success)
            {
                await DisplayAlertAsync(
                    "Unable to Delete",
                    result.Message ??
                    "Unable to delete appointment.",
                    "OK");

                return;
            }


            await DisplayAlertAsync(
                "Success",
                result.Message ??
                "Appointment deleted successfully.",
                "OK");


            await Shell.Current.GoToAsync(
                $"//{nameof(AppointmentsPage)}");
        }
        catch
        {
            await DisplayAlertAsync(
                "Error",
                "An unexpected error occurred.",
                "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }


    private async void Back_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}