namespace Smart.Windows.Markup;

using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

using Smart.Windows.Data;

[MarkupExtensionReturnType(typeof(ContainsToBoolConverter))]
public sealed class ContainsToBoolExtension : MarkupExtension
{
    public bool Invert { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new ContainsToBoolConverter { TrueValue = !Invert, FalseValue = Invert };
}

[MarkupExtensionReturnType(typeof(ContainsToTextConverter))]
public sealed class ContainsToTextExtension : MarkupExtension
{
    public string? True { get; set; }

    public string? False { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new ContainsToTextConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(typeof(ContainsToVisibilityConverter))]
public sealed class ContainsToVisibilityExtension : MarkupExtension
{
    public Visibility True { get; set; }

    public Visibility False { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new ContainsToVisibilityConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(typeof(ContainsToBrushConverter))]
public sealed class ContainsToBrushExtension : MarkupExtension
{
    public Brush True { get; set; } = Brushes.Transparent;

    public Brush False { get; set; } = Brushes.Transparent;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new ContainsToBrushConverter { TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(typeof(ContainsToColorConverter))]
public sealed class ContainsToColorExtension : MarkupExtension
{
    public Color True { get; set; } = Colors.Transparent;

    public Color False { get; set; } = Colors.Transparent;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new ContainsToColorConverter { TrueValue = True, FalseValue = False };
}
