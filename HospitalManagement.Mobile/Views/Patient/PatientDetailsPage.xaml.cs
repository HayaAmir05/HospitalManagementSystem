using HospitalManagement.Mobile.Models.PatientDtos;
using HospitalManagement.Mobile.Services.Interfaces;

namespace HospitalManagement.Mobile.Views.Patient;

[QueryProperty(nameof(PatientId), "patientId")]
public partial class PatientDetailsPage : ContentPage
{
    private readonly IPatientService _patientService;

    private int _patientId;

    private PatientResponseDto? _patient;


    public string PatientId
    {
        get => _patientId.ToString();

        set
        {
            if (int.TryParse(value, out int id))
            {
                _patientId = id;
            }
        }
    }


    public PatientDetailsPage(
        IPatientService patientService)
    {
        InitializeComponent();

        _patientService = patientService;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadPatient();
    }


    private async Task LoadPatient()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            _patient =
                await _patientService
                    .GetPatientByIdAsync(_patientId);

            if (_patient == null)
            {
                await DisplayAlertAsync(
                    "Error",
                    "Patient not found.",
                    "OK");

                await Shell.Current.GoToAsync("..");

                return;
            }

            NameLabel.Text = _patient.Name;
            DiseaseLabel.Text = _patient.Disease;

            AgeLabel.Text =
                _patient.Age.ToString();

            EmailLabel.Text =
                string.IsNullOrWhiteSpace(_patient.Email)
                    ? "Not provided"
                    : _patient.Email;

            ContactLabel.Text =
                _patient.Contact;

            AddressLabel.Text =
                _patient.Address;

            CreatedByLabel.Text =
                _patient.CreatedBy;
        }
        catch
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to load patient details.",
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


    private async void Edit_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(EditPatientPage)}?patientId={_patientId}");
    }


    private async void Delete_Clicked(
        object sender,
        EventArgs e)
    {
        bool confirm =
            await DisplayAlertAsync(
                "Delete Patient",
                "Are you sure you want to delete this patient?",
                "Delete",
                "Cancel");

        if (!confirm)
            return;


        try
        {
            var result =
                await _patientService
                    .DeletePatientAsync(_patientId);

            if (result.Success)
            {
                await DisplayAlertAsync(
                    "Success",
                    "Patient Deleted Successfully",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlertAsync(
                    "Delete Failed",
                    result.Message ?? "Unable to delete patient.",
                    "OK");
            }


        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                ex.Message,
                    "OK");
        }
    }
}