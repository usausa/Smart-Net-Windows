namespace Smart.Windows;

using Microsoft.CodeAnalysis;

public sealed class GeneratorTest
{
    private const string Source =
        """
        using Smart.Windows;
        using System.Windows;

        namespace Test;

        public partial class TestElement : DependencyObject
        {
            [DependencyProperty]
            public partial string? Text { get; set; }
        }
        """;

    // ------------------------------------------------------------
    // Basic
    // ------------------------------------------------------------

    [Fact]
    public void PropertyGeneratesFieldAndAccessor()
    {
        // Arrange & Act
        var generated = GeneratorTestHelper.GetGeneratedSource(Source);

        // Assert
        Assert.Contains("public static readonly global::System.Windows.DependencyProperty TextProperty = global::System.Windows.DependencyProperty.Register(", generated, StringComparison.Ordinal);
        Assert.Contains("nameof(Text)", generated, StringComparison.Ordinal);
        Assert.Contains("typeof(string)", generated, StringComparison.Ordinal);
        Assert.Contains("typeof(TestElement))", generated, StringComparison.Ordinal);
        Assert.Contains("public partial string? Text", generated, StringComparison.Ordinal);
        Assert.Contains("get => (string?)GetValue(TextProperty);", generated, StringComparison.Ordinal);
        Assert.Contains("set => SetValue(TextProperty, value);", generated, StringComparison.Ordinal);
        Assert.DoesNotContain("PropertyMetadata", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void PropertyProducesNoCompilationError()
    {
        // Arrange & Act
        var diagnostics = GeneratorTestHelper.GetDiagnosticsAll(Source);

        // Assert
        Assert.DoesNotContain(diagnostics, static x => x.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void MultiplePropertiesGenerateInOneClass()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty]
                public partial string? Text { get; set; }

                [DependencyProperty]
                public partial int Number { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("TextProperty", generated, StringComparison.Ordinal);
        Assert.Contains("NumberProperty", generated, StringComparison.Ordinal);
        Assert.Contains("get => (int)GetValue(NumberProperty);", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void ObjectPropertyOmitsCast()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty]
                public partial object? Value { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("get => GetValue(ValueProperty);", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void InternalPropertyKeepsAccessibility()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty]
                internal partial string? Text { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("internal static readonly global::System.Windows.DependencyProperty TextProperty", generated, StringComparison.Ordinal);
        Assert.Contains("internal partial string? Text", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void NestedClassGeneratesContainingTypes()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class Outer
            {
                public partial class TestElement : DependencyObject
                {
                    [DependencyProperty]
                    public partial string? Text { get; set; }
                }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("partial class Outer", generated, StringComparison.Ordinal);
        Assert.Contains("partial class TestElement", generated, StringComparison.Ordinal);
    }

    // ------------------------------------------------------------
    // Default value
    // ------------------------------------------------------------

    [Fact]
    public void DefaultValueIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(DefaultValue = "abc")]
                public partial string? Text { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("new global::System.Windows.PropertyMetadata(\"abc\")", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void DefaultValueIsCastToPropertyType()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(DefaultValue = 1)]
                public partial double Scale { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("new global::System.Windows.PropertyMetadata((double)1)", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void DefaultValueExpressionIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(DefaultValueExpression = "global::Test.TestElement.CreateDefault()")]
                public partial string? Text { get; set; }

                public static string CreateDefault() => "abc";
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("new global::System.Windows.PropertyMetadata(global::Test.TestElement.CreateDefault())", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void OptionsGenerateFrameworkMetadata()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(Options = FrameworkPropertyMetadataOptions.AffectsRender)]
                public partial double Scale { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("new global::System.Windows.FrameworkPropertyMetadata(", generated, StringComparison.Ordinal);
        Assert.Contains("default(double)", generated, StringComparison.Ordinal);
        Assert.Contains("(global::System.Windows.FrameworkPropertyMetadataOptions)(16)", generated, StringComparison.Ordinal);
    }

    // ------------------------------------------------------------
    // Callback
    // ------------------------------------------------------------

    [Fact]
    public void PropertyChangedCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(PropertyChanged = nameof(OnTextChanged))]
                public partial string? Text { get; set; }

                private void OnTextChanged(string? oldValue, string? newValue)
                {
                }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("new global::System.Windows.PropertyMetadata(static (d, e) => ((TestElement)d).OnTextChanged((string?)e.OldValue, (string?)e.NewValue))", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void PropertyChangedNoArgumentCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(PropertyChanged = nameof(OnTextChanged))]
                public partial string? Text { get; set; }

                private void OnTextChanged()
                {
                }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("new global::System.Windows.PropertyMetadata(static (d, e) => ((TestElement)d).OnTextChanged())", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void CoerceCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(Coerce = nameof(CoerceScale))]
                public partial double Scale { get; set; }

                private double CoerceScale(double value) => value;
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("new global::System.Windows.PropertyMetadata(", generated, StringComparison.Ordinal);
        Assert.Contains("default(double)", generated, StringComparison.Ordinal);
        Assert.Contains("null", generated, StringComparison.Ordinal);
        Assert.Contains("static (d, baseValue) => ((TestElement)d).CoerceScale((double)baseValue)", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void StaticCoerceCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(Coerce = nameof(CoerceScale))]
                public partial double Scale { get; set; }

                private static double CoerceScale(double value) => value;
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("static (d, baseValue) => CoerceScale((double)baseValue)", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(Validate = nameof(ValidateScale))]
                public partial double Scale { get; set; }

                private static bool ValidateScale(double value) => true;
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("null,", generated, StringComparison.Ordinal);
        Assert.Contains("static value => ValidateScale((double)value));", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void AllCallbacksProduceNoCompilationError()
    {
        // Arrange
        const string source =
            """
            using Smart.Windows;
            using System.Windows;

            namespace Test;

            public partial class TestElement : DependencyObject
            {
                [DependencyProperty(DefaultValue = 0d, Options = FrameworkPropertyMetadataOptions.AffectsMeasure, PropertyChanged = nameof(OnScaleChanged), Coerce = nameof(CoerceScale), Validate = nameof(ValidateScale))]
                public partial double Scale { get; set; }

                private void OnScaleChanged(double oldValue, double newValue)
                {
                }

                private double CoerceScale(double value) => value;

                private static bool ValidateScale(double value) => true;
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnosticsAll(source);

        // Assert
        Assert.DoesNotContain(diagnostics, static x => x.Severity == DiagnosticSeverity.Error);
    }
}
