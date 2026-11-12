using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Converters;

public class UnspecifiedDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    value => DateTime.SpecifyKind(value, DateTimeKind.Unspecified),
    value => DateTime.SpecifyKind(value, DateTimeKind.Unspecified));

public class UtcDateTimeOffsetConverter() : ValueConverter<DateTimeOffset, DateTimeOffset>(
    value => value.ToUniversalTime(),
    value => value);
