using FoodClient.ViewModels;
using FoodClient.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;

namespace FoodClient
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
            Services = services;

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            await CheckAuthenticationStatusAsync();
        }

        private async Task CheckAuthenticationStatusAsync()
        {
            var authToken = await SecureStorage.GetAsync("AuthToken");
            if (!string.IsNullOrEmpty(authToken))
            {
                var mainPageViewModel = Services.GetService<MainPageViewModel>();
                var mainPage = new MainPage(mainPageViewModel);
                await Shell.Current.GoToAsync("main");

            }
            else
            {
                //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                await Shell.Current.GoToAsync("login");

            }
        }
    }
}
