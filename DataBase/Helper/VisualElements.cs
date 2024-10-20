namespace DataBase.Helper
{
    public class VisualElements
    {
        public static void TraverseElements(Element parent, List<Element> elements)
        {
            if (parent is Layout layout)
            {
                foreach (var child in layout.Children)
                {
                    elements.Add(child as Element);
                    TraverseElements(child as Element, elements);
                }
            }
            else if (parent is ContentView contentView)
            {
                var child = contentView.Content;
                if (child != null)
                {
                    elements.Add(child);
                    TraverseElements(child, elements);
                }
            }
            else if (parent is ScrollView scrollView)
            {
                var child = scrollView.Content;
                if (child != null)
                {
                    elements.Add(child);
                    TraverseElements(child, elements);
                }
            }
            else if (parent is Border border)
            {
                var child = border.Content;
                if (child != null)
                {
                    elements.Add(child);
                    TraverseElements(child, elements);
                }
            }
        }
    }
}
