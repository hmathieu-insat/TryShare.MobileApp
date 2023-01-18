﻿using CommunityToolkit.Mvvm.Messaging;
using INSAT._4I4U.TryShare.MobileApp.Message;
using INSAT._4I4U.TryShare.MobileApp.Model;
using INSAT._4I4U.TryShare.MobileApp.Services.Tricycles;
using INSAT._4I4U.TryShare.MobileApp.View;
using INSAT._4I4U.TryShare.MobileApp.ViewModel.Base;

namespace INSAT._4I4U.TryShare.MobileApp.ViewModel
{
    [QueryProperty(nameof(Tricycle), "Tricycle")]
    public partial class TricycleUnlockingViewModel : BaseViewModel
    {

        public IBookingService _bookingService;

        [ObservableProperty]
        Tricycle tricycle;

        public TricycleUnlockingViewModel(IBookingService bookingService)
        {
            this._bookingService = bookingService;
        }

        [RelayCommand]
        public async Task GoToMainPageAsync(Tricycle tricycle)
        {
            await Shell.Current.Navigation.PopToRootAsync();
            IsPopupVisible = false;

            WeakReferenceMessenger.Default.Send(new BookingCompletedMessage());
        }

        [RelayCommand]
        public async Task GoToReturnPageAsync()
        {
            await _bookingService.RequestTricycleBookingAsync(Tricycle);
        }
    }
}
