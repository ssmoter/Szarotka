using Shared.Helper;

using Inventory.Model;

namespace Inventory.Data.Draw
{
    public class DrawGraph : IDrawable, IDisposable
    {
        public GraphValues[] GraphValues { get; set; }
        public DateTime[] XValues { get; set; }
        public int TypeOfGraph { get; set; }

        protected float _max = 0;
        protected float _min = 0;

        protected float _fromLeft = 0;
        protected float _fromBotton = 20;
        public void Dispose()
        {
            if (GraphValues is not null)
            {
                for (int i = 0; i < GraphValues.Length; i++)
                {
                    GraphValues[i].Path.Dispose();
                }
            }
            GraphValues = null;
            XValues = null;
            GC.SuppressFinalize(this);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            try
            {
#if WINDOWS
                canvas.ResetState();
#endif
                canvas.StrokeColor = Colors.Green;
                canvas.StrokeSize = 5;

                var scale = AutoScale(dirtyRect);
                _fromLeft = 25 + _min.ToString().Length;

                var theme = Application.Current.UserAppTheme;

                canvas.FontSize = 10;
                if (theme is AppTheme.Dark)
                {
                    canvas.FontColor = Colors.White;
                }
                else if (theme is AppTheme.Light)
                {
                    canvas.FontColor = Colors.Black;
                }
                else
                {
                    theme = Application.Current.RequestedTheme;

                    if (theme is AppTheme.Dark)
                    {
                        canvas.FontColor = Colors.White;
                    }
                    else if (theme is AppTheme.Light)
                    {
                        canvas.FontColor = Colors.Black;
                    }

                }

                DrawX(canvas, dirtyRect, scale);

                DrawY(canvas, dirtyRect, scale);


                DrawNet(canvas, dirtyRect, scale);

                canvas.Translate(_fromLeft, dirtyRect.Height - _fromBotton);


                switch (TypeOfGraph)
                {
                    case 0:
                        DrawGraphColumn.DrawColumn(canvas, scale, GraphValues);
                        break;
                    case 1:
                        DrawGraphLine.DrawLine(canvas, scale, GraphValues);

                        break;
                    case 2:
                        DrawGraphPoint.DrawPoint(canvas, scale, GraphValues);

                        break;
                    case 3:
                        DrawGraphPoint.DrawPoint(canvas, scale, GraphValues);
                        DrawGraphLine.DrawLine(canvas, scale, GraphValues);
                        break;
                    default:
                        break;
                }


                // DrawLegend(canvas, dirtyRect);

                // DrawColumn(canvas, scale);

                //  DrawPoint(canvas, scale);

                //  DrawLine(canvas, scale);
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message + ex.StackTrace, "Ok");
            }
        }


        protected (float, float) AutoScale(RectF dirtyRect)
        {
            if (GraphValues is null)
                return (1, 1);
            if (GraphValues.Length == 0)
                return (1, 1);

            float scaleY = 1;
            float scaleX = 1;

            _max = GraphValues.Max(x => x.Path.Points.Max(x => x.Y));
            _min = GraphValues.Min(x => x.Path.Points.Min(x => x.Y));

            var first = 0;
            var last = GraphValues.FirstOrDefault().Path.Count;


            if (last == 0)
                last = 1;

            scaleX = (dirtyRect.Width - _fromLeft * 2) / (last - first);

            scaleY = (dirtyRect.Height - (_fromBotton * 2)) / (Math.Abs(_min) + Math.Abs(_max));


            var strokeSize = scaleX;

            return (scaleX, scaleY);

        }

