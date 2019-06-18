namespace Smart.Windows.Interactivity
{
    using System.Reflection;
    using System.Windows;

    using Microsoft.Xaml.Behaviors;

    using Smart.Windows.Messaging;

    [TypeConstraint(typeof(DependencyObject))]
    public sealed class ResolvePropertyAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
            nameof(TargetObject),
            typeof(object),
            typeof(ResolvePropertyAction),
            new PropertyMetadata(null));

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            nameof(PropertyName),
            typeof(object),
            typeof(ResolvePropertyAction),
            new PropertyMetadata(null));

        public object TargetObject
        {
            get => GetValue(TargetObjectProperty);
            set => SetValue(TargetObjectProperty, value);
        }

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        private PropertyInfo property;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        protected override void Invoke(object parameter)
        {
            var target = TargetObject ?? AssociatedObject;
            var propertyName = PropertyName;
            if ((target is null) || (propertyName is null))
            {
                return;
            }

            if ((property is null) ||
                (property.DeclaringType != target.GetType()) ||
                (property.Name != propertyName))
            {
                property = target.GetType().GetRuntimeProperty(propertyName);
            }

            var eventArgs = (ResultEventArgs)parameter;
            eventArgs.Result = property.GetValue(target);
        }
    }
}
