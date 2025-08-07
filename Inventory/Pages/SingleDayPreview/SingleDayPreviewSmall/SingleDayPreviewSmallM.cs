using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;

using System.Collections.ObjectModel;

namespace Inventory.Pages.SingleDayPreview.SingleDayPreviewSmall
{
    public partial class SingleDayPreviewSmallM : ObservableObject
    {
        private bool price;
        public bool Price
        {
            get => price;
            set
            {
                if (SetProperty(ref price, value, nameof(Price)))
                {
                }
            }
        }

        private bool product;
        public bool Product
        {
            get => product;
            set
            {
                if (SetProperty(ref product, value, nameof(Product)))
                {
                }
            }
        }

        private bool cake;
        public bool Cake
        {
            get => cake;
            set
            {
                if (SetProperty(ref cake, value, nameof(Cake)))
                {
                }
            }
        }

        private ObservableCollection<Product> products = [];
        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                if (SetProperty(ref products, value, nameof(Products)))
                {
                }
            }
        }

        private ObservableCollection<CakeIsExpanded> cakes = [];
        public ObservableCollection<CakeIsExpanded> Cakes
        {
            get => cakes;
            set
            {
                if (SetProperty(ref cakes, value, nameof(Cakes)))
                {
                }
            }
        }


        private View parent;
        public View Parent
        {
            get => parent;
            set
            {
                if (SetProperty(ref parent, value, nameof(Parent)))
                {
                }
                if (parent is not null)
                {
                    var bounds = parent.Bounds;
                    MaxHeight = bounds.Height;
                }
            }
        }
        private double maxHeight;
        public double MaxHeight
        {
            get => maxHeight;
            set
            {
                if (SetProperty(ref maxHeight, value, nameof(MaxHeight)))
                {
                }
            }
        }


        private int countSellCakes;
        public int CountSellCakes
        {
            get => countSellCakes;
            set
            {
                if (SetProperty(ref countSellCakes, value, nameof(CountSellCakes)))
                {
                }
            }
        }

        private int countReturnCakes;
        public int CountReturnCakes
        {
            get => countReturnCakes;
            set
            {
                if (SetProperty(ref countReturnCakes, value, nameof(CountReturnCakes)))
                {
                }
            }
        }
        private bool cakesIsRefreshing;
        public bool CakesIsRefreshing
        {
            get => cakesIsRefreshing;
            set
            {
                if (SetProperty(ref cakesIsRefreshing, value, nameof(CakesIsRefreshing)))
                {
                }
            }
        }

    }

    public partial class CakeIsExpanded : Cake
    {
        public CakeIsExpanded()
        { }
        public CakeIsExpanded(Cake cake) : base(cake)
        {
            IsExpanded = false;
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (SetProperty(ref isExpanded, value, nameof(IsExpanded)))
                {
                }
            }
        }

    }

}
