namespace Smart.Windows.Interactivity;

using System.Windows;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(FrameworkElement))]
public sealed class DataContextDisposeAction : TriggerAction<FrameworkElement>
{
    protected override void Invoke(object parameter)
    {
        (AssociatedObject.DataContext as IDisposable)?.Dispose();
    }
}
