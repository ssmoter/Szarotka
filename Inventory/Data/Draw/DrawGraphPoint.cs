using Inventory.Model;

namespace Inventory.Data.Draw
{
    public class DrawGraphPoint
    {

        public static void DrawPoint(ICanvas canvas, (float x, float y) scale, GraphValues[] GraphValues)
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

    }
}
