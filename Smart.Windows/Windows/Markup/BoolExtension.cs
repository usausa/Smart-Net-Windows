namespace Smart.Windows.Markup
{
    using System;
    using System.Windows.Markup;

    public class BoolExtension : MarkupExtension
    {
        private readonly bool value;

        public BoolExtension(bool value)
        {
            this.value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return value;
        }
    }
}
