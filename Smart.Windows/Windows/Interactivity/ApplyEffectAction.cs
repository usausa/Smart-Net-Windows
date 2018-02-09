namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Interactivity;
    using System.Windows.Media.Effects;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(FrameworkElement))]
    public sealed class ApplyEffectAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty EffectProperty = DependencyProperty.Register(
            nameof(Effect),
            typeof(Effect),
            typeof(ApplyEffectAction),
            new PropertyMetadata(null));

        /// <summary>
        ///
        /// </summary>
        public Effect Effect
        {
            get => (Effect)GetValue(EffectProperty);
            set => SetValue(EffectProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            AssociatedObject.Effect = parameter as Effect ?? Effect;
        }
    }
}
