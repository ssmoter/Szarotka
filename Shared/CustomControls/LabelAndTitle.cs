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
                typeof(string),
                typeof(LabelAndTitle),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                if (bindable is LabelAndTitle view)
                {
                    view._content.Text = (string)newValue;
                }
            });

        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);

        }
        private readonly Label _title;
        private readonly Label _content;

        public LabelAndTitle()
        {
            _title = new Label();
            _title.FontSize *= 0.8;
            _title.TextDecorations = TextDecorations.Underline; 
            _content = new Label()
            {
                LineBreakMode = LineBreakMode.CharacterWrap,
            };

            this.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            this.AddRowDefinition(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });            

            this.Add(_title, 0, 0);
            this.Add(_content, 0, 1);
        }

    }
}
