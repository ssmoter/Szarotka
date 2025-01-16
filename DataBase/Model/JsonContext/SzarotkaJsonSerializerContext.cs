using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.EntitiesServer;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataBase.Model.JsonContext;

[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(RegisterUser))]
[JsonSerializable(typeof(LoginUser))]

[JsonSerializable(typeof(SelectedDayOfWeekRoutes))]
[JsonSerializable(typeof(ResidentialAddress))]
[JsonSerializable(typeof(CustomerRoutes))]
[JsonSerializable(typeof(CustomerRoutes[]))]

[JsonSerializable(typeof(ProductPrice))]
[JsonSerializable(typeof(Day))]
[JsonSerializable(typeof(Driver))]
[JsonSerializable(typeof(Product))]
[JsonSerializable(typeof(ProductName))]
[JsonSerializable(typeof(Cake))]

[JsonSourceGenerationOptions(
   WriteIndented = true,
   DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
   PropertyNameCaseInsensitive = true)]
//[JsonConverter(typeof(CustomDateTimeConverter))]
//[JsonConverter(typeof(CustomBoolConverter))]
public partial class SzarotkaJsonSerializerContext : JsonSerializerContext
{ }
public class CustomBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt32(out int result))
            {
                return result == 1;
            }
        }
        if (reader.TokenType == JsonTokenType.String)
        {
            if (bool.TryParse(reader.GetString(), out var result))
            {
                return result;
            }
        }

        return reader.GetBoolean();
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt32(out int resultInt))
            {
                return new(resultInt);
            }
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var str = reader.GetString();
            if (str == "0")
            {
                return new(int.Parse(str));
            }

            if (DateTime.TryParse(str, out var resultStr))
            {
                return resultStr;
            }
        }
        if (reader.TryGetDateTime(out var result))
        {
            return result;
        }

        return reader.GetDateTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        if (value == DateTime.MinValue)
        {
            writer.WriteNumberValue(0);
        }
        else
        {
            writer.WriteStringValue(value);
        }
    }
}
