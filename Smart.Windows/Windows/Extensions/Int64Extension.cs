namespace Smart.Windows.Extensions
{
    using System;
    using System.Windows.Markup;

    public class Int64Extension : MarkupExtension
    {
        private readonly long value;

        public Int64Extension(long value)
        {
            this.value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return value;
        }
    }
}
