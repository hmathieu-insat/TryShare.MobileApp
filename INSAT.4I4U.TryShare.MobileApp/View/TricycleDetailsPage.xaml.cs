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
    public async Task DisplayAlertNoInternetOrNoAccountFound()
    {
        if (!_viewModel.IsConnectedAndSignedIn)
        {
            await DisplayAlert("Alert", "You have been alerted", "OK");
        }
    }
}