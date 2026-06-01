using System.Security.Cryptography;
using System.Text;
using CommunityToolkit.Maui.Alerts; // Required for the new Toast API
using CommunityToolkit.Maui.Core;   // Required for Toast duration options

namespace PassCraft
{
    public partial class MainPage : ContentPage
    {
        private const string DefaultSymbols = "!@#$%^&*()-_=+[]{};:,.<>/?";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string NumberChars = "0123456789";

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnGenerateClicked(object sender, EventArgs e)
        {
            StringBuilder charPool = new StringBuilder();

            if (UpperSwitch.IsToggled) charPool.Append(UppercaseChars);
            if (LowerSwitch.IsToggled) charPool.Append(LowercaseChars);
            if (NumbersSwitch.IsToggled) charPool.Append(NumberChars);
            if (SymbolsSwitch.IsToggled) charPool.Append(SymbolsEntry.Text);

            if (charPool.Length == 0) return;

            int passwordLength = (int)LengthSlider.Value;
            char[] password = new char[passwordLength];
            string pool = charPool.ToString();

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = pool[RandomNumberGenerator.GetInt32(pool.Length)];
            }

            PasswordEntry.Text = new string(password);
        }

        private async void OnCopyClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordEntry.Text)) return;

            await Clipboard.Default.SetTextAsync(PasswordEntry.Text);

            // UPGRADE: Native unobtrusive Toast notification instead of a blocking popup
            var toast = Toast.Make("Password copied to clipboard!", ToastDuration.Short, 14);
            await toast.Show();
        }

        private void OnResetSymbolsClicked(object sender, EventArgs e)
        {
            // Restores the original string configuration to the UI field
            SymbolsEntry.Text = DefaultSymbols;
        }

        private void OnSymbolsToggled(object sender, ToggledEventArgs e)
        {
            if (SymbolsEditorLayout != null)
            {
                SymbolsEditorLayout.IsVisible = e.Value;
            }

            ValidateFilters();
        }

        private void OnFilterToggled(object sender, ToggledEventArgs e)
        {
            ValidateFilters();
        }

        private void ValidateFilters()
        {
            if (GenerateBtn == null) return;

            GenerateBtn.IsEnabled = UpperSwitch.IsToggled ||
                                    LowerSwitch.IsToggled ||
                                    NumbersSwitch.IsToggled ||
                                    SymbolsSwitch.IsToggled;
        }
    }
}
