using PassCraft.Core.Constants;
using PassCraft.UI.Views;

namespace PassCraft.UI
{
    /// <summary>
    /// The core navigation backbone for PassCraft.
    /// Extends the native <see cref="Shell"/> engine to establish application layouts, 
    /// global hierarchies, and dynamic sub-page routing mapping schemas.
    /// </summary>
    public partial class AppShell : Shell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppShell"/> class.
        /// Sets up structural presentation layers and explicitly maps string tokens to concrete sub-view types.
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            // REGISTRATION: Maps our type-safe navigation route key directly to the HistoryPage view template
            Routing.RegisterRoute(
                Constants.NavigationRoutes.HistoryPage,
                typeof(HistoryPage));
        }
    }
}
