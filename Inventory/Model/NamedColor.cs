using System.Reflection;
using System.Text;

namespace Inventory.Model
{
    public class NamedColor
    {
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public Color Color { get; set; }
        // Expose the Color fields as properties
        public float Red => Color.Red;
        public float Green => Color.Green;
        public float Blue => Color.Blue;
        public float Luminosity => Color.GetLuminosity();
        public NamedColor()
        {
            Color = new Color();
        }
        public static NamedColor[] All { get; private set; }


        static NamedColor()
        {
            List<NamedColor> all = new();
            StringBuilder stringBuilder = new();

            // Loop through the public static fields of the Color structure.
            foreach (FieldInfo fieldInfo in typeof(Colors).GetRuntimeFields())
            {
                if (fieldInfo.IsPublic &&
                    fieldInfo.IsStatic &&
                    fieldInfo.FieldType == typeof(Color))
                {
                    // Convert the name to a friendly name.
                    string name = fieldInfo.Name;
                    stringBuilder.Clear();
                    int index = 0;

                    if (name == nameof(Colors.Transparent))
                    {
                        continue;
                    }

                    foreach (char ch in name)
                    {
                        if (index != 0 && Char.IsUpper(ch))
                        {
                            stringBuilder.Append(' ');
                        }
                        stringBuilder.Append(ch);
                        index++;
                    }

                    // Instantiate a NamedColor object.
                    NamedColor namedColor = new()
                    {
                        Name = name,
                        FriendlyName = stringBuilder.ToString(),
                        Color = (Color)fieldInfo.GetValue(null)
                    };

                    // Add it to the collection.
                    all.Add(namedColor);
                }
            }

            all.TrimExcess();
            var a = all.OrderBy(x => x.Luminosity);
            All = a.ToArray();
        }

    }
}
