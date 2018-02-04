namespace Smart.Windows.Markup
{
    using System;
    using System.Windows.Markup;

    public class DoubleExtension : MarkupExtension
    {
        private readonly double value;

        public DoubleExtension(double value)
        {
            this.value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return value;
        }
    }
}
