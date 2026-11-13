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

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is null)
        {
            return;
        }
        var entry = (Entry)sender;

        // Jeśli pole zostało wyczyszczone, pozwalamy na to i resetujemy kolor
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            var defaultColor = Application.Current?.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;
            entry.TextColor = defaultColor;
            return;
        }

        // Sprawdzamy czy tekst składa się tylko z cyfr
        bool isValid = e.NewTextValue.All(char.IsDigit);

        if (!isValid)
        {
            // Odpinamy zdarzenie, aby uniknąć nieskończonej pętli
            entry.TextChanged -= OnEntryTextChanged;

            // Przywracamy poprzednią poprawną wartość
            entry.Text = e.OldTextValue;

            // Zapinamy zdarzenie z powrotem
            entry.TextChanged += OnEntryTextChanged;
        }
        else
        {
            // Jeśli tekst jest poprawny, upewniamy się, że ma domyślny kolor
            var defaultColor = Application.Current?.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;
            entry.TextColor = defaultColor;
        }
    }
}
