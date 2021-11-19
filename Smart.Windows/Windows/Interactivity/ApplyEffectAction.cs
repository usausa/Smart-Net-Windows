namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Media.Effects;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(FrameworkElement))]
public sealed class ApplyEffectAction : TriggerAction<FrameworkElement>
{
    public static readonly DependencyProperty EffectProperty = DependencyProperty.Register(
        nameof(Effect),
        typeof(Effect),
        typeof(ApplyEffectAction));

    public Effect Effect
    {
        get => (Effect)GetValue(EffectProperty);
        set => SetValue(EffectProperty, value);
    }

    protected override void Invoke(object parameter)
    {
        AssociatedObject.Effect = parameter as Effect ?? Effect;
    }
}
