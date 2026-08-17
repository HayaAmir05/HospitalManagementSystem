using System.Text.RegularExpressions;

namespace HospitalManagement.Mobile.Validators;

public static class PatientValidator
{
    public static Dictionary<string, string> Validate(string? name, string? email, string? age, string? contact, string? disease, string? address)
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(name))
        {
            errors["Name"] = "Name is required.";
        }
        else if (!Regex.IsMatch(name, @"^[A-Za-z\s.'-]{2,50}$"))
        {
            errors["Name"] = "Name must contain only letters and be between 2 and 50 characters.";
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            errors["Email"] = "Email is required.";
        }
        else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errors["Email"] = "Enter a valid email address.";
        }

        if (string.IsNullOrWhiteSpace(age))
        {
            errors["Age"] = "Age is required.";
        }
        else if (!int.TryParse(age, out int ageValue))
        {
            errors["Age"] = "Age must be a valid number.";
        }
        else if (ageValue < 0 || ageValue > 130)
        {
            errors["Age"] = "Age must be between 0 and 130.";
        }

        if (string.IsNullOrWhiteSpace(contact))
        {
            errors["Contact"] = "Contact is required.";
        }
        else if (!Regex.IsMatch(contact, @"^(03\d{9}|\+923\d{9})$"))
        {
            errors["Contact"] = "Enter a valid Pakistani number (e.g., 03001234567 or +923001234567).";
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
}