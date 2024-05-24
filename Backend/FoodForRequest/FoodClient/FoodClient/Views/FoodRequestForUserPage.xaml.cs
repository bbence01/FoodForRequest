using FoodClient.Models;
using FoodClient.Services;
using FoodClient.ViewModels;

namespace FoodClient.Views;

public partial class FoodRequestForUserPage : ContentPage
{
    FoodRequestForUserViewModel viewModel;

    public FoodRequestForUserPage(FoodRequestForUserViewModel viewModel)
	{
        InitializeComponent();


        BindingContext = viewModel;

        this.Appearing += MainPage_Appearing;

    }

    private async void MainPage_Appearing(object sender, EventArgs e)
    {
        await viewModel.LoadFoodRequestsAsync();
        await viewModel.LoadAllIngredientsAsync();

    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedFoodRequest = e.CurrentSelection.FirstOrDefault() as FoodRequest;
        if (selectedFoodRequest != null)
        {
            var viewModel = new FoodRequestDetailsViewModel(new RestService("http://localhost:5274/"));
            await viewModel.LoadFoodRequestDetailsAsync(selectedFoodRequest.Id);
            var detailsPage = new FoodRequestDetailsPage(viewModel);
            await Navigation.PushAsync(detailsPage);
        }
    }

}