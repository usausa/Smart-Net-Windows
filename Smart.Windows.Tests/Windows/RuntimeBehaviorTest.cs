namespace Smart.Windows;

using System.Windows;
using System.Windows.Media;

public sealed class RuntimeBehaviorTest
{
    private static RuntimeElement CreateElement() => new();

    // ------------------------------------------------------------
    // Property
    // ------------------------------------------------------------

    [Fact]
    public void PropertyIsRegistered()
    {
        // Arrange & Act
        var property = RuntimeElement.ScaleProperty;

        // Assert
        Assert.Equal(nameof(RuntimeElement.Scale), property.Name);
        Assert.Equal(typeof(double), property.PropertyType);
        Assert.Equal(typeof(RuntimeElement), property.OwnerType);
    }

    [Fact]
    public void ValueRoundTrips()
    {
        // Arrange
        var element = CreateElement();

        // Act
        element.Scale = 5d;

        // Assert
        Assert.Equal(5d, element.Scale);
        Assert.Equal(5d, element.GetValue(RuntimeElement.ScaleProperty));
    }

    // ------------------------------------------------------------
    // Default value
    // ------------------------------------------------------------

    [Fact]
    public void DefaultValueIsApplied()
    {
        // Arrange & Act
        var element = CreateElement();

        // Assert
        Assert.Equal(1d, element.Scale);
    }

    [Fact]
    public void DefaultValueExpressionIsApplied()
    {
        // Arrange & Act
        var element = CreateElement();

        // Assert
        Assert.Same(Brushes.SteelBlue, element.BarBrush);
    }

    // ------------------------------------------------------------
    // Callback
    // ------------------------------------------------------------

    [Fact]
    public void PropertyChangedCallbackIsInvoked()
    {
        // Arrange
        var element = CreateElement();

        // Act
        element.Scale = 3d;

        // Assert
        Assert.Equal(1, element.ChangedCount);
        Assert.Equal(1d, element.OldValue);
        Assert.Equal(3d, element.NewValue);
    }

    [Fact]
    public void CoerceCallbackIsApplied()
    {
        // Arrange
        var element = CreateElement();

        // Act
        element.Scale = 100d;

        // Assert
        Assert.Equal(10d, element.Scale);
        Assert.Equal(10d, element.NewValue);
    }

    [Fact]
    public void ValidateCallbackRejectsInvalidValue()
    {
        // Arrange
        var element = CreateElement();

        // Act & Assert
        element.Label = "abc";
        Assert.Equal("abc", element.Label);
        Assert.Throws<ArgumentException>(() => element.Label = "too long value");
    }
}

internal sealed partial class RuntimeElement : DependencyObject
{
    [DependencyProperty(DefaultValue = 1d, PropertyChanged = nameof(OnScaleChanged), Coerce = nameof(CoerceScale))]
    public partial double Scale { get; set; }

    [DependencyProperty(DefaultValueExpression = "global::System.Windows.Media.Brushes.SteelBlue")]
    public partial Brush? BarBrush { get; set; }

    [DependencyProperty(Validate = nameof(ValidateLabel))]
    public partial string? Label { get; set; }

    public double MaximumScale { get; set; } = 10d;

    public int ChangedCount { get; private set; }

    public double OldValue { get; private set; }

    public double NewValue { get; private set; }

    private void OnScaleChanged(double oldValue, double newValue)
    {
        ChangedCount++;
        OldValue = oldValue;
        NewValue = newValue;
    }

    private double CoerceScale(double value) => Math.Clamp(value, 0d, MaximumScale);

    private static bool ValidateLabel(string? value) => value is null || (value.Length <= 5);
}
