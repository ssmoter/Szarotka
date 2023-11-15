using Inventory.Model;

namespace Inventory.Data.Draw
{
    public class DrawGraphLine
    {
        public static void DrawLine(ICanvas canvas, (float x, float y) scale, GraphValues[] GraphValues)
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


    }
}
