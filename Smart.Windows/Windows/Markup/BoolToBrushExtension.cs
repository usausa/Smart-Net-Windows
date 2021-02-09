namespace Smart.Windows.Markup
{
    using System;
    using System.Windows.Markup;
    using System.Windows.Media;

    using Smart.Windows.Data;

    [MarkupExtensionReturnType(typeof(Brush))]
    public sealed class BoolToBrushExtension : MarkupExtension
    {
        public Brush True { get; set; } = Brushes.Transparent;

        public Brush False { get; set; } = Brushes.Transparent;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BooleanToBrushConverter { TrueValue = True, FalseValue = False };
        }
    }
}