        protected void DrawX(ICanvas canvas, RectF dirtyRect, (float, float) scale)
        {
            if (XValues is null)
                return;
            if (XValues.Length == 0)
                return;

            bool rotate = false;
            _fromBotton = 20;

            if (dirtyRect.Height > dirtyRect.Width)
            {
                rotate = true;
                _fromBotton = 60;
            }


            if (XValues.Length <= 8)
            {
                XDrawValues7(canvas, dirtyRect, scale, rotate);
            }
            else if (XValues.Length <= 32)
            {
                XDrawValues31(canvas, dirtyRect, scale, rotate);
            }
            else
            {
                XDrawValuesMore(canvas, dirtyRect, scale, rotate);
                _fromBotton = 60;
            }

            var start = new PointF(_fromLeft, dirtyRect.Height - _fromBotton);
            var end = new PointF(dirtyRect.Width - _fromLeft, dirtyRect.Height - _fromBotton);

            canvas.DrawLine(start, end);

        }
        protected void DrawY(ICanvas canvas, RectF dirtyRect, (float, float) scale)
        {
            var start = new PointF(_fromLeft, 0);
            var end = new PointF(_fromLeft, dirtyRect.Height - _fromBotton);

            canvas.DrawLine(start, end);
            YDrawValuesMore(canvas, dirtyRect, scale);
        }

        protected void DrawLine(ICanvas canvas, (float x, float y) scale)
        {
            if (GraphValues is null)
                return;
            if (GraphValues.Length == 0)
                return;


            // canvas.SaveState();
            canvas.Scale(scale.x, scale.y);


            var stroke = 1 / scale.x;

            if (stroke <= 0.01f)
            {
                stroke = 0.01f;
            }

            canvas.StrokeSize = stroke;

            for (int i = 0; i < GraphValues.Length; i++)
            {
                canvas.StrokeColor = GraphValues[i].Color.Color;
                canvas.DrawPath(GraphValues[i].Path);
            }

            canvas.RestoreState();
        }
        protected void DrawPoint(ICanvas canvas, (float x, float y) scale)
        {
            if (GraphValues is null)
                return;
            if (GraphValues.Length == 0)
                return;


            canvas.StrokeSize = 3;

            for (int i = 0; i < GraphValues.Length; i++)
            {
                canvas.StrokeColor = GraphValues[i].Color.Color;
                canvas.FillColor = GraphValues[i].Color.Color;
                foreach (var item in GraphValues[i].Path.Points)
                {
                    var x = (item.X * scale.x);
                    var y = (item.Y * scale.y);
                    canvas.DrawCircle(x, y, 3);

                    if (GraphValues[i].Path.Count <= 32)
                        canvas.DrawString((-item.Y).ToString(), x - 10, y - 10, HorizontalAlignment.Left);
                    //canvas.FillCircle(x, y, 5);
                }
            }
            //  YDrawValues(canvas, scale);
        }

        protected void DrawColumn(ICanvas canvas, (float x, float y) scale)
        {
            if (GraphValues is null)
                return;
            if (GraphValues.Length == 0)
                return;
            float singleWidthColumn = scale.x / (GraphValues.Length + 1);

            float location = 0;
            for (int i = 0; i < GraphValues.Length; i++)
            {
                canvas.StrokeColor = GraphValues[i].Color.Color;
                canvas.FillColor = GraphValues[i].Color.Color;
                canvas.FontColor = GraphValues[i].Color.Color;
                canvas.StrokeSize = singleWidthColumn;

                foreach (var item in GraphValues[i].Path.Points)
                {
                    var x = (item.X * scale.x);
                    var y = (item.Y * scale.y);

                    canvas.FillRectangle(x + location, 0, singleWidthColumn, y);

                    if (GraphValues[i].Path.Count <= 32)
                        canvas.DrawString((-item.Y).ToString(), x + location, y - 10, HorizontalAlignment.Left);
                }
                location += singleWidthColumn;
            }


        }

        protected void DrawNet(ICanvas canvas, RectF dirtyRect, (float x, float y) scale)
        {
            float width = scale.x, height = scale.x / 10;
            if (width < 10)
                width = 10;
            if (height < 10)
                height = 10;
            IPattern pattern;
            //Tworzenie paternu 10x10
            using (PictureCanvas pc = new(0, 0, width, height))
            {
                pc.StrokeColor = Colors.Red;
                pc.StrokeSize = 1f;
                //lewo
                pc.DrawLine(0, 0, 0, height);
                //góra
                pc.DrawLine(width, 0, 0, 0);

                pattern = new PicturePattern(pc.Picture, width, height);
            }

            //wypełnienie obiektu danym paternem
            PatternPaint pp = new()
            {
                Pattern = pattern
            };
            canvas.SaveState();
            canvas.SetFillPaint(pp, RectF.Zero);
            canvas.Translate(_fromLeft, dirtyRect.Height - _fromBotton);
            canvas.FillRectangle(0, 0, dirtyRect.Width - _fromLeft, -dirtyRect.Height);
            // canvas.FillRectangle(_fromLeft, 0, dirtyRect.Width - (_fromLeft * 2), dirtyRect.Height - _fromBotton);
            canvas.RestoreState();
        }

