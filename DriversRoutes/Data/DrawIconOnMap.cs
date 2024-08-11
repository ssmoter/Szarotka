using Microsoft.Maui.Graphics.Skia;

using SkiaSharp;

using System.Reflection.Metadata.Ecma335;

namespace DriversRoutes.Data
{
    public class DrawIconOnMap : IDisposable, IDrawable
    {
        public int Number { get; set; }
        public Color ColorFill { get; set; } = new Color(234, 67, 53, 255);
        public Color ColorBackground { get; set; } = new Color(179, 20, 18, 255);
        public Microsoft.Maui.Graphics.IImage Image { get; set; }

        public float ScaleX { get; set; } = 0;
        public float ScaleY { get; set; } = 0;

        public void Dispose()
        {
            ColorFill = null;
            ColorBackground = null;
            Image.Dispose();
            GC.SuppressFinalize(this);
        }


        public MemoryStream DrawWithImage(Microsoft.Maui.Graphics.IImage image = null)
        {
            image ??= Image;
            var height = (int)image.Height + 15;

            using var skiaBitmapExportContext = new SkiaBitmapExportContext((int)image.Width, height, 1, 72, true, true);
            //Create a SKCanvas

            using var bitmap = new SKBitmap((int)image.Width, height, colorType: SKColorType.RgbaF16Clamped, alphaType: SKAlphaType.Premul);

            using var skCanvas = new SKCanvas(bitmap);

            //Draw an image

            using var stream0 = new MemoryStream(image.AsBytes());

            using var skImage = SKImage.FromEncodedData(stream0);
            stream0.Position = 0;

            skCanvas.DrawImage(skImage, new SKPoint(0, 16));

            //Get a Microsoft.Maui.Graphics.ICanvas used by Maui to draw

            ICanvas canvas = skiaBitmapExportContext.Canvas;

            // Draw a red line using Maui 
            canvas.StrokeSize = 6;
            canvas.FontSize = 20;
            canvas.FontColor = ColorFill;
            canvas.StrokeColor = ColorFill;
            canvas.FillColor = ColorFill;

            canvas.DrawString(Number.ToString(), image.Width / 2, 16, HorizontalAlignment.Center);

            canvas.SaveState();

            //Create a stream to save the draw on ICanvas to SKCanvas

            using var stream1 = new MemoryStream();

            skiaBitmapExportContext.WriteToStream(stream1);

            stream1.Position = 0; // Reset position to start

            //Send draw as image with transparence to SKCanvas applying over image sent before

            using var skImageApliques = SKImage.FromEncodedData(stream1);

            skCanvas.DrawImage(skImageApliques, new SKPoint(0, 0));

            //Upload to Azure Blob Storage or wherever you want

            var stream2 = new MemoryStream();

            bitmap.Encode(stream2, SKEncodedImageFormat.Jpeg, 100);

            stream2.Position = 0; // Reset position to start
            return stream2;
        }


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();       
            canvas.StrokeSize = 1;
            canvas.FontSize = 20;
            canvas.FontColor = ColorFill;
            canvas.StrokeColor = ColorFill;
            canvas.FillColor = ColorFill;

            //canvas.DrawRectangle(0,0,dirtyRect.Width,dirtyRect.Height);
            //canvas.DrawLine(dirtyRect.Center.X, 0, dirtyRect.Center.X, dirtyRect.Height);
            canvas.Translate(dirtyRect.Center.X, 0);
            canvas.Scale(ScaleX, ScaleY);

            canvas.Translate(0,3);
            canvas.DrawString(Number.ToString(), 0f, 15, HorizontalAlignment.Center);
            canvas.SaveState();

            canvas.Translate(0, 12);

            DrawPoint(canvas, dirtyRect);
            canvas.ResetState();
       
        }


        private void DrawPoint(ICanvas canvas, RectF dirtyRect)
        {
            canvas.Translate(-23,0);
            PathF path = new(11, 24);
            path.LineTo(23, 42);
            path.LineTo(24, 42);
            path.LineTo(35, 24);
            path.LineTo(11, 24);

            canvas.DrawPath(path);
            canvas.FillPath(path);

            canvas.Translate(23.1f, 19);
            canvas.DrawCircle(0, 0, 27 / 2);
            canvas.FillCircle(0, 0, 27 / 2);
            canvas.DrawCircle(0, 0, 9 / 2);
            canvas.FillColor = ColorBackground;
            canvas.FillCircle(0, 0, 9 / 2);
        }
    }
}