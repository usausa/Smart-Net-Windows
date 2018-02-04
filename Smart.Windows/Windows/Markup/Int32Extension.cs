﻿namespace Smart.Windows.Markup
{
    using System;
    using System.Windows.Markup;

    [ContentProperty("Value")]
    [MarkupExtensionReturnType(typeof(int))]
    public class Int32Extension : MarkupExtension
    {
        public int Value { get; set; }

        public Int32Extension(int value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}
