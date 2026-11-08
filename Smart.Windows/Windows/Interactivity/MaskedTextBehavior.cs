namespace Smart.Windows.Interactivity;

using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(TextBox))]
public sealed class MaskedTextBehavior : Behavior<TextBox>
{
    public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(
        nameof(Mask),
        typeof(string),
        typeof(MaskedTextBehavior),
        new PropertyMetadata(null, HandleProviderPropertyChanged));

    public static readonly DependencyProperty PromptCharProperty = DependencyProperty.Register(
        nameof(PromptChar),
        typeof(char),
        typeof(MaskedTextBehavior),
        new PropertyMetadata('_', HandlePromptCharPropertyChanged));

    public static readonly DependencyProperty IncludeLiteralsProperty = DependencyProperty.Register(
        nameof(IncludeLiterals),
        typeof(bool),
        typeof(MaskedTextBehavior),
        new PropertyMetadata(true));

    public static readonly DependencyProperty NormalizerProperty = DependencyProperty.Register(
        nameof(Normalizer),
        typeof(Func<string, string>),
        typeof(MaskedTextBehavior),
        new PropertyMetadata(null, HandleProviderPropertyChanged));

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(string),
        typeof(MaskedTextBehavior),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, HandleValuePropertyChanged));

    public string? Mask
    {
        get => (string?)GetValue(MaskProperty);
        set => SetValue(MaskProperty, value);
    }

    public char PromptChar
    {
        get => (char)GetValue(PromptCharProperty);
        set => SetValue(PromptCharProperty, value);
    }

    public bool IncludeLiterals
    {
        get => (bool)GetValue(IncludeLiteralsProperty);
        set => SetValue(IncludeLiteralsProperty, value);
    }

    public Func<string, string>? Normalizer
    {
        get => (Func<string, string>?)GetValue(NormalizerProperty);
        set => SetValue(NormalizerProperty, value);
    }

#pragma warning disable CA1721
    public string? Value
    {
        get => (string?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
#pragma warning restore CA1721

    private MaskedTextProvider? provider;

    private bool updating;

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.PreviewTextInput += OnPreviewTextInput;
        AssociatedObject.TextChanged += OnTextChanged;
        DataObject.AddPastingHandler(AssociatedObject, OnPasting);
        CommandManager.AddPreviewExecutedHandler(AssociatedObject, OnPreviewExecuted);

        UpdateProvider();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
        AssociatedObject.TextChanged -= OnTextChanged;
        DataObject.RemovePastingHandler(AssociatedObject, OnPasting);
        CommandManager.RemovePreviewExecutedHandler(AssociatedObject, OnPreviewExecuted);

        provider = null;
        AssociatedObject.ClearValue(InputMethod.IsInputMethodEnabledProperty);

        base.OnDetaching();
    }

    private static void HandleProviderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (MaskedTextBehavior)d;
        if (behavior.AssociatedObject is not null)
        {
            behavior.UpdateProvider();
        }
    }

    private static void HandlePromptCharPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (MaskedTextBehavior)d;
        if (behavior.provider is not null)
        {
            behavior.provider.PromptChar = (char)e.NewValue;
            behavior.Render(behavior.provider, behavior.AssociatedObject.CaretIndex);
        }
    }

    private static void HandleValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (MaskedTextBehavior)d;
        if (behavior.updating || (behavior.provider is null))
        {
            return;
        }

        behavior.Load(behavior.provider, (string?)e.NewValue ?? string.Empty);
    }

    private void UpdateProvider()
    {
        var mask = Mask;
        if (String.IsNullOrEmpty(mask))
        {
            provider = null;
            AssociatedObject.ClearValue(InputMethod.IsInputMethodEnabledProperty);
            return;
        }

        provider = new MaskedTextProvider(mask, CultureInfo.CurrentCulture) { PromptChar = PromptChar };
        InputMethod.SetIsInputMethodEnabled(AssociatedObject, false);
        Load(provider, Value ?? AssociatedObject.Text);
    }

    private void Load(MaskedTextProvider current, string text)
    {
        if (!current.Set(Normalize(text)))
        {
            current.Clear();
        }

        Render(current, current.LastAssignedPosition + 1);
    }

    private string Normalize(string text) => Normalizer?.Invoke(text) ?? text;

    private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if ((provider is null) || AssociatedObject.IsReadOnly || String.IsNullOrEmpty(e.Text))
        {
            return;
        }

        e.Handled = true;
        Insert(provider, e.Text);
    }

    private void OnPasting(object sender, DataObjectPastingEventArgs e)
    {
        if (provider is null)
        {
            return;
        }

        e.CancelCommand();

        if (!AssociatedObject.IsReadOnly && (e.DataObject.GetData(DataFormats.UnicodeText) is string text))
        {
            Insert(provider, text);
        }
    }

    private void OnPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if ((provider is null) || AssociatedObject.IsReadOnly)
        {
            return;
        }

        if ((e.Command == EditingCommands.Backspace) || (e.Command == EditingCommands.DeletePreviousWord))
        {
            e.Handled = true;
            Remove(provider, false);
        }
        else if ((e.Command == EditingCommands.Delete) || (e.Command == EditingCommands.DeleteNextWord))
        {
            e.Handled = true;
            Remove(provider, true);
        }
        else if (e.Command == ApplicationCommands.Cut)
        {
            e.Handled = true;
            if (AssociatedObject.SelectionLength > 0)
            {
                ApplicationCommands.Copy.Execute(null, AssociatedObject);
                Remove(provider, true);
            }
        }
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (updating || (provider is null))
        {
            return;
        }

        var work = (MaskedTextProvider)provider.Clone();
        if (work.Set(Normalize(AssociatedObject.Text)))
        {
            provider = work;
            Commit(work, work.LastAssignedPosition + 1);
        }
        else
        {
            Render(provider, AssociatedObject.CaretIndex);
        }
    }

    private void Insert(MaskedTextProvider current, string text)
    {
        var input = Normalize(text);
        if (input.Length == 0)
        {
            return;
        }

        var work = (MaskedTextProvider)current.Clone();
        var start = AssociatedObject.SelectionStart;
        var length = AssociatedObject.SelectionLength;
        if (length > 0)
        {
            work.RemoveAt(start, start + length - 1);
        }

        if (!work.InsertAt(input, Math.Min(start, work.LastAssignedPosition + 1), out var position, out _))
        {
            return;
        }

        provider = work;
        var next = work.FindEditPositionFrom(position + 1, true);
        Commit(work, next >= 0 ? next : work.Length);
    }

    private void Remove(MaskedTextProvider current, bool forward)
    {
        var start = AssociatedObject.SelectionStart;
        var length = AssociatedObject.SelectionLength;
        if (length > 0)
        {
            current.RemoveAt(start, start + length - 1);
            Commit(current, start);
            return;
        }

        var position = forward ? current.FindEditPositionFrom(start, true) : start > 0 ? current.FindEditPositionFrom(start - 1, false) : -1;
        if (position < 0)
        {
            return;
        }

        current.RemoveAt(position);
        Commit(current, forward ? start : position);
    }

    private void Commit(MaskedTextProvider current, int caret)
    {
        Render(current, caret);

        updating = true;
        SetCurrentValue(ValueProperty, current.AssignedEditPositionCount == 0 ? string.Empty : current.ToString(false, IncludeLiterals));
        updating = false;
    }

    private void Render(MaskedTextProvider current, int caret)
    {
        updating = true;
        AssociatedObject.SetCurrentValue(TextBox.TextProperty, current.ToDisplayString());
        AssociatedObject.Select(Math.Min(caret, current.Length), 0);
        updating = false;
    }
}
