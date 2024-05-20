
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
    }
}
