namespace Smart.Windows.Markup;

using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

using Smart.Windows.Data;
using Smart.Windows.Expressions;

[MarkupExtensionReturnType(typeof(CompareToBoolConverter))]
public sealed class CompareToBoolExtension : MarkupExtension
{
    public ICompareExpression? Expression { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new CompareToBoolConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = true, FalseValue = false };
}

[MarkupExtensionReturnType(typeof(CompareToTextConverter))]
public sealed class CompareToTextExtension : MarkupExtension
{
    public ICompareExpression? Expression { get; set; }

    public string? True { get; set; }

    public string? False { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new CompareToTextConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(typeof(CompareToVisibilityConverter))]
public sealed class CompareToVisibilityExtension : MarkupExtension
{
    public ICompareExpression? Expression { get; set; }

    public Visibility True { get; set; }

    public Visibility False { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new CompareToVisibilityConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(typeof(CompareToBrushConverter))]
public sealed class CompareToBrushExtension : MarkupExtension
{
    public ICompareExpression? Expression { get; set; }

    public Brush True { get; set; } = Brushes.Transparent;

    public Brush False { get; set; } = Brushes.Transparent;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new CompareToBrushConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };
}

[MarkupExtensionReturnType(typeof(CompareToColorConverter))]
public sealed class CompareToColorExtension : MarkupExtension
{
    public ICompareExpression? Expression { get; set; }

    public Color True { get; set; } = Colors.Transparent;

    public Color False { get; set; } = Colors.Transparent;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new CompareToColorConverter { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };
}
