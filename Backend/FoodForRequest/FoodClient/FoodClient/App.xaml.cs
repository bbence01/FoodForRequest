using FoodClient.ViewModels;
using FoodClient.Views;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;


namespace FoodClient
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
            Services = services;

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override async void OnStart()
        {
           /* var authToken = await SecureStorage.GetAsync("AuthToken");
            if (!string.IsNullOrEmpty(authToken))
            {
                var mainPageViewModel = Services.GetService<MainPageViewModel>();
                MainPage = new NavigationPage(new MainPage(mainPageViewModel));
            }
            else*/
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
