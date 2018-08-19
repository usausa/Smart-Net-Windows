namespace Smart.Windows.Interactivity
{
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Interactivity;

    using Smart.Windows.Messaging;

    [TypeConstraint(typeof(DependencyObject))]
    public sealed class ResolveMethodAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
            nameof(TargetObject),
            typeof(object),
            typeof(ResolveMethodAction),
            new PropertyMetadata(null));

        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
            nameof(MethodName),
            typeof(object),
            typeof(ResolveMethodAction),
            new PropertyMetadata(null));

        public object TargetObject
        {
            get => GetValue(TargetObjectProperty);
            set => SetValue(TargetObjectProperty, value);
        }

        public string MethodName
        {
            get => (string)GetValue(MethodNameProperty);
            set => SetValue(MethodNameProperty, value);
        }

        private MethodInfo cachedMethod;

        protected override void Invoke(object parameter)
        {
            var target = TargetObject ?? AssociatedObject;
            var methodName = MethodName;
            if ((target == null) || (methodName == null))
            {
                return;
            }

            if ((cachedMethod == null) ||
                (cachedMethod.DeclaringType != target.GetType() ||
                 (cachedMethod.Name != methodName)))
            {
                var methodInfo = target.GetType().GetRuntimeMethods().FirstOrDefault(m =>
                    m.Name == methodName &&
                    (m.GetParameters().Length == 0));
                if (methodInfo == null)
                {
                    return;
                }

                cachedMethod = methodInfo;
            }

            var eventArgs = (ResultEventArgs)parameter;
            eventArgs.Result = cachedMethod.Invoke(target, null);
        }
    }
}
