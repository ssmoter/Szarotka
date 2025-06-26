namespace DataBase.Model.EntitiesRoutes;
public partial class ResidentialAddress : BaseEntities<Guid>, IDisposable
{
    private Guid customerId;
    public Guid CustomerId
    {
        get => customerId;
        set
        {
            if (SetProperty(ref customerId, value, nameof(CustomerId))) { }
        }
    }

    private string name = "";
    public string Name
    {
        get => name;
        set
        {
            if (SetProperty(ref name, value, nameof(Name))) { }
        }
    }
    private string surname = "";
    public string Surname
    {
        get => surname;
        set
        {
            if (!SetProperty(ref surname, value, nameof(Surname))) { }
        }
    }
    private string street = "";
    public string Street
    {
        get => street;
        set
        {
            if (SetProperty(ref street, value, nameof(Street))) { }
        }
    }
    private string houseNumber = "";
    public string HouseNumber
    {
        get => houseNumber;
        set
        {
            if (SetProperty(ref houseNumber, value, nameof(HouseNumber))) { }
        }
    }
    private string apartmentNumber = "";
    public string ApartmentNumber
    {
        get => apartmentNumber;
        set
        {
            if (SetProperty(ref apartmentNumber, value, nameof(ApartmentNumber))) { }
        }
    }
    private string postalCode = "";
    public string PostalCode
    {
        get => postalCode;
        set
        {
            if (SetProperty(ref postalCode, value, nameof(PostalCode))) { }
        }
    }

    private string city = "";
    public string City
    {
        get => city;
        set
        {
            if (SetProperty(ref city, value, nameof(City))) { }
        }
    }

    private string country = "";
    public string Country
    {
        get => country;
        set
        {
            if (SetProperty(ref country, value, nameof(Country))) { }
        }
    }
    public ResidentialAddress()
    { }

    public ResidentialAddress(ResidentialAddress copy)
    {
        this.CustomerId = copy.CustomerId;
        this.Name = copy.Name;
        this.Street = copy.Street;
        this.HouseNumber = copy.HouseNumber;
        this.ApartmentNumber = copy.ApartmentNumber;
        this.PostalCode = copy.PostalCode;
        this.City = copy.City;
        this.Country = copy.Country;
        this.Updated = copy.Updated;
        this.Surname = copy.Surname;
        this.Id = copy.Id;
        this.Created = copy.Created;
    }

    public override string ToString()
    {
        var _name = $"{Name} {Surname}";
        var _street = $"{Street} {HouseNumber} {(string.IsNullOrWhiteSpace(ApartmentNumber) ? "" : "/")} {ApartmentNumber}";
        var _city = $"{PostalCode} {City}";

        string to = $"{_name}{(!string.IsNullOrWhiteSpace(_name) ? Environment.NewLine : "")}{_street}{(!string.IsNullOrWhiteSpace(_street) ? Environment.NewLine : "")}{_city}{(!string.IsNullOrWhiteSpace(_city) ? Environment.NewLine : "")}{Country}";

        return to;
    }


    public void Dispose()
    {
        Name = "";
        Surname = "";
        Street = "";
        HouseNumber = "";
        ApartmentNumber = "";
        PostalCode = "";
        City = "";
        Country = "";
    }
}
