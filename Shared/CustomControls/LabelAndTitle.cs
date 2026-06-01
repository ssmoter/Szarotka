namespace Shared.CustomControls
{
    public partial class LabelAndTitle : Grid
    {

        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(LabelAndTitle),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                if (bindable is LabelAndTitle view)
                {
                    view._title.Text = (string)newValue;
                }
            });
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);

        }
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(
                nameof(Content),
                typeof(object),
                typeof(LabelAndTitle),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                if (bindable is LabelAndTitle view)
                {
                    if (newValue is not IView && newValue is not null)
                    {
                        view.Remove(view._content);
                        view._content = new Label
                        {
                            Text = newValue.ToString(),
                            LineBreakMode = LineBreakMode.CharacterWrap,
                        };
                        view.Add(view._content, 0, 1);
                    }
                    else if (newValue is IView control)
                    {
                        view.Remove(view._content);
                        view._content = control;
                        view.Add(view._content, 0, 1);
                    }
                }
            });

        public object Content
        {
            get => (object)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);

        }
        private readonly Label _title;
        private IView _content;

        public LabelAndTitle()
        {
            _title = new Label();
            _title.FontSize *= 0.8;
            _title.TextDecorations = TextDecorations.Underline;

            this.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            this.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            this.Add(_title, 0, 0);

        }

    }
}
