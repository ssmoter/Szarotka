using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.Products.ListProduct.AddEdit
{
    public partial class AddEditProductM : ObservableObject
    {
        [ObservableProperty]
        bool updateP;

        [ObservableProperty]
        bool addP;

        [ObservableProperty]
        bool isVisibleFrame;


        [ObservableProperty]
        bool isVisibleBread;

        [ObservableProperty]
        bool isVisibleBuns;

        [ObservableProperty]
        bool isVisibleCake;

        [ObservableProperty]
        bool isVisibleCookies;

        [ObservableProperty]
        bool isVisibleOther;

        public void HideBool(int id)
        {
            if (id != 1)
                IsVisibleBread = false;
            if (id != 2)
                IsVisibleBuns = false;
            if (id != 3)
                IsVisibleCake = false;
            if (id != 4)
                IsVisibleCookies = false;
            if (id != 5)
                IsVisibleOther = false;
        }

        public enum FrameToDisplay
        {
            frame = 0,
            bread,
            buns,
            cake,
            cookies,
            other,
            @default
        }

    }
}
