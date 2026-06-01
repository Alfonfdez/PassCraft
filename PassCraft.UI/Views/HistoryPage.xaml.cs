using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PassCraft.Core.Constants;
using PassCraft.Core.Contracts;
using PassCraft.Core.Models;

namespace PassCraft.UI.Views
{
    public partial class HistoryPage : ContentPage
    {
        private readonly IPasswordHistoryService _historyService;

        public HistoryPage(IPasswordHistoryService historyService)
        {
            InitializeComponent();
            _historyService = historyService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HistoryCollectionView.ItemsSource = _historyService.GetHistory();
        }

        private async void OnCopyRowClicked(object? sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is PasswordItem selectedItem)
            {
                if (string.IsNullOrWhiteSpace(selectedItem.Password)) return;

                await Clipboard.Default.SetTextAsync(selectedItem.Password);

                string toastMessage = string.Format(
                    Constants.UIConfig.HistoryCopiedToastTemplate,
                    selectedItem.Index);

                await Toast.Make(
                    toastMessage,
                    ToastDuration.Short,
                    Constants.UIConfig.ToastFontSize
                ).Show();
            }
        }
    }
}