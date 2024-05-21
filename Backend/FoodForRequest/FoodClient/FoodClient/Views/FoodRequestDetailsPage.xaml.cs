using FoodClient.ViewModels;

namespace FoodClient.Views;

public partial class FoodRequestDetailsPage : ContentPage
{
    FoodRequestDetailsViewModel viewModel;


    public FoodRequestDetailsPage(FoodRequestDetailsViewModel viewModel)
	{
        InitializeComponent();

        this.viewModel = viewModel;

        BindingContext = viewModel;

        this.Appearing += MainPage_Appearing;
    }

    private async void MainPage_Appearing(object sender, EventArgs e)
    {
        await viewModel.LoadFoodRequestDetailsAsync(viewModel.FoodRequest.Id);

    }



}