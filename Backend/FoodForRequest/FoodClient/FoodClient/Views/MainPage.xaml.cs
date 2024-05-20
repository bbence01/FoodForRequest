
using FoodClient.Models;
using FoodClient.Services;
using FoodClient.ViewModels;

namespace FoodClient.Views

{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            Console.WriteLine("MainPage Initialized"); 

        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFoodRequest = e.CurrentSelection.FirstOrDefault() as FoodRequest;
            if (selectedFoodRequest != null)
            {
                var viewModel = new FoodRequestDetailsViewModel(new RestService("http://localhost:5274/"));
                await viewModel.LoadFoodRequestDetailsAsync(selectedFoodRequest.Id);
                var detailsPage = new FoodRequestDetailsPage { BindingContext = viewModel };
                await Navigation.PushAsync(detailsPage);
            }
        }
    }
}
