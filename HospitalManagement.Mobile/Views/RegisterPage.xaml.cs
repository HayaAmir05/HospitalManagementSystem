using HospitalManagement.Mobile.Models;
using HospitalManagement.Mobile.Services.Interfaces;
using System.Text.RegularExpressions;

namespace HospitalManagement.Mobile
{
    public partial class RegisterPage : ContentPage
    {
        private readonly IAuthService _authService;
        private bool _isPasswordVisible = false;
        private bool _isConfirmPasswordVisible = false;
        private string? _selectedRole = null;

        public RegisterPage()
        {
            InitializeComponent();

            _authService =
                Application.Current!.Handler!.MauiContext!
                .Services.GetRequiredService<IAuthService>();
        }




        private void OnNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue)) return;

            string filtered = Regex.Replace(e.NewTextValue, @"[^a-zA-Z\s.'-]", "");

            if (filtered != e.NewTextValue)
            {
                NameEntry.Text = filtered;
            }
        }

        private void OnContactTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue)) return;

            string filtered = Regex.Replace(e.NewTextValue, @"[^0-9]", "");

            if (filtered != e.NewTextValue)
            {
                ContactEntry.Text = filtered;
            }
        }

        private void OnTogglePasswordClicked(object sender, EventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;
            PasswordEntry.IsPassword = !_isPasswordVisible;
            PasswordToggleButton.Text = _isPasswordVisible ? "🙈" : "👁";
        }

        private void OnToggleConfirmPasswordClicked(object sender, EventArgs e)
        {
            _isConfirmPasswordVisible = !_isConfirmPasswordVisible;
            ConfirmPasswordEntry.IsPassword = !_isConfirmPasswordVisible;
            ConfirmPasswordToggleButton.Text = _isConfirmPasswordVisible ? "🙈" : "👁";
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            NameErrorLabel.IsVisible = false;
            EmailErrorLabel.IsVisible = false;
            PasswordErrorLabel.IsVisible = false;
            ConfirmPasswordErrorLabel.IsVisible = false;
            ContactErrorLabel.IsVisible = false;
            RoleErrorLabel.IsVisible = false;
            GeneralErrorLabel.IsVisible = false;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                NameErrorLabel.Text = "Name is required.";
                NameErrorLabel.IsVisible = true;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                EmailErrorLabel.Text = "Email is required.";
                EmailErrorLabel.IsVisible = true;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                PasswordErrorLabel.Text = "Password is required.";
                PasswordErrorLabel.IsVisible = true;
                hasError = true;
            }

            if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                ConfirmPasswordErrorLabel.Text = "Passwords do not match.";
                ConfirmPasswordErrorLabel.IsVisible = true;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(ContactEntry.Text))
            {
                ContactErrorLabel.Text = "Contact is required.";
                ContactErrorLabel.IsVisible = true;
                hasError = true;
            }


            if (string.IsNullOrEmpty(_selectedRole))
            {
                RoleErrorLabel.Text = "Please select a role.";
                RoleErrorLabel.IsVisible = true;
                hasError = true;
            }
            if (hasError)
                return;

            var request = new RegisterRequestDto
            {
                Name = NameEntry.Text!,
                Email = EmailEntry.Text!,
                Password = PasswordEntry.Text!,
                ConfirmPassword = ConfirmPasswordEntry.Text!,
                Contact = ContactEntry.Text!,
                Role = _selectedRole!
            };

            var result = await _authService.RegisterAsync(request);

            if (result.Success)
            {
                await DisplayAlertAsync("Success", "Registration successful. Please log in.", "OK");

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                if (result.Errors.Any())
                {
                    if (result.Errors.ContainsKey("Name"))
                    {
                        NameErrorLabel.Text = result.Errors["Name"][0];
                        NameErrorLabel.IsVisible = true;
                    }

                    if (result.Errors.ContainsKey("Email"))
                    {
                        EmailErrorLabel.Text = result.Errors["Email"][0];
                        EmailErrorLabel.IsVisible = true;
                    }

                    if (result.Errors.ContainsKey("Password"))
                    {
                        PasswordErrorLabel.Text = result.Errors["Password"][0];
                        PasswordErrorLabel.IsVisible = true;
                    }

                    if (result.Errors.ContainsKey("ConfirmPassword"))
                    {
                        ConfirmPasswordErrorLabel.Text = result.Errors["ConfirmPassword"][0];
                        ConfirmPasswordErrorLabel.IsVisible = true;
                    }

                    if (result.Errors.ContainsKey("Contact"))
                    {
                        ContactErrorLabel.Text = result.Errors["Contact"][0];
                        ContactErrorLabel.IsVisible = true;
                    }

                    if (result.Errors.ContainsKey("Role"))
                    {
                        RoleErrorLabel.Text = result.Errors["Role"][0];
                        RoleErrorLabel.IsVisible = true;
                    }
                }
                else
                {
                    GeneralErrorLabel.Text = result.Message;
                    GeneralErrorLabel.IsVisible = true;
                }
            }
        }
        private async void OnRoleButtonClicked(object sender, EventArgs e)
        {
            string result = await DisplayActionSheetAsync(
                "Select Role",
                "Cancel",
                null,
                "Admin",
                "Receptionist");

            if (result != "Cancel" && !string.IsNullOrEmpty(result))
            {
                _selectedRole = result;

                RoleButton.Text = result;
                RoleButton.TextColor = Colors.Black;
            }
        }

        private async void OnLoginTapped(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }


    }
}