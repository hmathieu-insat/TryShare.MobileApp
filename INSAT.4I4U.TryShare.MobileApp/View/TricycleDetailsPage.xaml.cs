using INSAT._4I4U.TryShare.MobileApp.Model;
using System.Windows.Input;

namespace INSAT._4I4U.TryShare.MobileApp.View;

[QueryProperty(nameof(Tricycle),"Tricycle")]
public partial class TricycleDetailsPage: ContentPage
{
    readonly TricycleDetailsViewModel _viewModel;
    public TricycleDetailsPage(TricycleDetailsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        viewModel.OnDetailsTryToNavigateWithoutConnectivity = async () => await DisplayConnectivityErrorPopup();
        viewModel.OnDetailsTryToNavigateWithoutLocationEnabled = async () => await DisplayLocationUnabledErrorPopup();
        viewModel.OnDetailsTryToNavigateWithoutLocationAuthorized= async () => await DisplayLocationUnauthorizedErrorPopup();
    }

    protected override async void OnAppearing()
    {
        // Call the base methods first
        base.OnAppearing();

        // Cast the BindingContext to a TricycleDetailsViewModel (this cast is safe because the BindingContext is set in the constructor)
        // and call the SetTricycleAddressLabelAsync method defined in the ViewModel
        await (BindingContext as TricycleDetailsViewModel).SetTricycleAddressLabelAsync();
    }

    private async void OnTermsAndConditionsTapped(object sender, TappedEventArgs args)
    {
        await (BindingContext as TricycleDetailsViewModel).GoToTermsAndConditionsAsync();
    }
    public async Task DisplayConnectivityErrorPopup()
    {
        await DisplayAlert("Alerte", "Aucune connection internet trouv�e", "OK");
    }

    public async Task DisplayLocationUnabledErrorPopup()
    {
        await DisplayAlert("Alerte", "La localisation n'est pas activ�e", "OK");
    }

    public async Task DisplayLocationUnauthorizedErrorPopup()
    {
        await DisplayAlert("Alerte", "La localisation n'est pas autoris�e", "OK");
    }
}