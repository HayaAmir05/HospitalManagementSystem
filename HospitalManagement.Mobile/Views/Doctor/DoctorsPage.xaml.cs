
using HospitalManagement.Mobile.Models.DoctorDtos;
using HospitalManagement.Mobile.Services.Interfaces;
using HospitalManagement.Mobile.Views.Doctor;

namespace HospitalManagement.Mobile;

public partial class DoctorsPage : ContentPage
{
    private readonly IDoctorService _doctorService;

    private List<DoctorDto> _allDoctors = new();


    // For clearing search bar on the successful crud operation
    private bool _clearSearchOnAppearing = false;

    public DoctorsPage(IDoctorService doctorService)
    {
        InitializeComponent();
        _doctorService = doctorService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_clearSearchOnAppearing)
        {
            SearchBarDoctors.Text = "";
            _clearSearchOnAppearing = false;
        }

        await LoadDoctors();
    }

    private async Task LoadDoctors()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            DoctorsCollectionView.IsVisible = false;
            EmptyLayout.IsVisible = false;

            _allDoctors = await _doctorService.GetAllDoctorsAsync();

            if (_allDoctors.Count == 0)
            {
                EmptyLayout.IsVisible = true;
            }
            else
            {
                DoctorsCollectionView.ItemsSource = _allDoctors;
                DoctorsCollectionView.IsVisible = true;
            }
        }
        catch
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to load doctors.",
                "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;

            RefreshDoctors.IsRefreshing = false;
        }
    }

    private void SearchBarDoctors_TextChanged(object sender, TextChangedEventArgs e)
    {
        FilterDoctors(e.NewTextValue);
    }

    private void FilterDoctors(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            DoctorsCollectionView.ItemsSource = _allDoctors;
            DoctorsCollectionView.IsVisible = _allDoctors.Count > 0;
            EmptyLayout.IsVisible = _allDoctors.Count == 0;
            return;
        }

        var filteredDoctors = _allDoctors.Where(d =>
            d.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            d.Specialization.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();

        DoctorsCollectionView.ItemsSource = filteredDoctors;
        DoctorsCollectionView.IsVisible = filteredDoctors.Count > 0;
        EmptyLayout.IsVisible = filteredDoctors.Count == 0;
    }

    private async void RefreshDoctors_Refreshing(object sender, EventArgs e)
    {
        await LoadDoctors();
    }

    private async void DoctorCard_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext is DoctorDto doctor)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DoctorDetailsPage)}?doctorId={doctor.Id}");
        }
    }

    private async void AddDoctor_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddDoctorPage));
    }

    public void ClearSearch()
    {
        SearchBarDoctors.Text = "";
    }
}