using HospitalManagement.Mobile.Models;

using HospitalManagement.Mobile.Services.Interfaces;

namespace HospitalManagement.Mobile
{
    public partial class LoginPage : ContentPage
    {

        private readonly IAuthService _authService;
        private bool _isPasswordVisible = false;

        public LoginPage()
        {
            InitializeComponent();
            _authService =
         Application.Current!.Handler!.MauiContext!
         .Services.GetRequiredService<IAuthService>();
        }



        private void OnTogglePasswordClicked(object sender, EventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;

            PasswordEntry.IsPassword = !_isPasswordVisible;

            ToggleButton.Text = _isPasswordVisible ? "🙈" : "👁";
        }


        private async void OnLoginClicked(object sender, EventArgs e)
        {
            EmailErrorLabel.IsVisible = false;
            PasswordErrorLabel.IsVisible = false;
            GeneralErrorLabel.IsVisible = false;

            bool hasError = false;

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

            if (hasError)
                return;

            var request = new LoginRequestDto
            {
                Email = EmailEntry.Text!,
                Password = PasswordEntry.Text!
            };

            var result = await _authService.LoginAsync(request);

            if (result.Success)
            {
                Preferences.Set("auth_token", result.Token);
                await DisplayAlertAsync("Success", "Login successful.", "OK");
                await Shell.Current.GoToAsync($"{nameof(DashboardPage)}?userName={result.UserName}&role={result.Role}");
            }
            else
            {
                GeneralErrorLabel.Text = result.Message;
                GeneralErrorLabel.IsVisible = true;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            EmailEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            EmailErrorLabel.IsVisible = false;
            PasswordErrorLabel.IsVisible = false;
            GeneralErrorLabel.IsVisible = false;
        }

        private async void OnRegisterTapped(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

    }
}