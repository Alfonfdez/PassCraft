using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PassCraft.Core.Extensions;
using PassCraft.UI.Views;

namespace PassCraft.UI
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

            // 1. Core Architecture Layer Injections
            builder.Services.AddInfrastructureLayer();
            builder.Services.AddApplicationLayer();

            // 2. UI Pages Registration
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<HistoryPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
