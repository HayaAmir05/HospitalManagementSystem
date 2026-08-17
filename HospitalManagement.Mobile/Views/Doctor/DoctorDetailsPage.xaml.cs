using HospitalManagement.Mobile.Models.DoctorDtos;
using HospitalManagement.Mobile.Services.Interfaces;

namespace HospitalManagement.Mobile.Views.Doctor;

[QueryProperty(nameof(DoctorId), "doctorId")]
public partial class DoctorDetailsPage : ContentPage
{
    private readonly IDoctorService _doctorService;

    private DoctorDto? _doctor;

    private int _doctorId;

    public string DoctorId
    {
        get => _doctorId.ToString();

        set
        {
            if (int.TryParse(value, out int id))
            {
                _doctorId = id;
            }
        }
    }

    public DoctorDetailsPage(IDoctorService doctorService)
    {
        InitializeComponent();

        _doctorService = doctorService;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadDoctor();
    }


    private async Task LoadDoctor()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            _doctor = await _doctorService.GetDoctorByIdAsync(_doctorId);

            if (_doctor == null)
            {
                await DisplayAlertAsync(
                    "Doctor Not Found",
                    "The requested doctor could not be found.",
                    "OK");

                await Shell.Current.GoToAsync("..");

                return;
            }

            NameLabel.Text = _doctor.Name;

            SpecializationLabel.Text = _doctor.Specialization;

            AgeLabel.Text = $"{_doctor.Age} years";

            ExperienceLabel.Text = $"{_doctor.Experience} years";

            ContactLabel.Text = string.IsNullOrWhiteSpace(_doctor.Contact)
                ? "Not provided"
                : _doctor.Contact;

            EmailLabel.Text = string.IsNullOrWhiteSpace(_doctor.Email)
                ? "Not provided"
                : _doctor.Email;

            CreatedByLabel.Text = string.IsNullOrWhiteSpace(_doctor.CreatedBy)
                ? "System"
                : _doctor.CreatedBy;

            CreatedAtLabel.Text = _doctor.CreatedAt.ToString(
                "dd MMM yyyy, hh:mm tt");
        }
        catch
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to load doctor details.",
                "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }


    private async void EditDoctor_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(EditDoctorPage)}?doctorId={_doctorId}");
    }


    private async void DeleteDoctor_Clicked(
        object sender,
        EventArgs e)
    {
        bool confirm = await DisplayAlertAsync(
            "Delete Doctor",
            $"Are you sure you want to delete {_doctor?.Name}?",
            "Delete",
            "Cancel");

        if (!confirm)
            return;

        try
        {
            var result =
                await _doctorService.DeleteDoctorAsync(_doctorId);

            if (result.Success)
            {
                await DisplayAlertAsync(
                    "Success",
                    "Doctor deleted successfully.",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlertAsync(
                    "Delete Failed",
                    result.Message ?? "Unable to delete doctor.",
                    "OK");
            }
        }
        catch
        {
            await DisplayAlertAsync(
                "Error",
                "An unexpected error occurred while deleting the doctor.",
                "OK");
        }
    }


    private async void Back_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}