using FoodClient.Models;
using FoodClient.Services;
using FoodForRequestApp.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using FoodClient.ViewModels;


namespace FoodClient.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var loginData = new LoginViewModel
            {
                UserName = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            var response = await AuthService.LoginAsync(loginData);

            if (response != null)
            {
                await SecureStorage.SetAsync("AuthToken", response.Token);
                await SecureStorage.SetAsync("UserId", response.Id);
                await SecureStorage.SetAsync("Expiration", response.Expiration.ToString());

                var mainPageViewModel = App.Services.GetService<MainPageViewModel>();
                await Navigation.PushAsync(new MainPage(mainPageViewModel));
            }
            else
            {
                messageLabel.Text = "Login failed. Please try again.";
                messageLabel.IsVisible = true;
            }
        }
    }
}