        protected void DrawLegend(ICanvas canvas, RectF dirtyRect)
        {
            if (GraphValues is null)
                return;
            if (GraphValues.Length == 0)
                return;

            for (int i = 0; i < GraphValues.Length; i++)
            {

                canvas.FontColor = GraphValues[i].Color.Color;
                canvas.DrawString(GraphValues[i].Name, dirtyRect.Width - 5, _fromBotton + (i * 20), HorizontalAlignment.Right);
            }
        }



        #region Method

        protected void XDrawValues7(ICanvas canvas, RectF dirtyRect, (float, float) scale, bool rotate)
        {
            var point = GraphValues[0].Path.Points.ToArray();
            canvas.SaveState();
            if (rotate)
            {
                for (int i = 0; i < XValues.Length; i++)
                {
                    canvas.SaveState();
                    canvas.Rotate(70, point[i].X * scale.Item1 + 20, dirtyRect.Height - _fromBotton + 30);
                    canvas.DrawString(XValues[i].DayOfWeek.TranslateSelectedDay(), point[i].X * scale.Item1, dirtyRect.Height - _fromBotton + 15, HorizontalAlignment.Left);
                    canvas.RestoreState();
                    canvas.Translate(0, 0);
                }
            }
            else
            {
                for (int i = 0; i < XValues.Length; i++)
                {
                    canvas.DrawString(XValues[i].DayOfWeek.TranslateSelectedDay(), (point[i].X * scale.Item1) + _fromLeft, dirtyRect.Height - _fromBotton + 15, HorizontalAlignment.Left);
                }
            }

            canvas.RestoreState();
        }
        protected void XDrawValues31(ICanvas canvas, RectF dirtyRect, (float, float) scale, bool rotate)
        {
            var point = GraphValues[0].Path.Points.ToArray();
            canvas.SaveState();
            if (rotate)
            {
                for (int i = 0; i < XValues.Length; i++)
                {
                    canvas.SaveState();
                    canvas.Rotate(70, point[i].X * scale.Item1 + 20, dirtyRect.Height - _fromBotton + 30);
                    canvas.DrawString(XValues[i].ToString("dd-MM"), point[i].X * scale.Item1, dirtyRect.Height - _fromBotton + 15, HorizontalAlignment.Left);
                    canvas.RestoreState();
                    canvas.Translate(0, 0);
                }
            }
            else
            {
                for (int i = 0; i < XValues.Length; i++)
                {
                    canvas.DrawString(XValues[i].ToString("dd-MM"), (point[i].X * scale.Item1) + 25, dirtyRect.Height - _fromBotton + 15, HorizontalAlignment.Left);
                }
            }

            canvas.RestoreState();
        }
        protected void XDrawValuesMore(ICanvas canvas, RectF dirtyRect, (float, float) scale, bool rotate)
        {
            var point = GraphValues[0].Path.Points.ToArray();
            canvas.SaveState();
            if (rotate)
            {
                for (int i = 0; i < XValues.Length; i += 5)
                {
                    canvas.SaveState();

                    if (i + 2 >= XValues.Length)
                    {
                        int last = XValues.Length - 1;
                        canvas.Rotate(70, point[last].X * scale.Item1 + 45, dirtyRect.Height - _fromBotton + 10);
                        canvas.DrawString(XValues[last].ToString("dd-MM-yyyy"), point[last].X * scale.Item1, dirtyRect.Height - _fromBotton + 15, HorizontalAlignment.Left);
                        canvas.RestoreState();
                        canvas.Translate(0, 0);
                        break;
                    }

                    canvas.Rotate(70, point[i].X * scale.Item1 + 20, dirtyRect.Height - _fromBotton + 30);
                    canvas.DrawString(XValues[i].ToString("dd-MM-yyyy"), point[i].X * scale.Item1, dirtyRect.Height - _fromBotton + 15, HorizontalAlignment.Left);
                    canvas.RestoreState();
                    canvas.Translate(0, 0);
                }
            }
            else
            {
                for (int i = 0; i < XValues.Length; i += 3)
                {
                    canvas.SaveState();

                    if (i + 2 >= XValues.Length)
                    {
                        int last = XValues.Length - 1;
                        canvas.Rotate(70, point[last].X * scale.Item1 + 45, dirtyRect.Height - _fromBotton + 10);
                        canvas.DrawString(XValues[last].ToString("dd-MM-yyyy"), point[last].X * scale.Item1, dirtyRect.Height - _fromBotton + 15, HorizontalAlignment.Left);
                        canvas.RestoreState();
                        canvas.Translate(0, 0);
                        break;
                    }

                    canvas.Rotate(70, point[i].X * scale.Item1 + 45, dirtyRect.Height - _fromBotton + 10);
                    canvas.DrawString(XValues[i].ToString("dd-MM-yyyy"), point[i].X * scale.Item1, dirtyRect.Height - _fromBotton + 15, HorizontalAlignment.Left);
                    canvas.RestoreState();
                    canvas.Translate(0, 0);
                }
            }

            canvas.RestoreState();
        }

