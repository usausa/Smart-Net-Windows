namespace Smart.Windows.Interactivity;

using System.Windows;

using Microsoft.Xaml.Behaviors;

using Smart.Windows.Expressions;

public sealed class CompareStateBehavior : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(FrameworkElement),
        typeof(CompareStateBehavior));

    public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
        nameof(Binding),
        typeof(object),
        typeof(CompareStateBehavior),
        new PropertyMetadata(HandlePropertyChanged));

    public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
        nameof(Parameter),
        typeof(object),
        typeof(CompareStateBehavior),
        new PropertyMetadata(HandlePropertyChanged));

    public static readonly DependencyProperty ExpressionProperty = DependencyProperty.Register(
        nameof(Expression),
        typeof(ICompareExpression),
        typeof(CompareStateBehavior),
        new PropertyMetadata(CompareExpressions.Equal));

    public static readonly DependencyProperty TrueStateProperty = DependencyProperty.Register(
        nameof(TrueState),
        typeof(string),
        typeof(CompareStateBehavior),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty FalseStateProperty = DependencyProperty.Register(
        nameof(FalseState),
        typeof(string),
        typeof(CompareStateBehavior),
        new PropertyMetadata(string.Empty));

    public FrameworkElement? TargetObject
    {
        get => (FrameworkElement)GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

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

    public string TrueState
    {
        get => (string)GetValue(TrueStateProperty);
        set => SetValue(TrueStateProperty, value);
    }

    public string FalseState
    {
        get => (string)GetValue(FalseStateProperty);
        set => SetValue(FalseStateProperty, value);
    }

    private static void HandlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue == e.NewValue)
        {
            return;
        }

        ((CompareStateBehavior)d).HandlePropertyChanged();
    }

    private void HandlePropertyChanged()
    {
        var target = TargetObject ?? AssociatedObject;
        if (target is null)
        {
            return;
        }

        var expression = Expression ?? CompareExpressions.Equal;
        var stateName = expression.Eval(Binding, Parameter) ? TrueState : FalseState;
        VisualStateUtilities.GoToState(target, stateName, true);
    }
}
