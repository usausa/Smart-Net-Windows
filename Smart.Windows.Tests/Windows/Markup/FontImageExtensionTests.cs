namespace Smart.Windows.Markup;

using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

using Smart.Windows.Interactivity;

public sealed class FontImageExtensionTests
{
    private const string Menu = "\uE700";

    private const string Search = "\uE721";

    private static DrawingImage Provide(FontImageExtension extension) =>
        Assert.IsType<DrawingImage>(extension.ProvideValue(NullServiceProvider.Instance));

    private static GeometryDrawing GlyphDrawing(DrawingImage image) =>
        Assert.IsType<GeometryDrawing>(Assert.IsType<DrawingGroup>(image.Drawing).Children[1]);

    [Fact]
    public void HasSymbolFontByDefault()
    {
        // Arrange
        var extension = new FontImageExtension();

        // Act & Assert
        Assert.Equal("Segoe Fluent Icons, Segoe MDL2 Assets", extension.FontFamily.Source);
        Assert.Equal(16d, extension.Size);
        Assert.Same(Brushes.Black, extension.Brush);
    }

    [Fact]
    public void ProvidesFrozenImageSizedToCharacterBox()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Act
            var image = Provide(new FontImageExtension { Glyph = Menu });

            // Assert
            Assert.True(image.IsFrozen);
            Assert.Equal(16d, image.Width);
            Assert.Equal(16d, image.Height);
        });
    }

    [Fact]
    public void ProvidesSameSizeForDifferentGlyphs()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Act
            var menu = Provide(new FontImageExtension { Glyph = Menu });
            var search = Provide(new FontImageExtension { Glyph = Search });

            // Assert
            Assert.Equal(menu.Width, search.Width);
            Assert.Equal(menu.Height, search.Height);
        });
    }

    [Fact]
    public void UsesSize()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Act
            var image = Provide(new FontImageExtension { Glyph = Menu, Size = 32 });

            // Assert
            Assert.Equal(32d, image.Width);
            Assert.Equal(32d, image.Height);
        });
    }

    [Fact]
    public void DrawsGlyphWithBrush()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Act
            var drawing = GlyphDrawing(Provide(new FontImageExtension { Glyph = Menu, Brush = Brushes.Red }));

            // Assert
            Assert.Same(Brushes.Red, drawing.Brush);
            Assert.False(drawing.Geometry.Bounds.IsEmpty);
        });
    }

    [Fact]
    public void DoesNotFreezeGivenBrush()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var brush = new SolidColorBrush(Colors.Red);

            // Act
            var image = Provide(new FontImageExtension { Glyph = Menu, Brush = brush });

            // Assert
            Assert.False(brush.IsFrozen);
            Assert.True(image.IsFrozen);
            Assert.Equal(Colors.Red, Assert.IsType<SolidColorBrush>(GlyphDrawing(image).Brush).Color);
        });
    }

    [Fact]
    public void CanBeUsedInXaml()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            const string xaml = """
                <Image xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                       xmlns:s="clr-namespace:Smart.Windows.Markup;assembly=Smart.Windows"
                       Source="{s:FontImage Glyph=&#xE700;, Size=24, Brush=Red}" />
                """;

            // Act
            var image = Assert.IsType<Image>(XamlReader.Parse(xaml));

            // Assert
            var source = Assert.IsType<DrawingImage>(image.Source);
            Assert.Equal(24d, source.Width);
            Assert.Equal(Colors.Red, Assert.IsType<SolidColorBrush>(GlyphDrawing(source).Brush).Color);
        });
    }
}
