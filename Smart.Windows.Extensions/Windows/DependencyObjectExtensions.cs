namespace Smart.Windows;

using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows;

public static class DependencyObjectExtensions
{
    public static IObservable<TValue> PropertyChangedAsObservable<TValue>(
        this DependencyObject source,
        DependencyProperty property)
    {
        return Observable.Create<TValue>(observer =>
        {
            var descriptor = DependencyPropertyDescriptor.FromProperty(property, source.GetType());

            void OnPropertyChanged(object? sender, EventArgs e) => observer.OnNext((TValue)source.GetValue(property));

            descriptor.AddValueChanged(source, OnPropertyChanged);

            return () => descriptor.RemoveValueChanged(source, OnPropertyChanged);
        });
    }
}
