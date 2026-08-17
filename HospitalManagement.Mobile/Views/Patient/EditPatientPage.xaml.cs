using HospitalManagement.Mobile.Models.PatientDtos;
using HospitalManagement.Mobile.Services.Interfaces;
using System.Text.RegularExpressions;

namespace HospitalManagement.Mobile.Views.Patient;

[QueryProperty(nameof(PatientId), "patientId")]
public partial class EditPatientPage : ContentPage
{
    private readonly IPatientService _patientService;

    private int _patientId;

    private bool _isLoadingPatient;

    private bool _isSanitizing;


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


    public EditPatientPage(IPatientService patientService)
    {
        InitializeComponent();

        _patientService = patientService;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isLoadingPatient)
            return;

        _isLoadingPatient = true;

        await LoadPatient();

        _isLoadingPatient = false;
    }


    private async Task LoadPatient()
    {
        try
        {
            PageLoadingIndicator.IsVisible = true;
            PageLoadingIndicator.IsRunning = true;

            var patient =
                await _patientService.GetPatientByIdAsync(_patientId);

            if (patient == null)
            {
                GeneralErrorLabel.Text = "Patient not found.";
                GeneralErrorLabel.IsVisible = true;
                return;
            }

            NameEntry.Text = patient.Name;
            EmailEntry.Text = patient.Email;
            AgeEntry.Text = patient.Age.ToString();
            ContactEntry.Text = patient.Contact;
            DiseaseEntry.Text = patient.Disease;
            AddressEditor.Text = patient.Address;
        }
        catch
        {
            GeneralErrorLabel.Text =
                "Unable to load patient information.";

            GeneralErrorLabel.IsVisible = true;
        }
        finally
        {
            PageLoadingIndicator.IsRunning = false;
            PageLoadingIndicator.IsVisible = false;
        }
    }


    // ============================================================
    // SANITIZATION
    // ============================================================


    private void NameEntry_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        if (_isSanitizing || string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filtered =
            Regex.Replace(
                e.NewTextValue,
                @"[^a-zA-Z\s.'-]",
                "");

        if (filtered != e.NewTextValue)
        {
            _isSanitizing = true;

            NameEntry.Text = filtered;

            _isSanitizing = false;
        }
    }


    private void AgeEntry_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        if (_isSanitizing || string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filtered =
            Regex.Replace(
                e.NewTextValue,
                @"[^0-9]",
                "");

        if (filtered != e.NewTextValue)
        {
            _isSanitizing = true;

            AgeEntry.Text = filtered;

            _isSanitizing = false;
        }
    }


    private void ContactEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filtered = e.NewTextValue;

        if (filtered.StartsWith("+"))
        {
            filtered = "+" + Regex.Replace(filtered.Substring(1), @"[^0-9]", "");
        }
        else
        {
            filtered = Regex.Replace(filtered, @"[^0-9]", "");
        }

        if (filtered != e.NewTextValue)
        {
            ContactEntry.Text = filtered;
            ContactEntry.CursorPosition = filtered.Length;
        }
    }

    // ============================================================
    // FIELD-LEVEL VALIDATION
    // ============================================================


    private void NameEntry_Unfocused(
        object sender,
        FocusEventArgs e)
    {
        ValidateName();
    }


    private void EmailEntry_Unfocused(
        object sender,
        FocusEventArgs e)
    {
        ValidateEmail();
    }


    private void AgeEntry_Unfocused(
        object sender,
        FocusEventArgs e)
    {
        ValidateAge();
    }


    private void ContactEntry_Unfocused(
        object sender,
        FocusEventArgs e)
    {
        ValidateContact();
    }


    private void DiseaseEntry_Unfocused(
        object sender,
        FocusEventArgs e)
    {
        ValidateDisease();
    }


    private void AddressEditor_Unfocused(
        object sender,
        FocusEventArgs e)
    {
        ValidateAddress();
    }


    // ============================================================
    // VALIDATORS
    // ============================================================


    private bool ValidateName()
    {
        string name = NameEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(name))
        {
            ShowError(
                NameErrorLabel,
                "Name is required.");

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

        // Email is optional for Patient
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
                "Enter a valid age.");

            return false;
        }

        if (age < 0 || age > 130)
        {
            ShowError(
                AgeErrorLabel,
                "Age must be between 0 and 130.");

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
                "Contact is required.");

            return false;
        }

        if (!Regex.IsMatch(
                contact,
                @"^(03\d{9}|\+923\d{9})$"))
        {
            ShowError(
                ContactErrorLabel,
                "Enter a valid Pakistani number (e.g., 03001234567).");

            return false;
        }

        HideError(ContactErrorLabel);

        return true;
    }


    private bool ValidateDisease()
    {
        string disease =
            DiseaseEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(disease))
        {
            ShowError(
                DiseaseErrorLabel,
                "Disease is required.");

            return false;
        }

        HideError(DiseaseErrorLabel);

        return true;
    }


    private bool ValidateAddress()
    {
        string address =
            AddressEditor.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(address))
        {
            ShowError(
                AddressErrorLabel,
                "Address is required.");

            return false;
        }

        HideError(AddressErrorLabel);

        return true;
    }


    // ============================================================
    // SAVE
    // ============================================================


    private async void Save_Clicked(
        object sender,
        EventArgs e)
    {
        GeneralErrorLabel.IsVisible = false;

        bool isValid = true;

        if (!ValidateName())
            isValid = false;

        if (!ValidateEmail())
            isValid = false;

        if (!ValidateAge())
            isValid = false;

        if (!ValidateContact())
            isValid = false;

        if (!ValidateDisease())
            isValid = false;

        if (!ValidateAddress())
            isValid = false;


        if (!isValid)
            return;


        int.TryParse(
            AgeEntry.Text,
            out int age);


        var request = new PatientRequestDto
        {
            Name = NameEntry.Text?.Trim() ?? "",

            Email =
                string.IsNullOrWhiteSpace(EmailEntry.Text)
                    ? null
                    : EmailEntry.Text.Trim(),

            Age = age,

            Contact =
                ContactEntry.Text?.Trim() ?? "",

            Disease =
                DiseaseEntry.Text?.Trim() ?? "",

            Address =
                AddressEditor.Text?.Trim() ?? ""
        };


        try
        {
            SavingIndicator.IsVisible = true;
            SavingIndicator.IsRunning = true;

            var result =
                await _patientService.UpdatePatientAsync(
                    _patientId,
                    request);


            if (!result.Success)
            {
                ShowApiErrors(result);

                return;
            }


            await Shell.Current.GoToAsync("..");
        }
        catch
        {
            GeneralErrorLabel.Text =
                "An unexpected error occurred. Please try again.";

            GeneralErrorLabel.IsVisible = true;
        }
        finally
        {
            SavingIndicator.IsRunning = false;
            SavingIndicator.IsVisible = false;
        }
    }


    // ============================================================
    // API ERROR MAPPING
    // ============================================================


    private void ShowApiErrors(
        PatientOperationResponseDto result)
    {
        GeneralErrorLabel.IsVisible = false;

        if (result.Errors == null ||
            result.Errors.Count == 0)
        {
            GeneralErrorLabel.Text =
                result.Message ??
                "Unable to update patient.";

            GeneralErrorLabel.IsVisible = true;

            return;
        }


        if (result.Errors.TryGetValue(
                "Name",
                out var nameErrors))
        {
            ShowError(
                NameErrorLabel,
                nameErrors[0]);
        }


        if (result.Errors.TryGetValue(
                "Email",
                out var emailErrors))
        {
            ShowError(
                EmailErrorLabel,
                emailErrors[0]);
        }


        if (result.Errors.TryGetValue(
                "Age",
                out var ageErrors))
        {
            ShowError(
                AgeErrorLabel,
                ageErrors[0]);
        }


        if (result.Errors.TryGetValue(
                "Contact",
                out var contactErrors))
        {
            ShowError(
                ContactErrorLabel,
                contactErrors[0]);
        }


        if (result.Errors.TryGetValue(
                "Disease",
                out var diseaseErrors))
        {
            ShowError(
                DiseaseErrorLabel,
                diseaseErrors[0]);
        }


        if (result.Errors.TryGetValue(
                "Address",
                out var addressErrors))
        {
            ShowError(
                AddressErrorLabel,
                addressErrors[0]);
        }
    }


    // ============================================================
    // ERROR LABEL HELPERS
    // ============================================================


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


    // ============================================================
    // BACK / CANCEL
    // ============================================================


    private async void Back_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}