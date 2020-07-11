namespace Smart.Windows.Markup
{
    using System;
    using System.Windows.Markup;

    using Smart.Windows.Data;

    [MarkupExtensionReturnType(typeof(string))]
    public sealed class BoolToTextConverterExtension : MarkupExtension
    {
        public string True { get; set; }

        public string False { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BooleanToTextConverter { TrueValue = True, FalseValue = False };
        }
    }
}
