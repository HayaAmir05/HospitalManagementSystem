using HospitalManagement.Mobile.Models.AppointmentDtos;
using HospitalManagement.Mobile.Models.DoctorDtos;
using HospitalManagement.Mobile.Models.PatientDtos;
using HospitalManagement.Mobile.Services.Interfaces;

namespace HospitalManagement.Mobile.Views.Appointment;

public partial class AddAppointmentPage : ContentPage
{
    private readonly IAppointmentService _appointmentService;
    private readonly IPatientService _patientService;
    private readonly IDoctorService _doctorService;

    private int? _selectedPatientId;
    private int? _selectedDoctorId;

    public AddAppointmentPage(IAppointmentService appointmentService, IPatientService patientService, IDoctorService doctorService)
    {
        InitializeComponent();

        _appointmentService = appointmentService;
        _patientService = patientService;
        _doctorService = doctorService;

        AppointmentDatePicker.MinimumDate = DateTime.Today;
        AppointmentDatePicker.Date = DateTime.Today;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPatientsAndDoctors();
    }

    private async Task LoadPatientsAndDoctors()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var patients = await _patientService.GetAllPatientsAsync();
            var doctors = await _doctorService.GetAllDoctorsAsync();

            PatientPicker.ItemsSource = patients;
            DoctorPicker.ItemsSource = doctors;
        }
        catch
        {
            await DisplayAlertAsync("Error", "Unable to load patients and doctors.", "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }

    private void PatientPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PatientPicker.SelectedItem is PatientResponseDto patient)
        {
            _selectedPatientId = patient.Id;
            PatientErrorLabel.IsVisible = false;
        }
    }

    private void DoctorPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DoctorPicker.SelectedItem is DoctorDto doctor)
        {
            _selectedDoctorId = doctor.Id;
            DoctorErrorLabel.IsVisible = false;
        }
    }

    private void AppointmentDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        AppointmentDateErrorLabel.IsVisible = false;
    }

    private void ClearErrors()
    {
        PatientErrorLabel.IsVisible = false;
        DoctorErrorLabel.IsVisible = false;
        AppointmentDateErrorLabel.IsVisible = false;
    }

    private bool ValidateFields()
    {
        ClearErrors();

        bool hasError = false;

        if (!_selectedPatientId.HasValue)
        {
            PatientErrorLabel.Text = "Please select a patient.";
            PatientErrorLabel.IsVisible = true;
            hasError = true;
        }

        if (!_selectedDoctorId.HasValue)
        {
            DoctorErrorLabel.Text = "Please select a doctor.";
            DoctorErrorLabel.IsVisible = true;
            hasError = true;
        }

        if (AppointmentDatePicker.Date is DateTime selectedDate && selectedDate.Date < DateTime.Today)
        {
            AppointmentDateErrorLabel.Text = "Appointment date cannot be in the past.";
            AppointmentDateErrorLabel.IsVisible = true;
            hasError = true;
        }

        return !hasError;
    }


    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (!ValidateFields())
            return;

        if (AppointmentDatePicker.Date is not DateTime selectedDate)
        {
            AppointmentDateErrorLabel.Text = "Please select an appointment date.";
            AppointmentDateErrorLabel.IsVisible = true;
            return;
        }

        var request = new AppointmentRequestDto
        {
            PatientId = _selectedPatientId!.Value,
            DoctorId = _selectedDoctorId!.Value,
            AppointmentDate = selectedDate.Date
        };

        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            SaveButton.IsEnabled = false;

            var result = await _appointmentService.CreateAppointmentAsync(request);

            if (!result.Success)
            {
                if (result.Errors.TryGetValue("PatientId", out var patientErrors))
                {
                    PatientErrorLabel.Text = patientErrors[0];
                    PatientErrorLabel.IsVisible = true;
                }

                if (result.Errors.TryGetValue("DoctorId", out var doctorErrors))
                {
                    DoctorErrorLabel.Text = doctorErrors[0];
                    DoctorErrorLabel.IsVisible = true;
                }

                if (result.Errors.TryGetValue("AppointmentDate", out var dateErrors))
                {
                    AppointmentDateErrorLabel.Text = dateErrors[0];
                    AppointmentDateErrorLabel.IsVisible = true;
                }

                if (!result.Errors.Any())
                {
                    await DisplayAlertAsync("Unable to Create Appointment", result.Message ?? "Unable to create appointment.", "OK");
                }

                return;
            }

            await DisplayAlertAsync("Success", result.Message ?? "Appointment created successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch
        {
            await DisplayAlertAsync("Error", "An unexpected error occurred.", "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
            SaveButton.IsEnabled = true;
        }
    }



    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}