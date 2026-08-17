using HospitalManagement.Mobile.Models.AppointmentDtos;
using HospitalManagement.Mobile.Models.DoctorDtos;
using HospitalManagement.Mobile.Models.PatientDtos;
using HospitalManagement.Mobile.Services.Interfaces;

namespace HospitalManagement.Mobile.Views.Appointment;

[QueryProperty(nameof(AppointmentId), "appointmentId")]
public partial class EditAppointmentPage : ContentPage
{
    private readonly IAppointmentService _appointmentService;
    private readonly IPatientService _patientService;
    private readonly IDoctorService _doctorService;

    private int _appointmentId;

    private AppointmentResponseDto? _existingAppointment;

    private bool _isLoading;

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

    public EditAppointmentPage(IAppointmentService appointmentService, IPatientService patientService, IDoctorService doctorService)
    {
        InitializeComponent();

        _appointmentService = appointmentService;
        _patientService = patientService;
        _doctorService = doctorService;
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
            PageLoadingIndicator.IsVisible = true;
            PageLoadingIndicator.IsRunning = true;

            var appointment = await _appointmentService.GetAppointmentByIdAsync(_appointmentId);

            if (appointment == null)
            {
                await DisplayAlertAsync("Error", "Appointment not found.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            _existingAppointment = appointment;

            var patients = await _patientService.GetAllPatientsAsync();
            var doctors = await _doctorService.GetAllDoctorsAsync();

            PatientPicker.ItemsSource = patients;
            PatientPicker.ItemDisplayBinding = new Binding("Name");

            DoctorPicker.ItemsSource = doctors;
            DoctorPicker.ItemDisplayBinding = new Binding("Name");

            var patientIndex = patients.FindIndex(p => p.Id == appointment.PatientId);

            if (patientIndex >= 0)
                PatientPicker.SelectedIndex = patientIndex;

            var doctorIndex = doctors.FindIndex(d => d.Id == appointment.DoctorId);

            if (doctorIndex >= 0)
                DoctorPicker.SelectedIndex = doctorIndex;

            AppointmentDatePicker.Date = appointment.AppointmentDate.Date;

            // Existing appointment date becomes the minimum.
            // This allows an old appointment to remain editable
            // while preventing the user from moving it backward.
            AppointmentDatePicker.MinimumDate = appointment.AppointmentDate.Date;

            ConfigureStatusPicker(appointment.Status, appointment.AppointmentDate.Date);
        }
        catch
        {
            await DisplayAlertAsync("Error", "Unable to load appointment information.", "OK");
        }
        finally
        {
            PageLoadingIndicator.IsRunning = false;
            PageLoadingIndicator.IsVisible = false;
        }
    }

    private void ConfigureStatusPicker(string currentStatus, DateTime appointmentDate)
    {
        StatusPicker.Items.Clear();

        StatusPicker.Items.Add("Pending");
        StatusPicker.Items.Add("Completed");
        StatusPicker.Items.Add("Cancelled");

        if (appointmentDate.Date > DateTime.Today)
        {
            // Future appointment cannot be Completed.
            StatusPicker.Items.Remove("Completed");

            if (currentStatus == "Completed")
                currentStatus = "Pending";
        }

        var index = StatusPicker.Items.IndexOf(currentStatus);

        if (index >= 0)
            StatusPicker.SelectedIndex = index;
    }

    private void AppointmentDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        AppointmentDateErrorLabel.IsVisible = false;

        ConfigureStatusPicker(
            StatusPicker.SelectedItem?.ToString() ?? "Pending",
            e.NewDate!.Value.Date);
    }

