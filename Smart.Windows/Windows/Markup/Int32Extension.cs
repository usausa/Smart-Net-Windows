namespace Smart.Windows.Markup
{
    using System;
    using System.Windows.Markup;

    public class Int32Extension : MarkupExtension
    {
        private readonly int value;

        public Int32Extension(int value)
        {
            this.value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return value;
        }
    }
}
