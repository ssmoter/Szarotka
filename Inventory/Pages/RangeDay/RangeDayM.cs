using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;

namespace Inventory.Pages.RangeDay;

public partial class RangeDayM : ObservableObject
{
    private bool isRefreshing = false;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set
        {
            if (SetProperty(ref isRefreshing, value, nameof(IsRefreshing))) { }
        }
    }
    private bool enableSave;
    public bool EnableSave
    {
        get => enableSave;
        set
        {
            if (SetProperty(ref enableSave, value, nameof(EnableSave))) { }
        }
    }

    private bool listIsVisible = true;
    public bool ListIsVisible
    {
        get => listIsVisible;
        set
        {
            if (SetProperty(ref listIsVisible, value, nameof(ListIsVisible))) { }
        }
    }
    private bool graphIsVisible;
    public bool GraphIsVisible
    {
        get => graphIsVisible;
        set
        {
            if (SetProperty(ref graphIsVisible, value, nameof(GraphIsVisible))) { }
        }
    }
    private bool tableIsVisible;
    public bool TableIsVisible
    {
        get => tableIsVisible;
        set
        {
            if (SetProperty(ref tableIsVisible, value, nameof(TableIsVisible))) { }
        }
    }

    private bool sortTableIsVisible;
    public bool SortTableIsVisible
    {
        get => sortTableIsVisible;
        set
        {
            if (SetProperty(ref sortTableIsVisible, value, nameof(SortTableIsVisible))) { }
        }
    }

}
public partial class DayExpanded : ObservableObject
{
    private Day day;
    public Day Day
    {
        get => day;
        set
        {
            if (SetProperty(ref day, value, nameof(Day))) { }
        }
    }


    private int index;
    public int Index
    {
        get => index;
        set
        {
            if (SetProperty(ref index, value, nameof(Index))) { }
        }
    }

    private string selectedValue = "";
    public string SelectedValue
    {
        get => selectedValue;
        set
        {
            if (SetProperty(ref selectedValue, value, nameof(SelectedValue))) { }
        }
    }
    private IEnumerable<string> selectedHeaders;
    public IEnumerable<string> SelectedHeaders
    {
        get => selectedHeaders;
        set
        {
            if (SetProperty(ref selectedHeaders, value, nameof(SelectedHeaders))) { }
        }
    }
    public DayExpanded(Day day, int index, string selectedValue, IEnumerable<string> selectedHeaders)
    {
        this.Day = day;
        this.Index = index;
        SelectedHeaders = selectedHeaders;
        SelectedValue = selectedValue;
    }
    public DayExpanded()
    {

    }
}

public partial class FilterTyp : ObservableObject
{
    private bool sunday = true;
    public bool Sunday
    {
        get => sunday;
        set
        {
            if (SetProperty(ref sunday, value, nameof(Sunday))) { }
        }
    }
    private bool monday = true;
    public bool Monday
    {
        get => monday;
        set
        {
            if (SetProperty(ref monday, value, nameof(Monday))) { }
        }
    }

    private bool tuesday = true;
    public bool Tuesday
    {
        get => tuesday;
        set
        {
            if (SetProperty(ref tuesday, value, nameof(Tuesday))) { }
        }
    }
    private bool wednesday = true;
    public bool Wednesday
    {
        get => wednesday;
        set
        {
            if (SetProperty(ref wednesday, value, nameof(Wednesday))) { }
        }
    }
    private bool thursday = true;
    public bool Thursday
    {
        get => thursday;
        set
        {
            if (SetProperty(ref thursday, value, nameof(Thursday))) { }
        }
    }

    private bool friday = true;
    public bool Friday
    {
        get => friday;
        set
        {
            if (SetProperty(ref friday, value, nameof(Friday))) { }
        }
    }
    private bool saturday = true;
    public bool Saturday
    {
        get => saturday;
        set
        {
            if (SetProperty(ref saturday, value, nameof(Saturday))) { }
        }
    }

