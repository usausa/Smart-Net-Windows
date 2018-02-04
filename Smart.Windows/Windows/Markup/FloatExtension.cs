namespace Smart.Windows.Markup
{
    using System;
    using System.Windows.Markup;

    public class FloatExtension : MarkupExtension
    {
        private readonly float value;

        public FloatExtension(float value)
        {
            this.value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return value;
        }
    }
}
