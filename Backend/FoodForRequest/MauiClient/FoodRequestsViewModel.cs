using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MauiClient
{

    public partial class FoodRequestsViewModel : ObservableObject
    {
        public ObservableCollection<FoodRequest> FoodRequests { get; } = new ObservableCollection<FoodRequest>();

        [ObservableProperty]
        private FoodRequest selectedRequest;

        [ObservableProperty]
        private bool isLoading;

        public FoodRequestsViewModel()
        {
           // LoadFoodRequestsCommand = new AsyncRelayCommand(LoadFoodRequestsAsync);
            //ViewRequestDetailsCommand = new AsyncRelayCommand(ViewRequestDetailsAsync);
           // SaveRequestCommand = new AsyncRelayCommand(SaveRequestAsync);
        }

        [RelayCommand]
        async Task LoadFoodRequestsAsync()
        {
            IsLoading = true;
            var requests = await FoodService.GetFoodRequestsAsync();
            FoodRequests.Clear();
            requests.ForEach(request => FoodRequests.Add(request));
            IsLoading = false;
        }

        [RelayCommand]
        async Task ViewRequestDetailsAsync()
        {
            // This command would be triggered when a user selects an item from the list.
            SelectedRequest = await FoodService.GetFoodRequestByIdAsync(SelectedRequest.Id);
        }

        [RelayCommand]
        async Task SaveRequestAsync()
        {
            // Save the edited details of the request
            await FoodService.UpdateFoodRequestAsync(SelectedRequest);
        }
    }
}
