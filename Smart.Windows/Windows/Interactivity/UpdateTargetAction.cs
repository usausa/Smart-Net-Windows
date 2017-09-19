namespace Smart.Windows.Interactivity
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(FrameworkElement))]
    public class UpdateTargetAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(UpdateTargetAction));

        private DependencyPropertyDescriptor dpd;

        /// <summary>
        ///
        /// </summary>
        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            UpdatePropertyDescriptor();
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDetaching()
        {
            dpd = null;

            base.OnDetaching();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdatePropertyDescriptor();
        }

        /// <summary>
        ///
        /// </summary>
        private void UpdatePropertyDescriptor()
        {
            dpd = null;
            if ((AssociatedObject != null) && !String.IsNullOrEmpty(PropertyName))
            {
                dpd = DependencyPropertyDescriptor.FromName(PropertyName, AssociatedObject.GetType(), AssociatedObject.GetType());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (dpd == null)
            {
                return;
            }

            var binding = AssociatedObject.GetBindingExpression(dpd.DependencyProperty);
            binding?.UpdateTarget();
        }
    }
}
