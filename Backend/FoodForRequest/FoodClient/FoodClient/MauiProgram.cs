using CommunityToolkit.Maui;
using FoodClient.Services;
using FoodClient.ViewModels;
using FoodClient.Views;
using Microsoft.Extensions.Logging;

namespace FoodClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();


#endif
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<FoodRequestCreatePage>();
            builder.Services.AddSingleton<FoodRequestCreateViewModel>();
            builder.Services.AddSingleton<FoodRequestDetailsPage>();
            builder.Services.AddSingleton<FoodRequestDetailsViewModel>();
            builder.Services.AddSingleton<FoodRequestUpdatePage>();
            builder.Services.AddSingleton<FoodRequestUpdateViewModel>();
            builder.Services.AddSingleton<FoodRequestForUserPage>();
            builder.Services.AddSingleton<FoodRequestForUserViewModel>();

            builder.Services.AddSingleton<IRestService, RestService>();
            builder.Services.AddTransient<MainPageViewModel>();

            builder.Services.AddTransient<FoodRequestForUserPage>();
            builder.Services.AddTransient<FoodRequestForUserViewModel>();



            return builder.Build();
        }
    }
}
