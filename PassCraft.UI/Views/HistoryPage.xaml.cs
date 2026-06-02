using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PassCraft.Core.Constants;
using PassCraft.Core.Contracts;
using PassCraft.Core.Models;

namespace PassCraft.UI.Views
{
    /// <summary>
    /// Interaction logic behind the password logging panel interface.
    /// Binds historical track lists to streaming grids and handles individual row copy notifications.
    /// </summary>
    public partial class HistoryPage : BasePage
    {
        /// <summary>
        /// The tracking service used to query the snapshot historical logs.
        /// </summary>
        private readonly IPasswordHistoryService _historyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryPage"/> class with a history tracking engine dependency injection.
        /// </summary>
        /// <param name="historyService">The injected logging tracker tracking structural history logs.</param>
        public HistoryPage(IPasswordHistoryService historyService)
        {
            InitializeComponent();
            _historyService = historyService;
        }

        /// <summary>
        /// Native hook executed every time this page view model returns to focus on the viewport.
        /// Refreshes data sources from the logging stack.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            HistoryCollectionView.ItemsSource = _historyService.GetHistory();
        }

        /// <summary>
        /// Event handler executed when tapping the copy action on a row item.
        /// Resolves the bound data context, transmits the password to the system clipboard, 
        /// and displays an indexed toast notification.
        /// </summary>
        /// <param name="sender">The interactive UI element triggering the copy action.</param>
        /// <param name="e">The associated contextual event parameters.</param>
        private async void OnCopyRowClicked(object? sender, EventArgs e)
        {
            if (sender is VisualElement element && element.BindingContext is PasswordItem selectedItem)
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
