using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.Products.ListProduct.AddEdit;

public partial class AddEditProductM : ObservableObject
{
    private bool updateP;
    public bool UpdateP
    {
        get => updateP;
        set
        {
            if (SetProperty(ref updateP, value, nameof(UpdateP))) { }
        }
    }

    private bool addP;
    public bool AddP
    {
        get => addP;
        set
        {
            if (SetProperty(ref addP, value, nameof(AddP))) { }
        }
    }

    private bool isVisibleFrame;
    public bool IsVisibleFrame
    {
        get => isVisibleFrame;
        set
        {
            if (SetProperty(ref isVisibleFrame, value, nameof(IsVisibleFrame))) { }
        }
    }


    private bool isVisibleBread;
    public bool IsVisibleBread
    {
        get => isVisibleBread;
        set
        {
            if (SetProperty(ref isVisibleBread, value, nameof(IsVisibleBread))) { }
        }
    }

    private bool isVisibleBuns;
    public bool IsVisibleBuns
    {
        get => isVisibleBuns;
        set
        {
            if (SetProperty(ref isVisibleBuns, value, nameof(IsVisibleBuns))) { }
        }
    }

    private bool isVisibleCake;
    public bool IsVisibleCake
    {
        get => isVisibleCake;
        set
        {
            if (SetProperty(ref isVisibleCake, value, nameof(IsVisibleCake))) { }
        }
    }

    private bool isVisibleCookies;
    public bool IsVisibleCookies
    {
        get => isVisibleCookies;
        set
        {
            if (SetProperty(ref isVisibleCookies, value, nameof(IsVisibleCookies))) { }
        }
    }

    private bool isVisibleOther;
    public bool IsVisibleOther
    {
        get => isVisibleOther;
        set
        {
            if (SetProperty(ref isVisibleOther, value, nameof(IsVisibleOther))) { }
        }
    }

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

public partial class AddEditProductMImg : ObservableObject
{
    private string img;
    public string Img
    {
        get => img;
        set
        {
            if (SetProperty(ref img, value, nameof(Img))) { }
        }
    }
    public AddEditProductMImg(string img)
    {
        Img = img;
    }
    public AddEditProductMImg()
    {

    }
}