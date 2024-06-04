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
using Microsoft.Maui.Controls;

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

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            if (registerPasswordEntry.Text != registerConfirmPasswordEntry.Text)
            {
                registerMessageLabel.Text = "Passwords do not match.";
                registerMessageLabel.IsVisible = true;
                return;
            }

            var registerData = new RegisterViewModel
            {
                Email = registerEmailEntry.Text,
                FoodUserName = registerUserNameEntry.Text,
                Password = registerPasswordEntry.Text
            };

            var response = await AuthService.RegisterAsync(registerData);

            if (response)
            {
                registerMessageLabel.TextColor = Colors.Green;
                registerMessageLabel.Text = "Registration successful. You can now log in.";
                registerMessageLabel.IsVisible = true;
            }
            else
            {
                registerMessageLabel.TextColor = Colors.Red;
                registerMessageLabel.Text = "Registration failed. Please try again.";
                registerMessageLabel.IsVisible = true;
            }
        }
    }
}
