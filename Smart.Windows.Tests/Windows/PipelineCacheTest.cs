namespace Smart.Windows;

using SourceGenerateHelper.Testing;

public sealed class PipelineCacheTest
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

    private const string UnrelatedSource =
        """
        namespace Other;

        internal sealed class Unrelated;
        """;

    private const string AddedTargetSource =
        """
        using Smart.Windows;
        using System.Windows;

        namespace Test;

        public partial class AddedElement : DependencyObject
        {
            [DependencyProperty]
            public partial string? Text { get; set; }
        }
        """;

    // ------------------------------------------------------------
    // Cache
    // ------------------------------------------------------------

    [Fact]
    public void UnrelatedEditKeepsModelCached()
    {
        // Arrange & Act
        var result = GeneratorTestHelper.RunIncremental(Source, UnrelatedSource);

        // Assert
        Assert.Equal(result.FirstGeneratedText, result.SecondGeneratedText);
        Assert.NotEmpty(result.OutputReasons);
        Assert.DoesNotContain(result.OutputReasons, static x => x.IsChanged());
    }

    [Fact]
    public void TargetEditRebuildsModel()
    {
        // Arrange & Act
        var result = GeneratorTestHelper.RunIncremental(Source, AddedTargetSource);

        // Assert
        Assert.Contains(result.OutputReasons, static x => x.IsChanged());
    }
}
