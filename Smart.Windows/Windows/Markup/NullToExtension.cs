namespace Smart.Windows.Markup;

using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

using Smart.Windows.Data;

[MarkupExtensionReturnType(typeof(NullToBoolConverter))]
public sealed class NullToBoolExtension : MarkupExtension
{
    public bool Invert { get; set; }

    public bool HandleEmptyString { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new NullToBoolConverter { NullValue = !Invert, NonNullValue = Invert, HandleEmptyString = HandleEmptyString };
}

[MarkupExtensionReturnType(typeof(NullToTextConverter))]
public sealed class NullToTextExtension : MarkupExtension
{
    public string? Null { get; set; }

    public string? NonNull { get; set; }

    public bool HandleEmptyString { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new NullToTextConverter { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };
}

[MarkupExtensionReturnType(typeof(NullToVisibilityConverter))]
public sealed class NullToVisibilityExtension : MarkupExtension
{
    public Visibility Null { get; set; }

    public Visibility NonNull { get; set; }

    public bool HandleEmptyString { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new NullToVisibilityConverter { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };
}

[MarkupExtensionReturnType(typeof(NullToBrushConverter))]
public sealed class NullToBrushExtension : MarkupExtension
{
    public Brush Null { get; set; } = Brushes.Transparent;

    public Brush NonNull { get; set; } = Brushes.Transparent;

    public bool HandleEmptyString { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new NullToBrushConverter { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };
}

[MarkupExtensionReturnType(typeof(NullToColorConverter))]
public sealed class NullToColorExtension : MarkupExtension
{
    public Color Null { get; set; } = Colors.Transparent;

    public Color NonNull { get; set; } = Colors.Transparent;

    public bool HandleEmptyString { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new NullToColorConverter { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };
}
