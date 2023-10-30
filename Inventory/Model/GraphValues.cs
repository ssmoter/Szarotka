namespace Inventory.Model
{
    public class GraphValues
    {
        public PathF Path { get; set; }
        public string Name { get; set; }
        public NamedColor Color { get; set; }

        public GraphValues(int i)
        {
            Path ??= new PathF();
            Color ??= new NamedColor();
            Color = NamedColor.All[i];
        }

    }
}
