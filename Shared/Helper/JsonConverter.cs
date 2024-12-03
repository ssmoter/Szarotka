using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Helper
{
    public static class JsonOptions
    {
        public static JsonSerializerOptions JsonSerializeOptionsBoolAndDateTime => new()
        {
            Converters = { new BoolConverter(), new DateTimeConverter() }
        };
        public static JsonSerializerOptions JsonSerializeOptionsIgnoreCapitalLetters => new()
        {
            PropertyNameCaseInsensitive = true
        };
        public static JsonSerializerOptions JsonSerializeOptionsJsonStringEnumConverter => new()
        {
            Converters = { new JsonStringEnumConverter() }
        };


    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.TryGetDateTime(out DateTime date);
            return date;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {

        }
    }
    public class BoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.String => bool.TryParse(reader.GetString(), out var b) ? b : throw new JsonException($"BoolConverter as string error with value: {reader.GetString()}"),
                JsonTokenType.Number => reader.TryGetInt64(out long l) ? Convert.ToBoolean(l) : reader.TryGetDouble(out double d) ? Convert.ToBoolean(d) : false,
                _ => throw new JsonException($"BoolConverter error with value: {reader.GetString()}"),
            };

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) =>
            writer.WriteBooleanValue(value);
    }
}
