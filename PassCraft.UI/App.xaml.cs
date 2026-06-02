using Microsoft.Extensions.DependencyInjection;

namespace PassCraft.UI
{
    /// <summary>
    /// The master application bootstrapper class for PassCraft.
    /// Manages application-level lifecycle states and sets up the primary window display tree.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class and parses global XAML style resources.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Native initialization hook executed by .NET MAUI when creating the primary desktop or mobile window view.
        /// </summary>
        /// <param name="activationState">The state management arguments passed by the underlying host operating system.</param>
        /// <returns>A new <see cref="Window"/> configured to render the root application shell structure.</returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Instantiates a new window wrapper driven entirely by our customized AppShell routing tree
            return new Window(new AppShell());
        }
    }
}
