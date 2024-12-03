using CommunityToolkit.Maui.Views;

namespace Shared.Pages.Popups.SubAddLastValue;

public partial class SubAddLastValueV : Popup
{
    public SubAddLastValueV(int oldValue, string title = "")
    {
        InitializeComponent();
        var vm = new SubAddLastValueVM();
        vm.SubAddLastValueM.OldValue = oldValue;
        vm.SubAddLastValueM.Title = title;
        vm.Close += CloseAsync;
        BindingContext = vm;
    }


    private void Entry_TextChanged_SetValueToSecendPosition(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            if (entry.Text.Length > 0 && entry.Text.Length <= 1)
            {
                entry.CursorPosition = 1;
            }

            if (!string.IsNullOrWhiteSpace(e.OldTextValue))
            {
                if (e.OldTextValue.Contains('.'))
                {
                    entry.Text = entry.Text.Replace('.', ',');
                    entry.CursorPosition = entry.Text.Length;
                }
            }
        }
    }


}