    private void PatientPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        PatientErrorLabel.IsVisible = false;
    }

    private void DoctorPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        DoctorErrorLabel.IsVisible = false;
    }

    private void StatusPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        StatusErrorLabel.IsVisible = false;
    }

    private void ClearErrors()
    {
        PatientErrorLabel.IsVisible = false;
        DoctorErrorLabel.IsVisible = false;
        AppointmentDateErrorLabel.IsVisible = false;
        StatusErrorLabel.IsVisible = false;
        GeneralErrorLabel.IsVisible = false;
    }

    private bool ValidateFields()
    {
        ClearErrors();

        bool hasError = false;

        if (PatientPicker.SelectedItem is not PatientResponseDto)
        {
            PatientErrorLabel.Text = "Please select a patient.";
            PatientErrorLabel.IsVisible = true;
            hasError = true;
        }

        if (DoctorPicker.SelectedItem is not DoctorDto)
        {
            DoctorErrorLabel.Text = "Please select a doctor.";
            DoctorErrorLabel.IsVisible = true;
            hasError = true;
        }

        if (_existingAppointment != null &&
    AppointmentDatePicker.Date is DateTime pickedDate &&
    pickedDate.Date < _existingAppointment.AppointmentDate.Date)
        {
            AppointmentDateErrorLabel.Text = "Appointment date cannot be moved to an earlier date.";
            AppointmentDateErrorLabel.IsVisible = true;
            hasError = true;
        }

        if (StatusPicker.SelectedItem == null)
        {
            StatusErrorLabel.Text = "Please select an appointment status.";
            StatusErrorLabel.IsVisible = true;
            hasError = true;
        }

        if (AppointmentDatePicker.Date is DateTime futureCheckDate &&
     futureCheckDate.Date > DateTime.Today &&
     StatusPicker.SelectedItem?.ToString() == "Completed")
        {
            StatusErrorLabel.Text = "A future appointment cannot be marked as Completed.";
            StatusErrorLabel.IsVisible = true;
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

        var patient = (PatientResponseDto)PatientPicker.SelectedItem!;
        var doctor = (DoctorDto)DoctorPicker.SelectedItem!;

        var request = new AppointmentUpdateDto
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            AppointmentDate = selectedDate.Date,
            Status = StatusPicker.SelectedItem!.ToString()!
        };

        try
        {
            SavingIndicator.IsVisible = true;
            SavingIndicator.IsRunning = true;

            SaveButton.IsEnabled = false;

            var result = await _appointmentService.UpdateAppointmentAsync(_appointmentId, request);

            if (!result.Success)
            {
                ShowApiErrors(result);
                return;
            }

            await DisplayAlertAsync(
                "Success",
                result.Message ?? "Appointment updated successfully.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch
        {
            GeneralErrorLabel.Text = "An unexpected error occurred.";
            GeneralErrorLabel.IsVisible = true;
        }
        finally
        {
            SavingIndicator.IsRunning = false;
            SavingIndicator.IsVisible = false;

            SaveButton.IsEnabled = true;
        }
    }

    private void ShowApiErrors(AppointmentOperationResponseDto result)
    {
        ClearErrors();

        if (result.Errors == null || result.Errors.Count == 0)
        {
            GeneralErrorLabel.Text =
                result.Message ?? "Unable to update appointment.";

            GeneralErrorLabel.IsVisible = true;

            return;
        }

        foreach (var error in result.Errors)
        {
            var message = error.Value.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(message))
                continue;

            switch (error.Key)
            {
                case "PatientId":
                    PatientErrorLabel.Text = message;
                    PatientErrorLabel.IsVisible = true;
                    break;

                case "DoctorId":
                    DoctorErrorLabel.Text = message;
                    DoctorErrorLabel.IsVisible = true;
                    break;

                case "AppointmentDate":
                    AppointmentDateErrorLabel.Text = message;
                    AppointmentDateErrorLabel.IsVisible = true;
                    break;

                case "Status":
                    StatusErrorLabel.Text = message;
                    StatusErrorLabel.IsVisible = true;
                    break;

                default:
                    GeneralErrorLabel.Text = message;
                    GeneralErrorLabel.IsVisible = true;
                    break;
            }
        }
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}