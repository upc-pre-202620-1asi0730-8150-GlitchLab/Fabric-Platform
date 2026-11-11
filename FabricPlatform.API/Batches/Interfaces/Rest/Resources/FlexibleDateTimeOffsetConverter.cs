using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public sealed class FlexibleDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    private static readonly CultureInfo[] AcceptedCultures =
    {
        CultureInfo.GetCultureInfo("es-PE"),
        CultureInfo.GetCultureInfo("es-ES"),
        CultureInfo.GetCultureInfo("en-US"),
        CultureInfo.InvariantCulture
    };

    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("A date and time must be provided as a string.");
        }

        var value = reader.GetString()?.Replace("·", ",", StringComparison.Ordinal).Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        foreach (var culture in AcceptedCultures)
        {
            if (DateTimeOffset.TryParse(
                    value,
                    culture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out var dateTime))
            {
                return dateTime;
            }
        }

        throw new JsonException("The date and time format is not supported.");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value);
            return;
        }

        writer.WriteNullValue();
    }
}
