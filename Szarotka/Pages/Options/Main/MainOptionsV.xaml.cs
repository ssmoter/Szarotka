using System.Reflection;

namespace Szarotka.Pages.Options.Main;

public partial class MainOptionsV : ContentPage
{
    public MainOptionsV(MainOptionsVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}