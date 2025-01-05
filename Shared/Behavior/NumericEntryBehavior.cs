namespace Shared.Behavior;
public partial class NumericEntryBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.TextChanged += OnEntryTextChanged;
    }

    protected override void OnDetachingFrom(Entry bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.TextChanged -= OnEntryTextChanged;
    }

    void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            bool isValid = e.NewTextValue.ToCharArray().All(char.IsDigit);

            var defoutColor = Application.Current.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.White;

            ((Entry)sender).TextColor = isValid ? defoutColor : Colors.Red;

            if (!isValid)
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
    }
}
