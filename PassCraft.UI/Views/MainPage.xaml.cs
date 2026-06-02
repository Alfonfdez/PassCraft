using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PassCraft.Core.Constants;
using PassCraft.Core.Contracts;
using System.Text;

namespace PassCraft.UI.Views
{
    /// <summary>
    /// Interaction logic for the main password generation dashboard interface.
    /// Handles character criteria aggregation, configuration state validation, and dispatching tasks to background core services.
    /// </summary>
    public partial class MainPage : BasePage
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
            string finalPassword = _generationService.GeneratePassword(
                    (int)LengthSlider.Value,
                    UpperSwitch.IsToggled,
                    LowerSwitch.IsToggled,
                    NumbersSwitch.IsToggled,
                    SymbolsSwitch.IsToggled ? SymbolsEntry.Text : null,
                    ExcludedEntry?.Text
                );

            if (string.IsNullOrEmpty(finalPassword)) return;

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
        /// Event handler triggered when character category switches (Upper, Lower, Numbers) change state.
        /// Re-validates the password generation criteria to ensure a valid pool exists.
        /// </summary>
        /// <param name="sender">The interactive control source triggering the event.</param>
        /// <param name="e">The toggled event arguments.</param>
        private void OnFilterToggled(object? sender, ToggledEventArgs e) => ValidateFilters();

        /// <summary>
        /// Event handler triggered when the excluded characters entry field content changes.
        /// Re-evaluates criteria validation to ensure the exclusion rules do not empty the character pool.
        /// </summary>
        /// <param name="sender">The interactive control source triggering the event.</param>
        /// <param name="e">The text changed event arguments.</param>
        private void OnExcludedChanged(object? sender, TextChangedEventArgs e) => ValidateFilters();

        /// <summary>
        /// Fills the excluded characters entry with a predefined list of 
        /// visually similar or easily confused characters.
        /// </summary>
        private void OnFillAmbiguousClicked(object? sender, EventArgs e)
        {
            ExcludedEntry.Text = Constants.CharacterPools.AmbiguousCharacters;
        }

        /// <summary>
        /// Evaluates the current configuration of character criteria and exclusion rules.
        /// Updates the <see cref="GenerateBtn"/> state to ensure the generation process 
        /// always has a valid, non-empty character pool to pull from.
        /// </summary>
        private void ValidateFilters()
        {
            if (GenerateBtn == null) return;

            // 1. Check if at least one category is active
            bool hasCategory = UpperSwitch.IsToggled || LowerSwitch.IsToggled ||
                               NumbersSwitch.IsToggled || SymbolsSwitch.IsToggled;

            if (!hasCategory)
            {
                GenerateBtn.IsEnabled = false;
                return;
            }

            // 2. Calculate the potential pool based on active toggles
            var sb = new StringBuilder();
            if (UpperSwitch.IsToggled) sb.Append(Constants.CharacterPools.Uppercase);
            if (LowerSwitch.IsToggled) sb.Append(Constants.CharacterPools.Lowercase);
            if (NumbersSwitch.IsToggled) sb.Append(Constants.CharacterPools.Numbers);
            if (SymbolsSwitch.IsToggled && !string.IsNullOrEmpty(SymbolsEntry.Text))
                sb.Append(SymbolsEntry.Text);

            string pool = sb.ToString();

            // 3. Filter out excluded characters to determine if a valid subset remains
            if (!string.IsNullOrEmpty(ExcludedEntry?.Text))
            {
                foreach (char c in ExcludedEntry.Text)
                {
                    pool = pool.Replace(c.ToString(), "");
                }
            }

            // 4. Button is only enabled if the resulting pool contains at least one character
            GenerateBtn.IsEnabled = pool.Length > 0;
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
