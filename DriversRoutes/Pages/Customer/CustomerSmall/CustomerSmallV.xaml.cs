using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

using DataBase.Model.EntitiesRoutes;

using System.Windows.Input;

namespace DriversRoutes.Pages.Customer.CustomerSmall;

public partial class CustomerSmallV : ContentView
{

    #region Variables

    public static readonly BindableProperty QueueNumberProperty
    = BindableProperty.Create(nameof(QueueNumber), typeof(int?), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public int? QueueNumber
    {
        get => (int?)GetValue(QueueNumberProperty);
        set => SetValue(QueueNumberProperty, value);
    }

    public static readonly BindableProperty NameProperty
    = BindableProperty.Create(nameof(Name), typeof(string), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public static readonly BindableProperty DescriptionProperty
    = BindableProperty.Create(nameof(Description), typeof(string), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
        var control = (CustomerSmallV)bindable;
        var value = newValue as string;

        control.CustomerSmallM.Description = !string.IsNullOrWhiteSpace(value);
    });
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly BindableProperty PhoneNumberProperty
    = BindableProperty.Create(nameof(PhoneNumber), typeof(string), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
        var control = (CustomerSmallV)bindable;
        var value = newValue as string;

        control.CustomerSmallM.PhoneNumber = !string.IsNullOrWhiteSpace(value);
    });
    public string PhoneNumber
    {
        get => (string)GetValue(PhoneNumberProperty);
        set => SetValue(PhoneNumberProperty, value);
    }
    public static readonly BindableProperty CreatedDateProperty
    = BindableProperty.Create(nameof(CreatedDate), typeof(DateTime?), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public DateTime? CreatedDate
    {
        get => (DateTime?)GetValue(CreatedDateProperty);
        set => SetValue(CreatedDateProperty, value);
    }

    public static readonly BindableProperty LongitudeProperty
    = BindableProperty.Create(nameof(Longitude), typeof(double?), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
        var control = (CustomerSmallV)bindable;
        if (newValue is double value)
        {
            control.CustomerSmallM.Coordinates = value != 0;
        }

    });
    public double? Longitude
    {
        get => (double?)GetValue(LongitudeProperty);
        set => SetValue(LongitudeProperty, value);
    }

