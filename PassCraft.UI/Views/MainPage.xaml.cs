using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PassCraft.Core.Constants;
using PassCraft.Core.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace PassCraft.UI.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly IPasswordHistoryService _historyService;

        public MainPage(IPasswordHistoryService historyService)
        {
            InitializeComponent();
            _historyService = historyService;
        }

        private void OnGenerateClicked(object? sender, EventArgs e)
        {
            StringBuilder charPool = new StringBuilder();

            if (UpperSwitch.IsToggled) charPool.Append(Constants.CharacterPools.Uppercase);
            if (LowerSwitch.IsToggled) charPool.Append(Constants.CharacterPools.Lowercase);
            if (NumbersSwitch.IsToggled) charPool.Append(Constants.CharacterPools.Numbers);
            if (SymbolsSwitch.IsToggled) charPool.Append(SymbolsEntry.Text);

            if (charPool.Length == 0) return;

            int passwordLength = (int)LengthSlider.Value;
            char[] password = new char[passwordLength];
            string pool = charPool.ToString();

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = pool[RandomNumberGenerator.GetInt32(pool.Length)];
            }

            string finalPassword = new string(password);
            PasswordEntry.Text = finalPassword;

            _historyService.AddPassword(finalPassword);
        }

        private async void OnCopyClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordEntry.Text)) return;
            await Clipboard.Default.SetTextAsync(PasswordEntry.Text);
            await Toast.Make(Constants.UIConfig.CopiedToastMessage, ToastDuration.Short, Constants.UIConfig.ToastFontSize).Show();
        }

        private void OnResetSymbolsClicked(object? sender, EventArgs e) =>
            SymbolsEntry.Text = Constants.CharacterPools.DefaultSymbols;

        private void OnSymbolsToggled(object? sender, ToggledEventArgs e)
        {
            if (SymbolsEditorLayout != null) SymbolsEditorLayout.IsVisible = e.Value;
            ValidateFilters();
        }

        private void OnFilterToggled(object? sender, ToggledEventArgs e) => ValidateFilters();

        private void ValidateFilters()
        {
            if (GenerateBtn == null) return;
            GenerateBtn.IsEnabled = UpperSwitch.IsToggled || LowerSwitch.IsToggled || NumbersSwitch.IsToggled || SymbolsSwitch.IsToggled;
        }

        private async void OnViewHistoryClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(Constants.NavigationRoutes.HistoryPage);
        }
    }
}
