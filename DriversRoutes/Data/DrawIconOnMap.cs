using DriversRoutes.Model;

using Microsoft.Maui.Graphics.Skia;

using SkiaSharp;

using System.Xml.Linq;

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
            Image?.Dispose();
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

            canvas.Translate(0, 3);
            canvas.DrawString(Number.ToString(), 0f, 15, HorizontalAlignment.Center);
            canvas.SaveState();

            canvas.Translate(0, 12);

            DrawPoint(canvas, dirtyRect);
            canvas.ResetState();

        }


        private void DrawPoint(ICanvas canvas, RectF dirtyRect)
        {
            canvas.Translate(-23, 0);
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


        public static string GenerateHtmlImgFromBase64String(string base64String)
        {
            return $"<img src=\"data:image/png;base64, {base64String}\"/>";
        }
        public static string[] GenerateBase64StringPins(int length)
        {
            var pins = new string[length];
            var pinSize = GetScale();
            DrawIconOnMap drawIconOnMap = new()
            {
                ScaleX = pinSize.ScaleX,
                ScaleY = pinSize.ScaleY,
            };
            for (int i = 0; i < length; i++)
            {
                SkiaBitmapExportContext skiaBitmapExportContext = new(pinSize.Width, pinSize.Height, 1);
                drawIconOnMap.Number = i + 1;
                var pin = GetNewPin(skiaBitmapExportContext, drawIconOnMap);
                pins[i] = pin;
            }
            return pins;
        }
        public static string GenerateBase64StringPin(int number)
        {
            var pinSize = GetScale();
            SkiaBitmapExportContext skiaBitmapExportContext = new(pinSize.Width, pinSize.Height, 1);
            DrawIconOnMap drawIconOnMap = new()
            {
                Number = number,
                ScaleX = pinSize.ScaleX,
                ScaleY = pinSize.ScaleY,
            };
            var pin = GetNewPin(skiaBitmapExportContext, drawIconOnMap);
            return pin;
        }
        public static ImageSource GetImagePin(int number)
        {
            var pinSize = GetScale();
            SkiaBitmapExportContext skiaBitmapExportContext = new(pinSize.Width, pinSize.Height, 1);
            DrawIconOnMap drawIconOnMap = new()
            {
                Number = number,
                ScaleX = pinSize.ScaleX,
                ScaleY = pinSize.ScaleY,
            };
            var pin = ImageStream(skiaBitmapExportContext, drawIconOnMap);
            return ImageSource.FromStream(() => skiaBitmapExportContext.Image.AsStream());
        }
        public static ImageSource GetImagePin(int number, Color colorFill, Color colorBackground)
        {
            var pinSize = GetScale();
            SkiaBitmapExportContext skiaBitmapExportContext = new(pinSize.Width, pinSize.Height, 1);
            DrawIconOnMap drawIconOnMap = new()
            {
                Number = number,
                ScaleX = pinSize.ScaleX,
                ScaleY = pinSize.ScaleY,
                ColorFill = colorFill,
                ColorBackground = colorBackground,

            };
            var pin = ImageStream(skiaBitmapExportContext, drawIconOnMap);
            return ImageSource.FromStream(() => skiaBitmapExportContext.Image.AsStream());
        }

        private static string GetNewPin(SkiaBitmapExportContext skiaBitmapExportContext, DrawIconOnMap drawIconOnMap)
        {
            var stream = ImageStream(skiaBitmapExportContext, drawIconOnMap);
            using var memory = new MemoryStream();
            stream.CopyTo(memory);
            var pin = Convert.ToBase64String(memory.ToArray());
            return pin;
        }

        private static Stream ImageStream(SkiaBitmapExportContext skiaBitmapExportContext, DrawIconOnMap drawIconOnMap)
        {
            ICanvas canvas = skiaBitmapExportContext.Canvas;
            drawIconOnMap.Draw(canvas, new RectF(0, 0, skiaBitmapExportContext.Width, skiaBitmapExportContext.Height));
            var stream = skiaBitmapExportContext.Image.AsStream();
            return stream;
        }

        private static PinSize GetScale()
        {
            var scaleX = (int)DeviceDisplay.Current.MainDisplayInfo.Density;
            var scaleY = scaleX;
            var width = 40 * scaleX;
            var height = 58 * scaleY;

            return new PinSize(scaleX, scaleY, width, height);
        }



        public static string CreatePinSvg(string text)
        {
            XElement svg = CreatedSvg(text);

            // Konwertowanie SVG do string
            return svg.ToString();
        }
        public static string CreatePinSvg(int text)
        {
            XElement svg = CreatedSvg(text.ToString());

            // Konwertowanie SVG do string
            return svg.ToString();
        }
        private static XElement CreatedSvg(string text)
        {
            XNamespace ns = "http://www.w3.org/2000/svg";

            var svg = new XElement(ns + "svg",
                new XAttribute("xmlns", ns),
                new XAttribute("viewBox", "0 0 24 24"),
                new XAttribute("width", "24"),
                new XAttribute("height", "48"),
                new XElement(ns + "path",
                    new XAttribute("d", "M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z"),
                    new XAttribute("fill", "#ea4335") // Czerwony kolor pinu
                ),
                new XElement(ns + "circle",
                    new XAttribute("cx", "12"),
                    new XAttribute("cy", "9"),
                    new XAttribute("r", "2.5"),
                    new XAttribute("fill", "#ffffff") // Biały kolor wewnętrznego koła
                ),
                new XElement(ns + "text",
                    new XAttribute("x", "12"),
                    new XAttribute("y", "2"), // Pozycja tekstu nad pinem
                    new XAttribute("text-anchor", "middle"),
                    new XAttribute("font-size", "15"),
                    new XAttribute("fill", "#ea4335"), // Czarny kolor tekstu
                    text // Przekazany parametr tekstowy
                )
            );
            return svg;
        }
    }
}