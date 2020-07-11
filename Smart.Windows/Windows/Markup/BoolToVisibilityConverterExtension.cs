namespace Smart.Windows.Markup
{
    using System;
    using System.Windows;
    using System.Windows.Markup;

    using Smart.Windows.Data;

    [MarkupExtensionReturnType(typeof(Visibility))]
    public sealed class BoolToVisibilityConverter : MarkupExtension
    {
        public Visibility True { get; set; }

        public Visibility False { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BooleanToVisibilityConverter { TrueValue = True, FalseValue = False };
        }
    }
}
