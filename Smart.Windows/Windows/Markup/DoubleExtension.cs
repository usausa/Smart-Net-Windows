﻿namespace Smart.Windows.Markup
{
    using System;
    using System.Windows.Markup;

    [ContentProperty("Value")]
    [MarkupExtensionReturnType(typeof(double))]
    public class DoubleExtension : MarkupExtension
    {
        public double Value { get; set; }

        public DoubleExtension(double value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}
