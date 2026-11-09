namespace Smart.Windows;

using System.Windows;
using System.Windows.Controls;

using Smart.Windows.Interactivity;

public sealed class DependencyObjectExtensionsTests
{
    private static (Grid Root, StackPanel Panel, Border Border, TextBlock Text) Build()
    {
        var text = new TextBlock();
        var border = new Border { Tag = "border", Child = text };
        var panel = new StackPanel { Tag = "panel", Children = { border } };
        var root = new Grid { Tag = "root", Children = { panel } };
        return (root, panel, border, text);
    }

    private static bool HasTag(FrameworkElement element, string tag) => Equals(element.Tag, tag);

    //------------------------------------------------------------
    // Parent
    //------------------------------------------------------------

    [Fact]
    public void FindParentWithPredicateSkipsNotMatching()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (root, _, _, text) = Build();

            // Act & Assert
            Assert.Same(root, text.FindParent<Panel>(static x => HasTag(x, "root")));
            Assert.Null(text.FindParent<Panel>(static _ => false));
        });
    }

    [Fact]
    public void FindParentWithStatePassesState()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (root, _, _, text) = Build();

            // Act & Assert
            Assert.Same(root, text.FindParent<Panel, string>("root", static (x, s) => HasTag(x, s)));
        });
    }

    [Fact]
    public void FindParentOrSelfReturnsSelfFirst()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (_, _, border, text) = Build();

            // Act & Assert
            Assert.Same(border, border.FindParentOrSelf<Border>());
            Assert.Same(border, text.FindParentOrSelf<Border>());
        });
    }

    [Fact]
    public void FindParentOrSelfWithPredicateChecksSelf()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (root, panel, _, _) = Build();

            // Act & Assert
            Assert.Same(panel, panel.FindParentOrSelf<Panel>(static x => HasTag(x, "panel")));
            Assert.Same(root, panel.FindParentOrSelf<Panel>(static x => HasTag(x, "root")));
            Assert.Same(root, panel.FindParentOrSelf<Panel, string>("root", static (x, s) => HasTag(x, s)));
        });
    }

    //------------------------------------------------------------
    // Children
    //------------------------------------------------------------

    [Fact]
    public void FindChildWithStatePassesState()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (root, _, border, _) = Build();

            // Act & Assert
            Assert.Same(border, root.FindChild<FrameworkElement, string>("border", static (x, s) => HasTag(x, s)));
            Assert.Null(root.FindChild<FrameworkElement, string>("none", static (x, s) => HasTag(x, s)));
        });
    }

    [Fact]
    public void FindChildrenWithStatePassesState()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (root, panel, border, _) = Build();

            // Act
            var children = root.FindChildren<FrameworkElement, object?>(null, static (x, s) => x.Tag != s).ToList();

            // Assert
            Assert.Equal([panel, border], children);
        });
    }

    [Fact]
    public void FindChildOrSelfReturnsSelfFirst()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (root, _, border, _) = Build();

            // Act & Assert
            Assert.Same(root, root.FindChildOrSelf<Grid>());
            Assert.Same(border, root.FindChildOrSelf<Border>());
            Assert.Null(root.FindChildOrSelf<Button>());
        });
    }

    [Fact]
    public void FindChildOrSelfWithPredicateChecksSelf()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (root, panel, _, _) = Build();

            // Act & Assert
            Assert.Same(root, root.FindChildOrSelf<Panel>(static x => HasTag(x, "root")));
            Assert.Same(panel, root.FindChildOrSelf<Panel>(static x => HasTag(x, "panel")));
            Assert.Same(panel, root.FindChildOrSelf<Panel, string>("panel", static (x, s) => HasTag(x, s)));
        });
    }
}
