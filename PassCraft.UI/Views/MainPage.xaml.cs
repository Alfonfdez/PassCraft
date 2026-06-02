using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PassCraft.Core.Constants;
using PassCraft.Core.Contracts;

namespace PassCraft.UI.Views
{
    /// <summary>
    /// Interaction logic for the main password generation dashboard interface.
    /// Handles character criteria aggregation, configuration state validation, and dispatching tasks to background core services.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// The decoupled generation engine utilized to build mathematically random character sequences.
        /// </summary>
        private readonly IPasswordGenerationService _generationService;

        /// <summary>
        /// The persistent tracking orchestrator used to log securely authorized password outputs.
        /// </summary>
        private readonly IPasswordHistoryService _historyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class with required business logic engine injections.
        /// </summary>
        /// <param name="generationService">The injected engine handling password generation routines.</param>
        /// <param name="historyService">The injected logging instance tracking generation history records.</param>
        public MainPage(IPasswordGenerationService generationService, IPasswordHistoryService historyService)
        {
            InitializeComponent();
            _generationService = generationService;
            _historyService = historyService;
        }

        /// <summary>
        /// Event handler triggered when the primary password generation button is clicked.
        /// Compiles active UI filter parameters, requests a new string token from the core, and registers it into history.
        /// </summary>
        /// <param name="sender">The interactive control source triggering the event.</param>
        /// <param name="e">The associated contextual event parameters.</param>
        private void OnGenerateClicked(object? sender, EventArgs e)
        {
            // 1. Offload the complex generation logic cleanly to the service layer
            string finalPassword = _generationService.GeneratePassword(
                (int)LengthSlider.Value,
                UpperSwitch.IsToggled,
                LowerSwitch.IsToggled,
                NumbersSwitch.IsToggled,
                SymbolsSwitch.IsToggled ? SymbolsEntry.Text : null
            );

            if (string.IsNullOrEmpty(finalPassword)) return;

            // 2. Update UI and log to history
            PasswordEntry.Text = finalPassword;
            _historyService.AddPassword(finalPassword);
        }

        /// <summary>
        /// Event handler triggered when the clipboard copy action button is tapped.
        /// Forwards the active cleartext password content to the system pasteboard and displays a verification alert.
        /// </summary>
        /// <param name="sender">The interactive control source triggering the event.</param>
        /// <param name="e">The associated contextual event parameters.</param>
        private async void OnCopyClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordEntry.Text)) return;
            await Clipboard.Default.SetTextAsync(PasswordEntry.Text);
            await Toast.Make(Constants.UIConfig.CopiedToastMessage, ToastDuration.Short, Constants.UIConfig.ToastFontSize).Show();
        }

        /// <summary>
        /// Event handler triggered when clicking the symbol reset action button.
        /// Reverts custom structural configurations back to default application character rules.
        /// </summary>
        /// <param name="sender">The interactive control source triggering the event.</param>
        /// <param name="e">The associated contextual event parameters.</param>
        private void OnResetSymbolsClicked(object? sender, EventArgs e) =>
            SymbolsEntry.Text = Constants.CharacterPools.DefaultSymbols;

        /// <summary>
        /// Event handler triggered when the specialized symbols toggle switch changes state.
        /// Smoothly expands or collapses the inline interactive layout editor block and initiates validation filters.
        /// </summary>
        /// <param name="sender">The interactive control source triggering the event.</param>
        /// <param name="e">The associated contextual event parameters.</param>
        private void OnSymbolsToggled(object? sender, ToggledEventArgs e)
        {
            if (SymbolsEditorLayout != null) SymbolsEditorLayout.IsVisible = e.Value;
            ValidateFilters();
        }

        /// <summary>
        /// Standardized routing filter event tracking switch transitions across the criteria block.
        /// </summary>
        /// <param name="sender">The interactive control source triggering the event.</param>
        /// <param name="e">The associated contextual event parameters.</param>
        private void OnFilterToggled(object? sender, ToggledEventArgs e) => ValidateFilters();

        /// <summary>
        /// Evaluates structural switch metrics across the criteria array.
        /// Safety flags or completely locks out the generation button if no valid pools are checked.
        /// </summary>
        private void ValidateFilters()
        {
            if (GenerateBtn == null) return;
            GenerateBtn.IsEnabled = UpperSwitch.IsToggled || LowerSwitch.IsToggled || NumbersSwitch.IsToggled || SymbolsSwitch.IsToggled;
        }

        /// <summary>
        /// Event handler triggered when tapping the header theme configuration toggle button.
        /// Flips the application's runtime theme environment dynamically between Light and Dark visual profiles.
        /// </summary>
        private void OnThemeToggleClicked(object? sender, EventArgs e)
        {
            if (Application.Current == null) return;

            // Invert the current execution mode context
            Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;
        }

        /// <summary>
        /// Event handler linked to the top header toolbar navigation element.
        /// Commands the root shell engine to route forward onto the password logging interface.
        /// </summary>
        /// <param name="sender">The interactive control source triggering the event.</param>
        /// <param name="e">The associated contextual event parameters.</param>
        private async void OnViewHistoryClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(Constants.NavigationRoutes.HistoryPage);
        }
    }
}