        protected void YDrawValuesMore(ICanvas canvas, RectF dirtyRect, (float, float) scale)
        {
            canvas.SaveState();

            canvas.DrawString((-_max).ToString(), 0, (-_min * scale.Item2) + _fromBotton, HorizontalAlignment.Left);
            canvas.DrawString((-_min).ToString(), 0, (-_max * scale.Item2) + _fromBotton, HorizontalAlignment.Left);
            canvas.RestoreState();

            //var min = -_max;
            //var max = -_min;

            //if (min <= 0)
            //{
            //    if (GraphValues is not null)
            //    {
            //        min = -GraphValues.Max(x => x.Path.Points.Where(x => x.Y < 0).Max(x => x.Y));
            //    }
            //}


            //int numbers = (int)((dirtyRect.Height) / ((max - min) * scale.Item2));

            //canvas.SaveState();
            //canvas.Translate(0, dirtyRect.Height - _fromBotton);

            //for (int i = 0; i < dirtyRect.Height; i += 10)
            //{
            //    canvas.DrawString(GetFirstDigitAndZeros(numbers * i), 0, -(numbers * i), HorizontalAlignment.Left);
            //}

            //for (int i = 1; i < numbers; i++)
            //{
            //    //int firstDigit = (int)(min * i / Math.Pow(10, (int)Math.Floor(Math.Log10(min * i))));
            //    canvas.DrawString(GetFirstDigitAndZeros(min * i), 0, -(min * scale.Item2 * i), HorizontalAlignment.Left);
            //}
            //canvas.RestoreState();
        }

        protected static string GetFirstDigitAndZeros(float value)
        {
            ReadOnlySpan<char> valueChars = value.ToString();
            int zeros = 1;

            if (valueChars.Length == 1)
                zeros = 1;
            else if (valueChars.Length <= 2)
                zeros = 2;
            else if (valueChars.Length > 3 && valueChars.Length < 5)
                zeros = 2;
            else if (valueChars.Length > 5 && valueChars.Length < 7)
                zeros = 3;
            else if (valueChars.Length > 9)
                zeros = 5;


            Span<char> finalChars = stackalloc char[valueChars.Length];

            for (int i = 0; i < zeros; i++)
            {
                finalChars[i] = valueChars[i];
            }

            for (int i = zeros; i < valueChars.Length; i++)
            {
                finalChars[i] = '0';
            }
            return finalChars.ToString();
        }




        #endregion

    }
}
