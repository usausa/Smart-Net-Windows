namespace Smart.Windows.Data;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Data;

[ValueConversion(typeof(object), typeof(string))]
public sealed class FormatConverter : IValueConverter
{
    public bool UseCurrentCulture { get; set; }

    public NumberStyles NumberStyles { get; set; } = NumberStyles.Any;

    public DateTimeStyles DateTimeStyles { get; set; } = DateTimeStyles.None;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return null;
        }

        var format = parameter as string;
        var provider = ResolveCulture(culture);
        if (IsCompositeFormat(format))
        {
            return String.Format(provider, format, value);
        }

        return value is IFormattable formattable ? formattable.ToString(format, provider) : value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string text)
        {
            return value;
        }

        var underlyingType = Nullable.GetUnderlyingType(targetType);
        var type = underlyingType ?? targetType;
        if ((type == typeof(string)) || (type == typeof(object)))
        {
            return text;
        }

        if (String.IsNullOrWhiteSpace(text))
        {
            return underlyingType is not null ? null : DependencyProperty.UnsetValue;
        }

        var format = parameter as string;
        var exactFormat = String.IsNullOrEmpty(format) || IsCompositeFormat(format) ? null : format;
        return Parse(text, type, exactFormat, ResolveCulture(culture)) ?? DependencyProperty.UnsetValue;
    }

    private CultureInfo ResolveCulture(CultureInfo culture) =>
        UseCurrentCulture ? CultureInfo.CurrentCulture : culture;

    private static bool IsCompositeFormat([NotNullWhen(true)] string? format) =>
        (format is not null) && format.Contains('{', StringComparison.Ordinal);

    private object? Parse(string text, Type type, string? format, CultureInfo culture)
    {
        if (type.IsEnum)
        {
            return Enum.TryParse(type, text, true, out var result) ? result : null;
        }

        return Type.GetTypeCode(type) switch
        {
            TypeCode.SByte => ParseNumber<sbyte>(text, NumberStyles, culture),
            TypeCode.Byte => ParseNumber<byte>(text, NumberStyles, culture),
            TypeCode.Int16 => ParseNumber<short>(text, NumberStyles, culture),
            TypeCode.UInt16 => ParseNumber<ushort>(text, NumberStyles, culture),
            TypeCode.Int32 => ParseNumber<int>(text, NumberStyles, culture),
            TypeCode.UInt32 => ParseNumber<uint>(text, NumberStyles, culture),
            TypeCode.Int64 => ParseNumber<long>(text, NumberStyles, culture),
            TypeCode.UInt64 => ParseNumber<ulong>(text, NumberStyles, culture),
            TypeCode.Single => ParseNumber<float>(text, NumberStyles, culture),
            TypeCode.Double => ParseNumber<double>(text, NumberStyles, culture),
            TypeCode.Decimal => ParseNumber<decimal>(text, NumberStyles, culture),
            TypeCode.DateTime => ParseDateTime(text, format, culture),
            _ when type == typeof(DateTimeOffset) => ParseDateTimeOffset(text, format, culture),
            _ when type == typeof(DateOnly) => ParseDateOnly(text, format, culture),
            _ when type == typeof(TimeOnly) => ParseTimeOnly(text, format, culture),
            _ when type == typeof(TimeSpan) => ParseTimeSpan(text, format, culture),
            _ => throw new NotSupportedException($"Target type is not supported. type=[{type}]")
        };
    }

    private static T? ParseNumber<T>(string text, NumberStyles styles, CultureInfo culture)
        where T : struct, INumberBase<T> =>
        T.TryParse(text, styles, culture, out var result) ? result : null;

    private DateTime? ParseDateTime(string text, string? format, CultureInfo culture)
    {
        if ((format is not null) && DateTime.TryParseExact(text, format, culture, DateTimeStyles, out var exact))
        {
            return exact;
        }

        return DateTime.TryParse(text, culture, DateTimeStyles, out var result) ? result : null;
    }

    private DateTimeOffset? ParseDateTimeOffset(string text, string? format, CultureInfo culture)
    {
        if ((format is not null) && DateTimeOffset.TryParseExact(text, format, culture, DateTimeStyles, out var exact))
        {
            return exact;
        }

        return DateTimeOffset.TryParse(text, culture, DateTimeStyles, out var result) ? result : null;
    }

    private DateOnly? ParseDateOnly(string text, string? format, CultureInfo culture)
    {
        if ((format is not null) && DateOnly.TryParseExact(text, format, culture, DateTimeStyles, out var exact))
        {
            return exact;
        }

        return DateOnly.TryParse(text, culture, DateTimeStyles, out var result) ? result : null;
    }

    private TimeOnly? ParseTimeOnly(string text, string? format, CultureInfo culture)
    {
        if ((format is not null) && TimeOnly.TryParseExact(text, format, culture, DateTimeStyles, out var exact))
        {
            return exact;
        }

        return TimeOnly.TryParse(text, culture, DateTimeStyles, out var result) ? result : null;
    }

    private static TimeSpan? ParseTimeSpan(string text, string? format, CultureInfo culture)
    {
        if ((format is not null) && TimeSpan.TryParseExact(text, format, culture, out var exact))
        {
            return exact;
        }

        return TimeSpan.TryParse(text, culture, out var result) ? result : null;
    }
}
