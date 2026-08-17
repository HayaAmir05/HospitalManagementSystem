using HospitalManagement.Mobile.Models.DoctorDtos;
using HospitalManagement.Mobile.Services.Interfaces;

namespace HospitalManagement.Mobile.Views.Doctor;

[QueryProperty(nameof(DoctorId), "doctorId")]
public partial class EditDoctorPage : ContentPage
{
    private readonly IDoctorService _doctorService;

    private int _doctorId;

    private bool _isLoadingDoctor;


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


    public EditDoctorPage(IDoctorService doctorService)
    {
        InitializeComponent();

        _doctorService = doctorService;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_isLoadingDoctor)
        {
            await LoadDoctor();
        }
    }


    private async Task LoadDoctor()
    {
        try
        {
            _isLoadingDoctor = true;

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var doctor =
                await _doctorService.GetDoctorByIdAsync(_doctorId);

            if (doctor == null)
            {
                await DisplayAlertAsync(
                    "Doctor Not Found",
                    "The requested doctor could not be found.",
                    "OK");

                await Shell.Current.GoToAsync("..");

                return;
            }


            // Fill the form with existing doctor information

            NameEntry.Text = doctor.Name;

            EmailEntry.Text = doctor.Email;

            AgeEntry.Text = doctor.Age.ToString();

            ContactEntry.Text = doctor.Contact;

            SpecializationEntry.Text =
                doctor.Specialization;

            ExperienceEntry.Text =
                doctor.Experience.ToString();
        }
        catch
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to load doctor information.",
                "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;

            _isLoadingDoctor = false;
        }
    }


    private async void SaveChanges_Clicked(
        object sender,
        EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        ErrorLabel.Text = "";

        // Basic client-side conversion checks

        if (!int.TryParse(AgeEntry.Text, out int age))
        {
            ErrorLabel.Text = "Please enter a valid age.";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (!int.TryParse(
                ExperienceEntry.Text,
                out int experience))
        {
            ErrorLabel.Text =
                "Please enter a valid experience value.";

            ErrorLabel.IsVisible = true;
            return;
        }


        var request = new DoctorRequestDto
        {
            Name = NameEntry.Text?.Trim() ?? "",

            Email = string.IsNullOrWhiteSpace(EmailEntry.Text)
                ? null
                : EmailEntry.Text.Trim(),

            Age = age,

            Contact = ContactEntry.Text?.Trim() ?? "",

            Specialization =
                SpecializationEntry.Text?.Trim() ?? "",

            Experience = experience
        };


        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;


            var result =
                await _doctorService.UpdateDoctorAsync(
                    _doctorId,
                    request);


            if (result.Success)
            {
                await DisplayAlertAsync(
                    "Success",
                    "Doctor updated successfully.",
                    "OK");

                /*
                 * Go back to DoctorDetailsPage.
                 *
                 * The details page will reload the
                 * updated doctor when it appears again.
                 */

                await Shell.Current.GoToAsync("..");

                return;
            }


            // Display API validation errors

            if (result.Errors != null &&
                result.Errors.Count > 0)
            {
                var messages = new List<string>();

                foreach (var error in result.Errors)
                {
                    foreach (var message in error.Value)
                    {
                        messages.Add(message);
                    }
                }

                ErrorLabel.Text =
                    string.Join(Environment.NewLine, messages);

                ErrorLabel.IsVisible = true;
            }
            else
            {
                ErrorLabel.Text =
                    result.Message ??
                    "Unable to update doctor.";

                ErrorLabel.IsVisible = true;
            }
        }
        catch
        {
            ErrorLabel.Text =
                "An unexpected error occurred while updating the doctor.";

            ErrorLabel.IsVisible = true;
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