using HospitalManagement.Mobile.Models.DoctorDtos;
using HospitalManagement.Mobile.Services.Interfaces;
using System.Text.RegularExpressions;

namespace HospitalManagement.Mobile.Views.Doctor;

public partial class AddDoctorPage : ContentPage
{
    private readonly IDoctorService _doctorService;

    public AddDoctorPage(IDoctorService doctorService)
    {
        InitializeComponent();

        _doctorService = doctorService;
    }


    // =========================================================
    // TEXT SANITIZATION
    // =========================================================

    private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filtered =
            Regex.Replace(
                e.NewTextValue,
                @"[^a-zA-Z\s.'-]",
                "");

        if (filtered != e.NewTextValue)
        {
            NameEntry.Text = filtered;
            NameEntry.CursorPosition = filtered.Length;
        }
    }


    private void AgeEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filtered =
            Regex.Replace(
                e.NewTextValue,
                @"[^0-9]",
                "");

        if (filtered != e.NewTextValue)
        {
            AgeEntry.Text = filtered;
            AgeEntry.CursorPosition = filtered.Length;
        }
    }


    private void ContactEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        string value = e.NewTextValue;

        string filtered;

        if (value.StartsWith("+"))
        {
            filtered =
                "+" +
                Regex.Replace(
                    value.Substring(1),
                    @"[^0-9]",
                    "");
        }
        else
        {
            filtered =
                Regex.Replace(
                    value,
                    @"[^0-9]",
                    "");
        }

        if (filtered != value)
        {
            ContactEntry.Text = filtered;
            ContactEntry.CursorPosition = filtered.Length;
        }
    }


    private void SpecializationEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filtered =
            Regex.Replace(
                e.NewTextValue,
                @"[^a-zA-Z\s-]",
                "");

        if (filtered != e.NewTextValue)
        {
            SpecializationEntry.Text = filtered;
            SpecializationEntry.CursorPosition = filtered.Length;
        }
    }


    private void ExperienceEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filtered =
            Regex.Replace(
                e.NewTextValue,
                @"[^0-9]",
                "");

        if (filtered != e.NewTextValue)
        {
            ExperienceEntry.Text = filtered;
            ExperienceEntry.CursorPosition = filtered.Length;
        }
    }


    // =========================================================
    // FIELD VALIDATION - WHEN FIELD LOSES FOCUS
    // =========================================================

    private void NameEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateName();
    }


    private void EmailEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateEmail();
    }


    private void AgeEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateAge();
    }


    private void ContactEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateContact();
    }


    private void SpecializationEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateSpecialization();
    }


    private void ExperienceEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateExperience();
    }


    // =========================================================
    // INDIVIDUAL VALIDATORS
    // =========================================================

    private bool ValidateName()
    {
        string name = NameEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(name))
        {
            ShowError(
                NameErrorLabel,
                "Doctor's name is required.");

            return false;
        }

        if (!Regex.IsMatch(
                name,
                @"^[A-Za-z\s.'-]{2,50}$"))
        {
            ShowError(
                NameErrorLabel,
                "Name must contain only letters and be between 2 and 50 characters.");

            return false;
        }

        HideError(NameErrorLabel);

        return true;
    }


    private bool ValidateEmail()
    {
        string email = EmailEntry.Text?.Trim() ?? "";

        // Email is optional for Doctor.

        if (string.IsNullOrWhiteSpace(email))
        {
            HideError(EmailErrorLabel);
            return true;
        }

        if (!Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            ShowError(
                EmailErrorLabel,
                "Enter a valid email address.");

            return false;
        }

        HideError(EmailErrorLabel);

        return true;
    }


    private bool ValidateAge()
    {
        string ageText = AgeEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(ageText))
        {
            ShowError(
                AgeErrorLabel,
                "Age is required.");

            return false;
        }

        if (!int.TryParse(
                ageText,
                out int age))
        {
            ShowError(
                AgeErrorLabel,
                "Age must be a valid number.");

            return false;
        }

        if (age < 23 || age > 80)
        {
            ShowError(
                AgeErrorLabel,
                "Doctor's age must be between 23 and 80.");

            return false;
        }

        HideError(AgeErrorLabel);

        return true;
    }


    private bool ValidateContact()
    {
        string contact =
            ContactEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(contact))
        {
            ShowError(
                ContactErrorLabel,
                "Contact number is required.");

            return false;
        }

        if (!Regex.IsMatch(
                contact,
                @"^(03\d{9}|\+923\d{9})$"))
        {
            ShowError(
                ContactErrorLabel,
                "Enter a valid Pakistani number (e.g. 03001234567 or +923001234567).");

            return false;
        }

        HideError(ContactErrorLabel);

        return true;
    }


    private bool ValidateSpecialization()
    {
        string specialization =
            SpecializationEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(specialization))
        {
            ShowError(
                SpecializationErrorLabel,
                "Specialization is required.");

            return false;
        }

        if (specialization.Length < 2)
        {
            ShowError(
                SpecializationErrorLabel,
                "Specialization must contain at least 2 characters.");

            return false;
        }

        HideError(SpecializationErrorLabel);

        return true;
    }


    private bool ValidateExperience()
    {
        string experienceText =
            ExperienceEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(experienceText))
        {
            ShowError(
                ExperienceErrorLabel,
                "Experience is required.");

            return false;
        }

        if (!int.TryParse(
                experienceText,
                out int experience))
        {
            ShowError(
                ExperienceErrorLabel,
                "Experience must be a valid number.");

            return false;
        }

        if (experience < 0)
        {
            ShowError(
                ExperienceErrorLabel,
                "Experience cannot be negative.");

            return false;
        }

        HideError(ExperienceErrorLabel);

        return true;
    }


    // =========================================================
    // VALIDATE EVERYTHING
    // =========================================================

    private bool ValidateAllFields()
    {
        bool nameValid = ValidateName();
        bool emailValid = ValidateEmail();
        bool ageValid = ValidateAge();
        bool contactValid = ValidateContact();
        bool specializationValid = ValidateSpecialization();
        bool experienceValid = ValidateExperience();

        return
            nameValid &&
            emailValid &&
            ageValid &&
            contactValid &&
            specializationValid &&
            experienceValid;
    }


    // =========================================================
    // ERROR LABEL HELPERS
    // =========================================================

    private void ShowError(
        Label label,
        string message)
    {
        label.Text = message;
        label.IsVisible = true;
    }


    private void HideError(Label label)
    {
        label.Text = "";
        label.IsVisible = false;
    }


    private void ClearAllErrors()
    {
        HideError(NameErrorLabel);
        HideError(EmailErrorLabel);
        HideError(AgeErrorLabel);
        HideError(ContactErrorLabel);
        HideError(SpecializationErrorLabel);
        HideError(ExperienceErrorLabel);

        GeneralErrorLabel.Text = "";
        GeneralErrorLabel.IsVisible = false;
    }


    // =========================================================
    // SAVE DOCTOR
    // =========================================================

    private async void SaveDoctor_Clicked(
        object sender,
        EventArgs e)
    {
        ClearAllErrors();

        // Validate every field before calling API.

        if (!ValidateAllFields())
            return;


        if (!int.TryParse(
                AgeEntry.Text,
                out int age))
        {
            ShowError(
                AgeErrorLabel,
                "Age must be a valid number.");

            return;
        }


        if (!int.TryParse(
                ExperienceEntry.Text,
                out int experience))
        {
            ShowError(
                ExperienceErrorLabel,
                "Experience must be a valid number.");

            return;
        }


        var request = new DoctorRequestDto
        {
            Name = NameEntry.Text!.Trim(),

            Email = string.IsNullOrWhiteSpace(
                EmailEntry.Text)
                ? null
                : EmailEntry.Text.Trim(),

            Age = age,

            Contact = ContactEntry.Text!.Trim(),

            Specialization =
                SpecializationEntry.Text!.Trim(),

            Experience = experience
        };


        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            SaveButton.IsEnabled = false;


            var result =
                await _doctorService
                    .CreateDoctorAsync(request);


            // =============================================
            // API SUCCESS
            // =============================================

            if (result.Success)
            {
                await DisplayAlertAsync(
                    "Success",
                    result.Message ??
                    "Doctor created successfully.",
                    "OK");

                await Shell.Current.GoToAsync("..");

                return;
            }


            // =============================================
            // API VALIDATION / BUSINESS ERRORS
            // =============================================

            if (result.Errors != null &&
                result.Errors.Count > 0)
            {
                foreach (var error in result.Errors)
                {
                    string field = error.Key;

                    string message =
                        error.Value.FirstOrDefault()
                        ?? "Invalid value.";

                    switch (field)
                    {
                        case "Name":
                            ShowError(
                                NameErrorLabel,
                                message);
                            break;

                        case "Email":
                            ShowError(
                                EmailErrorLabel,
                                message);
                            break;

                        case "Age":
                            ShowError(
                                AgeErrorLabel,
                                message);
                            break;

                        case "Contact":
                            ShowError(
                                ContactErrorLabel,
                                message);
                            break;

                        case "Specialization":
                            ShowError(
                                SpecializationErrorLabel,
                                message);
                            break;

                        case "Experience":
                            ShowError(
                                ExperienceErrorLabel,
                                message);
                            break;

                        default:
                            GeneralErrorLabel.Text =
                                message;

                            GeneralErrorLabel.IsVisible =
                                true;

                            break;
                    }
                }
            }
            else
            {
                GeneralErrorLabel.Text =
                    result.Message ??
                    "Unable to create doctor.";

                GeneralErrorLabel.IsVisible = true;
            }
        }
        catch
        {
            GeneralErrorLabel.Text =
                "Unable to connect to the server. Please try again.";

            GeneralErrorLabel.IsVisible = true;
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;

            SaveButton.IsEnabled = true;
        }
    }


    // =========================================================
    // CANCEL
    // =========================================================

    private async void CancelButton_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    // =========================================================
    // BACK
    // =========================================================

    private async void BackButton_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}