    public static readonly BindableProperty LatitudeProperty
    = BindableProperty.Create(nameof(Latitude), typeof(double?), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
        var control = (CustomerSmallV)bindable;

        if (newValue is double value)
        {
            control.CustomerSmallM.Coordinates = value != 0;
        }
    });
    public double? Latitude
    {
        get => (double?)GetValue(LatitudeProperty);
        set => SetValue(LatitudeProperty, value);
    }

    public static readonly BindableProperty DayOfWeekProperty
    = BindableProperty.Create(nameof(DayOfWeek), typeof(SelectedDayOfWeekRoutes), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public SelectedDayOfWeekRoutes DayOfWeek
    {
        get => (SelectedDayOfWeekRoutes)GetValue(DayOfWeekProperty);
        set => SetValue(DayOfWeekProperty, value);
    }

    public static readonly BindableProperty ResidentialAddressProperty
    = BindableProperty.Create(nameof(ResidentialAddress), typeof(ResidentialAddress), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public ResidentialAddress ResidentialAddress
    {
        get => (ResidentialAddress)GetValue(ResidentialAddressProperty);
        set => SetValue(ResidentialAddressProperty, value);
    }




    public static readonly BindableProperty DeleteIsVisibleProperty
    = BindableProperty.Create(nameof(DeleteIsVisible), typeof(bool), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public bool DeleteIsVisible
    {
        get => (bool)GetValue(DeleteIsVisibleProperty);
        set => SetValue(DeleteIsVisibleProperty, value);
    }

    public static readonly BindableProperty EditIsVisibleProperty
    = BindableProperty.Create(nameof(EditIsVisible), typeof(bool), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public bool EditIsVisible
    {
        get => (bool)GetValue(EditIsVisibleProperty);
        set => SetValue(EditIsVisibleProperty, value);
    }

    public static readonly BindableProperty DisplayIsVisibleProperty
    = BindableProperty.Create(nameof(DisplayIsVisible), typeof(bool), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public bool DisplayIsVisible
    {
        get => (bool)GetValue(DisplayIsVisibleProperty);
        set => SetValue(DisplayIsVisibleProperty, value);
    }

    public static readonly BindableProperty LocationIsVisibleProperty
    = BindableProperty.Create(nameof(LocationIsVisible), typeof(bool), typeof(CustomerSmallV), propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public bool LocationIsVisible
    {
        get => (bool)GetValue(LocationIsVisibleProperty);
        set => SetValue(LocationIsVisibleProperty, value);
    }

    #endregion

    #region Buttons

    public static readonly BindableProperty DeleteButtonCommandProperty =
           BindableProperty.Create(nameof(DeleteButtonCommand), typeof(ICommand), typeof(CustomerSmallV));

    public ICommand DeleteButtonCommand
    {
        get => (ICommand)GetValue(DeleteButtonCommandProperty);
        set => SetValue(DeleteButtonCommandProperty, value);
    }

    public static readonly BindableProperty EditButtonCommandProperty =
       BindableProperty.Create(nameof(EditButtonCommand), typeof(ICommand), typeof(CustomerSmallV));

    public ICommand EditButtonCommand
    {
        get => (ICommand)GetValue(EditButtonCommandProperty);
        set => SetValue(EditButtonCommandProperty, value);
    }

    public static readonly BindableProperty DisplayButtonCommandProperty =
       BindableProperty.Create(nameof(DisplayButtonCommand), typeof(ICommand), typeof(CustomerSmallV));

    public ICommand DisplayButtonCommand
    {
        get => (ICommand)GetValue(DisplayButtonCommandProperty);
        set => SetValue(DisplayButtonCommandProperty, value);
    }
    public static readonly BindableProperty LocationButtonCommandProperty =
   BindableProperty.Create(nameof(LocationButtonCommand), typeof(ICommand), typeof(CustomerSmallV));

    public ICommand LocationButtonCommand
    {
        get => (ICommand)GetValue(LocationButtonCommandProperty);
        set => SetValue(LocationButtonCommandProperty, value);
    }





    public static readonly BindableProperty DeleteButtonCommandParameterProperty =
       BindableProperty.Create(nameof(DeleteButtonCommandParameter), typeof(object), typeof(CustomerSmallV));

    public object DeleteButtonCommandParameter
    {
        get => GetValue(DeleteButtonCommandParameterProperty);
        set => SetValue(DeleteButtonCommandParameterProperty, value);
    }

    public static readonly BindableProperty EditButtonCommandParameterProperty =
       BindableProperty.Create(nameof(EditButtonCommandParameter), typeof(object), typeof(CustomerSmallV));

    public object EditButtonCommandParameter
    {
        get => GetValue(EditButtonCommandParameterProperty);
        set => SetValue(EditButtonCommandParameterProperty, value);
    }

    public static readonly BindableProperty DisplayButtonCommandParameterProperty =
       BindableProperty.Create(nameof(DisplayButtonCommandParameter), typeof(object), typeof(CustomerSmallV));

    public object DisplayButtonCommandParameter
    {
        get => GetValue(DisplayButtonCommandParameterProperty);
        set => SetValue(DisplayButtonCommandParameterProperty, value);
    }

    public static readonly BindableProperty LocationButtonCommandParameterProperty =
   BindableProperty.Create(nameof(LocationButtonCommandParameter), typeof(object), typeof(CustomerSmallV));

    public object LocationButtonCommandParameter
    {
        get => GetValue(LocationButtonCommandParameterProperty);
        set => SetValue(LocationButtonCommandParameterProperty, value);
    }

    #endregion

    private CustomerSmallM customerSmallM;
    public CustomerSmallM CustomerSmallM
    {
        get => customerSmallM;
        set
        {
            customerSmallM = value;
            OnPropertyChanged(nameof(CustomerSmallM));
        }
    }
    public CustomerSmallV()
    {
        InitializeComponent();
        CustomerSmallM = new CustomerSmallM();
    }


    private async void TapGestureRecognizer_Tapped_CopyLocation(object sender, TappedEventArgs e)
    {
        await CopyLocation();
    }


    private async Task CopyLocation()
    {
        string location = $"{Latitude.ToString().Replace(',', '.')} , {Longitude.ToString().Replace(',', '.')}";
        await Clipboard.SetTextAsync(location);

        var toast = Toast.Make("Skopiowano współrzędne geograficzne", ToastDuration.Short);
        await toast.Show();
    }

    private void Button_Clicked_Call(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(PhoneNumber))
            if (PhoneDialer.Default.IsSupported)
                PhoneDialer.Default.Open(PhoneNumber);
    }
}
