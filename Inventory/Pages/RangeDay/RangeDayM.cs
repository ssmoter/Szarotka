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

    private CalculationTyp calculationBy = CalculationTyp.None;
    public CalculationTyp CalculationBy
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

    public static CalculationTyp[] GetArray()
    {
        var array = new CalculationTyp[16];
        array[0] = CalculationTyp.None;
        array[1] = CalculationTyp.SumWeek;
        array[2] = CalculationTyp.SumMonth;
        array[3] = CalculationTyp.SumYear;
        array[4] = CalculationTyp.SumDayOfWeek;
        array[5] = CalculationTyp.SumAll;

        array[6] = CalculationTyp.AverageWeek;
        array[7] = CalculationTyp.AverageMonth;
        array[8] = CalculationTyp.AverageYear;
        array[9] = CalculationTyp.AverageDayOfWeek;
        array[10] = CalculationTyp.AverageAll;

        array[11] = CalculationTyp.MedianWeek;
        array[12] = CalculationTyp.MedianMonth;
        array[13] = CalculationTyp.MedianYear;
        array[14] = CalculationTyp.MedianDayOfWeek;
        array[15] = CalculationTyp.MedianAll;
        return array;
    }

    public static string CalculationTranslate(CalculationTyp typ)
    {
        return typ switch
        {
            CalculationTyp.None => "Brak",
            CalculationTyp.SumWeek => "Suma tygodnia",
            CalculationTyp.SumMonth => "Suma miesiąca",
            CalculationTyp.SumYear => "Suma roku",
            CalculationTyp.SumDayOfWeek => "Suma dni tygodnia",
            CalculationTyp.SumAll => "Suma",
            CalculationTyp.AverageWeek => "Średnia tygodnia",
            CalculationTyp.AverageMonth => "Średnia miesiąca",
            CalculationTyp.AverageYear => "Średnia roku",
            CalculationTyp.AverageDayOfWeek => "Średnia dni tygodnia",
            CalculationTyp.AverageAll => "Średnia",
            CalculationTyp.MedianWeek => "Mediana tygodnia",
            CalculationTyp.MedianMonth => "Mediana miesiąca",
            CalculationTyp.MedianYear => "Mediana roku",
            CalculationTyp.MedianDayOfWeek => "Mediana dni tygodnia",
            CalculationTyp.MedianAll => "Mediana",
            _ => "Brak"
        };
    }
    public static TimeSpan TimerDelay => TimeSpan.FromSeconds(2);
}


public enum CalculationTyp
{
    None,
    SumWeek,
    SumMonth,
    SumYear,
    SumDayOfWeek,
    SumAll,

    AverageWeek,
    AverageMonth,
    AverageYear,
    AverageDayOfWeek,
    AverageAll,

    MedianWeek,
    MedianMonth,
    MedianYear,
    MedianDayOfWeek,
    MedianAll,
}

