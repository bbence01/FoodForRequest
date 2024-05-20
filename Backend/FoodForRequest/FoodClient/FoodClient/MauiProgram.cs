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
            builder.Services.AddSingleton<IRestService, RestService>();
            builder.Services.AddTransient<MainPageViewModel>();

            builder.Services.AddTransient<MainPageViewModel>();

          

            return builder.Build();
        }
    }
}
