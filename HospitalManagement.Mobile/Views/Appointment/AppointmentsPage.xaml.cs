using HospitalManagement.Mobile.Models.AppointmentDtos;
using HospitalManagement.Mobile.Services.Interfaces;

namespace HospitalManagement.Mobile.Views.Appointment;

public partial class AppointmentsPage : ContentPage
{
    private readonly IAppointmentService _appointmentService;

    // Keeps the complete list returned by the API.
    // We search/filter this list instead of repeatedly calling the API.
    private List<AppointmentResponseDto> _allAppointments = new();

    public AppointmentsPage(IAppointmentService appointmentService)
    {
        InitializeComponent();

        _appointmentService = appointmentService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadAppointments();
    }

    private async Task LoadAppointments()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var appointments =
                await _appointmentService.GetAllAppointmentsAsync();

            _allAppointments = appointments.ToList();

            // Apply the current search text to the newly loaded data.
            ApplySearch();
        }
        catch
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to load appointments.",
                "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }

    // Runs whenever the user types in the search bar.
    private void SearchBarAppointments_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        ApplySearch();
    }

    private void ApplySearch()
    {
        string searchText =
            SearchBarAppointments.Text?
                .Trim()
                .ToLower() ?? string.Empty;

        // No search text → show all appointments.
        if (string.IsNullOrWhiteSpace(searchText))
        {
            AppointmentsCollectionView.ItemsSource =
                _allAppointments;

            EmptyLayout.IsVisible =
                _allAppointments.Count == 0;

            return;
        }

        // Search by:
        // 1. Patient name
        // 2. Doctor name
        // 3. Status
        var filteredAppointments =
            _allAppointments
                .Where(a =>
                    (a.PatientName ?? "")
                        .ToLower()
                        .Contains(searchText)

                    ||

                    (a.DoctorName ?? "")
                        .ToLower()
                        .Contains(searchText)

                    ||

                    (a.Status ?? "")
                        .ToLower()
                        .Contains(searchText))
                .ToList();

        AppointmentsCollectionView.ItemsSource =
            filteredAppointments;

        EmptyLayout.IsVisible =
            filteredAppointments.Count == 0;
    }

    // The WHOLE appointment card is clickable.
    private async void AppointmentCard_Tapped(
    object sender,
    TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext is AppointmentResponseDto appointment)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(AppointmentDetailsPage)}?appointmentId={appointment.Id}");
        }
    }
 
    // Pull-to-refresh.
    private async void RefreshAppointments_Refreshing(
        object sender,
        EventArgs e)
    {
        try
        {
            await LoadAppointments();


        }
        finally
        {
            RefreshAppointments.IsRefreshing = false;
        }
    }

    // Add appointment button.
    private async void Add_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(AddAppointmentPage));
    }
}