using FoodClient.ViewModels;
using FoodClient.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                var expiration = await SecureStorage.GetAsync("Expiration");
                var parsedDate = DateTime.Parse(expiration);
                DateTime AddedTime = parsedDate.AddHours(2);
                DateTime currentTime = DateTime.Now;

                int comparisonResult = DateTime.Compare(AddedTime, currentTime);

                if (comparisonResult > 0)
                {
                    var mainPageViewModel = Services.GetService<MainPageViewModel>();
                    var mainPage = new MainPage(mainPageViewModel);
                    await Shell.Current.GoToAsync("main");
                }
                else
                {
                    await Shell.Current.GoToAsync("login");


                }


            }
            else
            {
                await Shell.Current.GoToAsync("login");

            }
        }
    }
}
