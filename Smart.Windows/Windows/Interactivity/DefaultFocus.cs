namespace Smart.Windows.Interactivity;

using System.Windows;

public static class DefaultFocus
{
    public static readonly DependencyProperty IsTargetProperty = DependencyProperty.RegisterAttached(
        "IsTarget",
        typeof(bool),
        typeof(DefaultFocus),
        new PropertyMetadata(false));

    public static bool GetIsTarget(DependencyObject obj) => (bool)obj.GetValue(IsTargetProperty);

    public static void SetIsTarget(DependencyObject obj, bool value) => obj.SetValue(IsTargetProperty, value);
}
