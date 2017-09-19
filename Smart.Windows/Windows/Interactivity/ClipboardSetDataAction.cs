namespace Smart.Windows.Interactivity
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(DependencyObject))]
    public class ClipboardSetDataAction : TriggerAction<DependencyObject>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
            nameof(TargetObject),
            typeof(object),
            typeof(ClipboardSetDataAction));

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
            nameof(MethodName),
            typeof(string),
            typeof(ClipboardSetDataAction));

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
            nameof(Format),
            typeof(string),
            typeof(ClipboardSetDataAction));

        /// <summary>
        ///
        /// </summary>
        public object TargetObject
        {
            get => GetValue(TargetObjectProperty);
            set => SetValue(TargetObjectProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        public string MethodName
        {
            get => (string)GetValue(MethodNameProperty);
            set => SetValue(MethodNameProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            var method = TargetObject.GetType().GetMethod(MethodName, BindingFlags.Instance | BindingFlags.Public);
            var result = method.Invoke(TargetObject, null);
            Clipboard.SetData(Format, result);
        }
    }
}
