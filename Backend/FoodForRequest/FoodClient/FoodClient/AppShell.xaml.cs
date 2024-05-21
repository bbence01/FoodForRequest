using CommunityToolkit.Mvvm.Input;
using FoodClient.Views;
using FoodForRequestApp.Services;
using System.Windows.Input;

namespace FoodClient
{
    public partial class AppShell : Shell
    {
        public ICommand LogoutCommand { get; }

        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;


            LogoutCommand = new Command(async () => await Logout());

            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("main", typeof(MainPage));

        }

        private async Task Logout()
        {
            try
            {
                //await AuthService.LogoutAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during logout: {ex.Message}");
            }
            finally
            {
                SecureStorage.Remove("AuthToken");
                SecureStorage.Remove("UserId");

                await Current.GoToAsync("login");
            }
        }
    }
}
