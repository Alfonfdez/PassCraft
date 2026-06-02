using Microsoft.Maui.Controls;

namespace PassCraft.UI.Views
{
    /// <summary>
    /// Provides a foundational implementation for application pages, 
    /// encapsulating shared utility logic and consistent UI behaviors.
    /// </summary>
    public class BasePage : ContentPage
    {
        /// <summary>
        /// Toggles the application's runtime theme between Light and Dark visual profiles.
        /// </summary>
        /// <param name="sender">The interactive UI element triggering the theme transition.</param>
        /// <param name="e">The associated contextual event parameters.</param>
        protected void OnThemeToggleClicked(object? sender, EventArgs e)
        {
            if (Application.Current == null) return;

            Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;
        }
    }
}
