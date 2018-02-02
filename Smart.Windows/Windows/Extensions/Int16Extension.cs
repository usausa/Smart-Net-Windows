namespace Smart.Windows.Extensions
{
    using System;
    using System.Windows.Markup;

    public class Int16Extension : MarkupExtension
    {
        private readonly short value;

        public Int16Extension(short value)
        {
            this.value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return value;
        }
    }
}
