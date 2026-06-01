using PassCraft.Core.Constants;
using PassCraft.UI.Views;

namespace PassCraft.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
                Constants.NavigationRoutes.HistoryPage,
                typeof(HistoryPage));
        }
    }
}
