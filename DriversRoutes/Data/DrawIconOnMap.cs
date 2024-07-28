namespace DriversRoutes.Data
{
    public class DrawIconOnMap : IDisposable
    {
        public int Number { get; set; }
        public Color ColorFill { get; set; } = Colors.Black;
        public Color ColorBackground { get; set; } = Colors.White;

        public void Dispose()
        {
            ColorFill = null;
            ColorBackground = null;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FontColor = ColorFill;
            canvas.StrokeColor = ColorFill;
            canvas.FillColor = ColorFill;

            canvas.DrawString(Number.ToString(), dirtyRect.Center.X, 15, HorizontalAlignment.Center);
            canvas.Translate(0, 12);
            DrawPoint(canvas, dirtyRect);
        }


        private void DrawPoint(ICanvas canvas, RectF dirtyRect)
        {
            PathF path = new(11, 24);
            path.LineTo(23, 42);
            path.LineTo(24, 42);
            path.LineTo(36, 24);
            path.LineTo(11, 24);

            canvas.DrawPath(path);
            canvas.FillPath(path);

            canvas.Translate(23.5f, 19);
            canvas.DrawCircle(0, 0, 27 / 2);
            canvas.FillCircle(0, 0, 27 / 2);
            canvas.DrawCircle(0, 0, 9 / 2);
            canvas.FillColor = ColorBackground;
            canvas.FillCircle(0, 0, 9 / 2);
        }
    }
}