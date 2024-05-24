using CommunityToolkit.Mvvm.Input;
using FoodClient.ViewModels;
using FoodClient.Views;
using FoodForRequestApp.Services;
using System.Windows.Input;

namespace FoodClient
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();



            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("main", typeof(MainPage));
            Routing.RegisterRoute("FoodRequestForUser", typeof(FoodRequestForUserViewModel));

        }

       
      

    }
}
