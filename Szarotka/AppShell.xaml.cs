using Szarotka.Service;

namespace Szarotka
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