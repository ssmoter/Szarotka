namespace Inventory.Pages.SingleDayPreview.SingleDayPreviewSmall
{
    public partial class TransparentToColorGraphicsView : GraphicsView
    {
        public static readonly BindableProperty ToColorProperty
            = BindableProperty.Create(nameof(ToColor), typeof(Color), typeof(TransparentToColorGraphicsView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is TransparentToColorGraphicsView view)
                {
                    if (newValue is Color color)
                    {
                        view._drawable.ToColor = color;
                        view.Invalidate();
                    }
                }
            });
        public Color ToColor
        {
            get => (Color)GetValue(ToColorProperty);
            set => SetValue(ToColorProperty, value);
        }

        private readonly TransparentToColorDrawable _drawable;
        public TransparentToColorGraphicsView()
        {
            _drawable = new();
            base.Drawable = _drawable;
        }
    }



    public class TransparentToColorDrawable : IDrawable
    {
        public Color ToColor = Colors.Transparent;
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            LinearGradientPaint linearGradientPaint = new()
            {
                StartColor = Colors.Transparent,
                EndColor = ToColor,
                // StartPoint is already (0,0)
                EndPoint = new Point(1, 0)
            };

            //RectF linearRectangle = new RectF(10, 10, 200, 100);
            RectF linearRectangle = dirtyRect;
            canvas.SetFillPaint(linearGradientPaint, linearRectangle);
            canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
            canvas.FillRoundedRectangle(linearRectangle, 0);
        }
    }
}
