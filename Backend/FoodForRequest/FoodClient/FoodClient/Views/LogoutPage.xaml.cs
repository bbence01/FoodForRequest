using CommunityToolkit.Mvvm.Input;
using FoodForRequestApp.Services;

namespace FoodClient.Views;

public partial class LogoutPage : ContentPage
{
	public LogoutPage()
	{
		InitializeComponent();
	}

    [RelayCommand]
    private async Task Logout()
    {
        try
        {
            await AuthService.LogoutAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during logout: {ex.Message}");
        }
        finally
        {
            SecureStorage.Remove("AuthToken");
            SecureStorage.Remove("UserId");

            await Shell.Current.GoToAsync("login");
        }
    }
}