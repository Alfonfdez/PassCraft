using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
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

#if ANDROID
            // Fix for Android native underline visibility in Light Mode
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NativeUnderlineColorFix", (handler, view) =>
            {
                // Corregido: Iniciales en mayúscula para el espacio de nombres nativo de Android
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Microsoft.Maui.Graphics.Color.FromArgb("#877B66").ToPlatform());
            });

            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NativeUnderlineColorFix", (handler, view) =>
            {
                // Corregido: Iniciales en mayúscula para el espacio de nombres nativo de Android
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Microsoft.Maui.Graphics.Color.FromArgb("#877B66").ToPlatform());
            });
#endif
            return builder.Build();
        }
    }
}
