using HospitalManagement.Mobile.Models.PatientDtos;
using HospitalManagement.Mobile.Services.Interfaces;

namespace HospitalManagement.Mobile.Views.Patient;

public partial class PatientsPage : ContentPage
{
    private readonly IPatientService _patientService;

    private List<PatientResponseDto> _allPatients = new();

    public PatientsPage(IPatientService patientService)
    {
        InitializeComponent();

        _patientService = patientService;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadPatients();
    }


    private async Task LoadPatients()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            PatientsCollectionView.IsVisible = false;
            EmptyLayout.IsVisible = false;

            _allPatients =
                await _patientService.GetAllPatientsAsync();

            if (_allPatients.Count == 0)
            {
                EmptyLayout.IsVisible = true;
            }
            else
            {
                PatientsCollectionView.ItemsSource =
                    _allPatients;

                PatientsCollectionView.IsVisible = true;
            }
        }
        catch
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to load patients.",
                "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;

            RefreshPatients.IsRefreshing = false;
        }
    }


    private void SearchBarPatients_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        FilterPatients(e.NewTextValue);
    }


    private void FilterPatients(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            PatientsCollectionView.ItemsSource =
                _allPatients;

            PatientsCollectionView.IsVisible =
                _allPatients.Count > 0;

            EmptyLayout.IsVisible =
                _allPatients.Count == 0;

            return;
        }

        var filteredPatients =
            _allPatients
                .Where(p =>
                    p.Name.Contains(
                        keyword,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    p.Disease.Contains(
                        keyword,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

        PatientsCollectionView.ItemsSource =
            filteredPatients;

        PatientsCollectionView.IsVisible =
            filteredPatients.Count > 0;

        EmptyLayout.IsVisible =
            filteredPatients.Count == 0;
    }


    private async void RefreshPatients_Refreshing(
        object sender,
        EventArgs e)
    {
        await LoadPatients();
    }


    private async void PatientCard_Tapped(
        object sender,
        TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext is PatientResponseDto patient)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(PatientDetailsPage)}?patientId={patient.Id}");
        }
    }


    private async void AddPatient_Clicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(AddPatientPage));
    }
}