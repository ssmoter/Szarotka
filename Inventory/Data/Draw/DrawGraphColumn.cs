using Inventory.Model;

namespace Inventory.Data.Draw
{
    public class DrawGraphColumn
    {
        public static void DrawColumn(ICanvas canvas, (float x, float y) scale, GraphValues[] GraphValues)
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


    }
}
