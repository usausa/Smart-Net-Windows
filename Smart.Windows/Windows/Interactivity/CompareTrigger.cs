namespace Smart.Windows.Interactivity;

using System.Windows;

using Microsoft.Xaml.Behaviors;

using Smart.Windows.Expressions;

[TypeConstraint(typeof(DependencyObject))]
public sealed class CompareTrigger : TriggerBase<DependencyObject>
{
    public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
        nameof(Binding),
        typeof(object),
        typeof(CompareTrigger),
        new PropertyMetadata(HandlePropertyChanged));

    public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
        nameof(Parameter),
        typeof(object),
        typeof(CompareTrigger),
        new PropertyMetadata(HandlePropertyChanged));

    public static readonly DependencyProperty ExpressionProperty = DependencyProperty.Register(
        nameof(Expression),
        typeof(ICompareExpression),
        typeof(CompareTrigger),
        new PropertyMetadata(CompareExpressions.Equal));

    public object? Binding
    {
        get => GetValue(BindingProperty);
        set => SetValue(BindingProperty, value);
    }

    public object? Parameter
    {
        get => GetValue(ParameterProperty);
        set => SetValue(ParameterProperty, value);
    }

    public ICompareExpression? Expression
    {
        get => (ICompareExpression)GetValue(ExpressionProperty);
        set => SetValue(ExpressionProperty, value);
    }

    private static void HandlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue == e.NewValue)
        {
            return;
        }

        ((CompareTrigger)d).HandlePropertyChanged();
    }

    private void HandlePropertyChanged()
    {
        var expression = Expression ?? CompareExpressions.Equal;
        if (expression.Eval(Binding, Parameter))
        {
            InvokeActions(null);
        }
    }
}
