namespace Smart.Windows.Markup
{
    using System;
    using System.Windows.Markup;
    using System.Windows.Media;

    using Smart.Windows.Data;

    [MarkupExtensionReturnType(typeof(Color))]
    public sealed class BoolToColorConverterExtension : MarkupExtension
    {
        public Color True { get; set; } = Colors.Transparent;

        public Color False { get; set; } = Colors.Transparent;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BooleanToColorConverter { TrueValue = True, FalseValue = False };
        }
    }
}