    public DayOfWeek[] GetSelectedDays()
    {
        var selectedDays = new List<DayOfWeek>();
        if (Sunday) selectedDays.Add(DayOfWeek.Sunday);
        if (Monday) selectedDays.Add(DayOfWeek.Monday);
        if (Tuesday) selectedDays.Add(DayOfWeek.Tuesday);
        if (Wednesday) selectedDays.Add(DayOfWeek.Wednesday);
        if (Thursday) selectedDays.Add(DayOfWeek.Thursday);
        if (Friday) selectedDays.Add(DayOfWeek.Friday);
        if (Saturday) selectedDays.Add(DayOfWeek.Saturday);

        return [.. selectedDays];
    }


    private string selectedName = "";
    public string SelectedName
    {
        get => selectedName;
        set
        {
            if (SetProperty(ref selectedName, value, nameof(SelectedName))) { }
        }
    }
    private string selectedKey = "";
    public string SelectedKey
    {
        get => selectedKey;
        set
        {
            if (SetProperty(ref selectedKey, value, nameof(SelectedKey))) { }
        }
    }
    private OrderTyp orderBy = OrderTyp.Asc;
    public OrderTyp OrderBy
    {
        get => orderBy;
        set
        {
            if (SetProperty(ref orderBy, value, nameof(OrderBy))) { }
        }
    }

    private string sortedName = "";
    public string SortedName
    {
        get => sortedName;
        set
        {
            if (SetProperty(ref sortedName, value, nameof(SortedName))) { }
        }
    }

    private string sortedKey = "*";
    public string SortedKey
    {
        get => sortedKey;
        set
        {
            if (SetProperty(ref sortedKey, value, nameof(SortedKey))) { }
        }
    }

    private DayCalculationType calculationBy = DayCalculationType.None;
    public DayCalculationType CalculationBy
    {
        get => calculationBy;
        set
        {
            if (SetProperty(ref calculationBy, value, nameof(CalculationBy))) { }
        }
    }

    public enum OrderTyp
    {
        /// <summary>
        /// Brak sortowania
        /// </summary>
        None,
        /// <summary>
        /// Malejący
        /// </summary>
        Desc,
        /// <summary>
        /// Rosnący
        /// </summary>
        Asc,
    }

    public static DayCalculationType[] GetArray()
    {
        var array = new DayCalculationType[16];
        array[0] = DayCalculationType.None;
        array[1] = DayCalculationType.SumWeek;
        array[2] = DayCalculationType.SumMonth;
        array[3] = DayCalculationType.SumYear;
        array[4] = DayCalculationType.SumDayOfWeek;
        array[5] = DayCalculationType.SumAll;

        array[6] = DayCalculationType.AverageWeek;
        array[7] = DayCalculationType.AverageMonth;
        array[8] = DayCalculationType.AverageYear;
        array[9] = DayCalculationType.AverageDayOfWeek;
        array[10] = DayCalculationType.AverageAll;

        array[11] = DayCalculationType.MedianWeek;
        array[12] = DayCalculationType.MedianMonth;
        array[13] = DayCalculationType.MedianYear;
        array[14] = DayCalculationType.MedianDayOfWeek;
        array[15] = DayCalculationType.MedianAll;
        return array;
    }

    public static string CalculationTranslate(DayCalculationType typ)
    {
        return typ switch
        {
            DayCalculationType.None => "Brak",
            DayCalculationType.SumWeek => "Suma tygodnia",
            DayCalculationType.SumMonth => "Suma miesiąca",
            DayCalculationType.SumYear => "Suma roku",
            DayCalculationType.SumDayOfWeek => "Suma dni tygodnia",
            DayCalculationType.SumAll => "Suma",
            DayCalculationType.AverageWeek => "Średnia tygodnia",
            DayCalculationType.AverageMonth => "Średnia miesiąca",
            DayCalculationType.AverageYear => "Średnia roku",
            DayCalculationType.AverageDayOfWeek => "Średnia dni tygodnia",
            DayCalculationType.AverageAll => "Średnia",
            DayCalculationType.MedianWeek => "Mediana tygodnia",
            DayCalculationType.MedianMonth => "Mediana miesiąca",
            DayCalculationType.MedianYear => "Mediana roku",
            DayCalculationType.MedianDayOfWeek => "Mediana dni tygodnia",
            DayCalculationType.MedianAll => "Mediana",
            _ => "Brak"
        };
    }
    public static TimeSpan TimerDelay => TimeSpan.FromSeconds(2);
}




