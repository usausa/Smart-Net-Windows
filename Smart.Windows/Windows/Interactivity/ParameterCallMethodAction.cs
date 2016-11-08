namespace Smart.Windows.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    public class ParameterCallMethodAction : TriggerAction<DependencyObject>
    {
        private readonly List<MethodInfo> methods = new List<MethodInfo>();

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(ParameterCallMethodAction), new PropertyMetadata(OnTargetObjectChanged));

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register("MethodName", typeof(string), typeof(ParameterCallMethodAction), new PropertyMetadata(OnMethodNameChanged));

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty MethodParameterProperty = DependencyProperty.Register("MethodParameter", typeof(object), typeof(ParameterCallMethodAction), new PropertyMetadata(null));

        /// <summary>
        ///
        /// </summary>
        public object TargetObject
        {
            get { return GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        /// <summary>
        ///
        /// </summary>
        public string MethodName
        {
            get { return (string)GetValue(MethodNameProperty); }
            set { SetValue(MethodNameProperty, value); }
        }

        /// <summary>
        ///
        /// </summary>
        public object MethodParameter
        {
            get { return GetValue(MethodParameterProperty); }
            set { SetValue(MethodParameterProperty, value); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            var target = TargetObject;
            var argument = MethodParameter;
            foreach (var method in methods.Where(m => (argument == null) || argument.GetType().IsAssignableFrom(m.GetParameters()[0].ParameterType)))
            {
                method.Invoke(target, new[] { argument });
                return;
            }

            throw new ArgumentException("Valid method not found.");
        }

        /// <summary>
        ///
        /// </summary>
        private void UpdateMethodInfo()
        {
            methods.Clear();
            if ((TargetObject != null) && !String.IsNullOrEmpty(MethodName))
            {
                methods.AddRange(TargetObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(method =>
                        String.Equals(method.Name, MethodName, StringComparison.Ordinal) &&
                        (method.ReturnType == typeof(void)) &&
                        (method.GetParameters().Length == 1)));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnTargetObjectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var action = (ParameterCallMethodAction)sender;
            action.UpdateMethodInfo();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var action = (ParameterCallMethodAction)sender;
            action.UpdateMethodInfo();
        }
    }
}
