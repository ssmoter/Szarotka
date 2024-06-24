using SzarotkaBlazor.Service;

namespace SzarotkaBlazor
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RoutingCollectionExtensions.AddRoutings();

        }
    }
}
