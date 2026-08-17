using HospitalManagement.Mobile.Models.PatientDtos;
using HospitalManagement.Mobile.Services.Interfaces;
using System.Text.RegularExpressions;

namespace HospitalManagement.Mobile.Views.Patient;

public partial class AddPatientPage : ContentPage
{
    private readonly IPatientService _patientService;

    public AddPatientPage(IPatientService patientService)
    {
        InitializeComponent();
        _patientService = patientService;
    }

    private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filtered = Regex.Replace(
            e.NewTextValue,
            @"[^a-zA-Z\s.'-]",
            "");

        if (filtered != e.NewTextValue)
            NameEntry.Text = filtered;
    }

    private void AgeEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        string filtered = Regex.Replace(
            e.NewTextValue,
            @"[^0-9]",
            "");

        if (filtered != e.NewTextValue)
            AgeEntry.Text = filtered;
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

    private void NameEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateSingleField("Name");
    }

    private void EmailEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateSingleField("Email");
    }

    private void AgeEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateSingleField("Age");
    }

    private void ContactEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateSingleField("Contact");
    }

    private void DiseaseEntry_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateSingleField("Disease");
    }

    private void AddressEditor_Unfocused(object sender, FocusEventArgs e)
    {
        ValidateSingleField("Address");
    }

    private Dictionary<string, string> ValidateFields()
    {
        var errors = new Dictionary<string, string>();

        string name = NameEntry.Text?.Trim() ?? "";
        string email = EmailEntry.Text?.Trim() ?? "";
        string ageText = AgeEntry.Text?.Trim() ?? "";
        string contact = ContactEntry.Text?.Trim() ?? "";
        string disease = DiseaseEntry.Text?.Trim() ?? "";
        string address = AddressEditor.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(name))
        {
            errors["Name"] = "Name is required.";
        }
        else if (!Regex.IsMatch(name, @"^[A-Za-z\s.'-]{2,50}$"))
        {
            errors["Name"] = "Name must contain only valid letters and be 2-50 characters.";
        }

        if (!string.IsNullOrWhiteSpace(email) &&
            !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errors["Email"] = "Enter a valid email address.";
        }

        if (string.IsNullOrWhiteSpace(ageText))
        {
            errors["Age"] = "Age is required.";
        }
        else if (!int.TryParse(ageText, out int age))
        {
            errors["Age"] = "Age must be a valid number.";
        }
        else if (age < 0 || age > 130)
        {
            errors["Age"] = "Age must be between 0 and 130.";
        }

        if (string.IsNullOrWhiteSpace(contact))
        {
            errors["Contact"] = "Contact is required.";
        }
        else if (!Regex.IsMatch(contact, @"^(03\d{9}|\+923\d{9})$"))
        {
            errors["Contact"] = "Enter a valid Pakistani number.";
        }

        if (string.IsNullOrWhiteSpace(disease))
        {
            errors["Disease"] = "Disease is required.";
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            errors["Address"] = "Address is required.";
        }

        return errors;
    }

    private void ValidateSingleField(string field)
    {
        ClearFieldError(field);

        var errors = ValidateFields();

        if (errors.TryGetValue(field, out string? message))
            ShowFieldError(field, message);
    }

    private void ShowFieldError(string field, string message)
    {
        switch (field)
        {
            case "Name":
                NameErrorLabel.Text = message;
                NameErrorLabel.IsVisible = true;
                break;

            case "Email":
                EmailErrorLabel.Text = message;
                EmailErrorLabel.IsVisible = true;
                break;

            case "Age":
                AgeErrorLabel.Text = message;
                AgeErrorLabel.IsVisible = true;
                break;

            case "Contact":
                ContactErrorLabel.Text = message;
                ContactErrorLabel.IsVisible = true;
                break;

            case "Disease":
                DiseaseErrorLabel.Text = message;
                DiseaseErrorLabel.IsVisible = true;
                break;

            case "Address":
                AddressErrorLabel.Text = message;
                AddressErrorLabel.IsVisible = true;
                break;
        }
    }

    private void ClearFieldError(string field)
    {
        switch (field)
        {
            case "Name":
                NameErrorLabel.Text = "";
                NameErrorLabel.IsVisible = false;
                break;

            case "Email":
                EmailErrorLabel.Text = "";
                EmailErrorLabel.IsVisible = false;
                break;

            case "Age":
                AgeErrorLabel.Text = "";
                AgeErrorLabel.IsVisible = false;
                break;

            case "Contact":
                ContactErrorLabel.Text = "";
                ContactErrorLabel.IsVisible = false;
                break;

            case "Disease":
                DiseaseErrorLabel.Text = "";
                DiseaseErrorLabel.IsVisible = false;
                break;

            case "Address":
                AddressErrorLabel.Text = "";
                AddressErrorLabel.IsVisible = false;
                break;
        }
    }

    private void ClearAllErrors()
    {
        NameErrorLabel.IsVisible = false;
        EmailErrorLabel.IsVisible = false;
        AgeErrorLabel.IsVisible = false;
        ContactErrorLabel.IsVisible = false;
        DiseaseErrorLabel.IsVisible = false;
        AddressErrorLabel.IsVisible = false;
    }

    private void DisplayValidationErrors(
        Dictionary<string, string[]> errors)
    {
        ClearAllErrors();

        foreach (var error in errors)
        {
            if (error.Value.Length == 0)
                continue;

            ShowFieldError(error.Key, error.Value[0]);
        }
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        ClearAllErrors();

        var errors = ValidateFields();

        if (errors.Count > 0)
        {
            foreach (var error in errors)
                ShowFieldError(error.Key, error.Value);

            return;
        }

        int age = int.Parse(AgeEntry.Text!);

        var request = new PatientRequestDto
        {
            Name = NameEntry.Text!.Trim(),
            Email = string.IsNullOrWhiteSpace(EmailEntry.Text)
                ? null
                : EmailEntry.Text.Trim(),
            Age = age,
            Contact = ContactEntry.Text!.Trim(),
            Disease = DiseaseEntry.Text!.Trim(),
            Address = AddressEditor.Text!.Trim()
        };

        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var result =
                await _patientService.CreatePatientAsync(request);

            if (!result.Success)
            {
                DisplayValidationErrors(result.Errors);
                return;
            }

            await DisplayAlertAsync(
                "Success",
                result.Message ?? "Patient created successfully.",
                "OK");

            await Shell.Current.GoToAsync("..");
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

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}