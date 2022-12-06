﻿using INSAT._4I4U.TryShare.MobileApp.ViewModel.Base;
using INSAT._4I4U.TryShare.MobileApp.Model;
using INSAT._4I4U.TryShare.MobileApp.View;
using INSAT._4I4U.TryShare.MobileApp.Services.Tricycles;

namespace INSAT._4I4U.TryShare.MobileApp.ViewModel
{
    public partial class MainPageViewModel : BaseViewModel
    {

        public ObservableCollection<Tricycle> Tricycles { get; } = new();

        [ObservableProperty]
        Tricycle mockSelectedTricycle;

        readonly ITricycleService tricycleMockService;

        public MainPageViewModel(ITricycleService tricycleMockService)
        {
            Title = "Accueil";
            this.tricycleMockService = tricycleMockService;
        }

        [RelayCommand]
        async Task GetTricyclesAsync()
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var list = await tricycleMockService.GetTricyclesAsync();

                if (Tricycles.Count != 0)
                    Tricycles.Clear();

                foreach (var tricycle in list)
                    Tricycles.Add(tricycle);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get tricycle {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToDetailsAsync(Tricycle tricycle)
        {
            if (Tricycles.Count == 0)
            {
                await GetTricyclesAsync();
                MockSelectedTricycle = Tricycles[0];
                return;
            }

            if (tricycle == null)
                return;

            await Shell.Current.GoToAsync(nameof(TricycleDetailsPage), true, new Dictionary<string, object>
                { {"Tricycle", tricycle } });
        }
    }
}

