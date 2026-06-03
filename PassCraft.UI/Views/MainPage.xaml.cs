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
    public partial class MainPage : BasePage
    {
        /// <summary>
        /// The decoupled generation engine utilized to build mathematically random character sequences.
        /// </summary>
        private readonly IPasswordGenerationService _generationService;

        /// <summary>
        /// The domain validation engine utilized to dynamically evaluate real-time character matrix configurations.
        /// </summary>
        private readonly IPasswordValidationService _validationService;

        /// <summary>
        /// The persistent tracking orchestrator used to log securely authorized password outputs.
        /// </summary>
        private readonly IPasswordHistoryService _historyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class with required business logic engine injections 
        /// and evaluates initial filter rules to synchronize startup UI component states.
        /// </summary>
        /// <param name="generationService">The injected engine handling password generation routines.</param>
        /// <param name="validationService">The injected validation service evaluating character pool viability.</param>
        /// <param name="historyService">The injected logging instance tracking generation history records.</param>
        public MainPage(IPasswordGenerationService generationService, IPasswordValidationService validationService, IPasswordHistoryService historyService)
        {
            InitializeComponent();
            _generationService = generationService;
            _validationService = validationService;
            _historyService = historyService;

            // Run initial validation to sync all button states with the starting data
            ValidateFilters();
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
        /// Event handler triggered when the custom symbols entry field content changes.
        /// Re-evaluates validation states to handle scenarios where all symbol characters are deleted.
        /// </summary>
        /// <param name="sender">The interactive control source triggering the event.</param>
        /// <param name="e">The text changed event arguments.</param>
        private void OnSymbolsTextChanged(object? sender, TextChangedEventArgs e) => ValidateFilters();

        /// <summary>
        /// Fills the excluded characters entry with a predefined list of 
        /// visually similar or easily confused characters.
        /// </summary>
        private void OnFillAmbiguousClicked(object? sender, EventArgs e)
        {
            ExcludedEntry.Text = Constants.CharacterPools.AmbiguousCharacters;
        }

        /// <summary>
        /// Evaluates UI criteria against the validation domain service and updates control interactability.
        /// </summary>
        private void ValidateFilters()
        {
            if (GenerateBtn == null) return;

            // 1. Handle primary Generation Button eligibility state
            GenerateBtn.IsEnabled = _validationService.IsPoolValid(
                UpperSwitch.IsToggled,
                LowerSwitch.IsToggled,
                NumbersSwitch.IsToggled,
                SymbolsSwitch.IsToggled,
                SymbolsEntry?.Text,
                ExcludedEntry?.Text
            );

            // 2. Handle Reset Symbols button visibility (Invisible if current input matches the default reference)
            if (ResetSymbolsBtn != null && SymbolsEntry != null)
            {
                bool isAlreadyDefault = _validationService.AreCharacterSetsEquivalent(
                    SymbolsEntry.Text,
                    Constants.CharacterPools.DefaultSymbols);

                ResetSymbolsBtn.IsVisible = !isAlreadyDefault;
            }

            // 3. Handle Ambiguous shortcut button visibility (Invisible if current input matches the ambiguity reference)
            if (AmbiguousBtn != null && ExcludedEntry != null)
            {
                bool isAlreadyAmbiguous = _validationService.AreCharacterSetsEquivalent(
                    ExcludedEntry.Text,
                    Constants.CharacterPools.AmbiguousCharacters);

                AmbiguousBtn.IsVisible = !isAlreadyAmbiguous;
            }
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
