using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Shared.CustomControls
{
    public partial class ExpandableCollectionView : ContentView
    {
        private List<object> _internalAllItems = [];
        private readonly CollectionView _collectionView;

        // 1. Czyste BindableProperty - cała logika wykonuje się w OnIsExpandedChanged
        public static readonly BindableProperty IsExpandedProperty =
            BindableProperty.Create(
                nameof(IsExpanded),
                typeof(bool),
                typeof(ExpandableCollectionView),
                false,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnIsExpandedChanged);

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        // 2. Źródło danych
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ExpandableCollectionView), null, propertyChanged: OnItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // 3. Szablon wyglądu
        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ExpandableCollectionView), null, propertyChanged: OnItemTemplateChanged);

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public ObservableCollection<object> VisibleItems { get; } = [];
        public ICommand ToggleExpandCommand { get; }

        public ExpandableCollectionView()
        {
            ToggleExpandCommand = new Command<object>(ToggleExpand);
            _collectionView = new CollectionView();

            _collectionView.SetBinding(
                targetProperty: CollectionView.ItemsSourceProperty,
                binding: Binding.Create<ExpandableCollectionView, ObservableCollection<object>>(
                    static (ExpandableCollectionView staticControl) => staticControl.VisibleItems,
                    source: this
                )
            );

            Content = _collectionView;
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ExpandableCollectionView)bindable).UpdateItemsList(newValue as IEnumerable);
        }

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ExpandableCollectionView)bindable)._collectionView.ItemTemplate = (DataTemplate)newValue;
        }

        // KLUCZOWE MIEJSCE: Zmiana pola IsExpanded ZAWSZE wywołuje tę metodę
        private static void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExpandableCollectionView)bindable;
            control.RefreshVisibleItems((bool)newValue);
        }

        private void UpdateItemsList(IEnumerable items)
        {
            _internalAllItems.Clear();
            if (items == null)
            {
                VisibleItems.Clear();
                return;
            }

            foreach (var item in items) _internalAllItems.Add(item);

            // Przy załadowaniu danych budujemy listę na podstawie AKTUALNEGO stanu IsExpanded
            RefreshVisibleItems(IsExpanded);
        }

        private void ToggleExpand(object clickedItem)
        {
            if (_internalAllItems.Count == 0 || clickedItem != _internalAllItems.First()) return;

            // Jedyne co robi kliknięcie, to zmiana właściwości. 
            // Ponieważ użyliśmy SetValue (wewnątrz właściwości IsExpanded), 
            // MAUI automatycznie powiadomi ViewModel oraz odpali OnIsExpandedChanged.
            IsExpanded = !IsExpanded;
        }

        // Ta metoda odpowiada TYLKO za dopasowanie widoku do stanu bool
        private void RefreshVisibleItems(bool expanded)
        {
            if (_internalAllItems.Count == 0) return;

            if (expanded)
            {
                if (VisibleItems.Count == 0) VisibleItems.Add(_internalAllItems.First());

                for (int i = VisibleItems.Count; i < _internalAllItems.Count; i++)
                {
                    VisibleItems.Add(_internalAllItems[i]);
                }
            }
            else
            {
                if (VisibleItems.Count == 0)
                {
                    VisibleItems.Add(_internalAllItems.First());
                }
                else
                {
                    while (VisibleItems.Count > 1)
                    {
                        VisibleItems.RemoveAt(1);
                    }
                }
            }
        }
    }
}
