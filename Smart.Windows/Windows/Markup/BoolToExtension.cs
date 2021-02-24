namespace Smart.Windows.Markup
{
    using System;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;

    using Smart.Windows.Data;

    [MarkupExtensionReturnType(typeof(BoolToColorConverter))]
    public sealed class BoolToTextExtension : MarkupExtension
    {
        public string True { get; set; }

        public string False { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BoolToTextConverter { TrueValue = True, FalseValue = False };
        }
    }

    [MarkupExtensionReturnType(typeof(BoolToColorConverter))]
    public sealed class BoolToVisibilityExtension : MarkupExtension
    {
        public Visibility True { get; set; }

        public Visibility False { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BoolToVisibilityConverter { TrueValue = True, FalseValue = False };
        }
    }

    [MarkupExtensionReturnType(typeof(BoolToColorConverter))]
    public sealed class BoolToBrushExtension : MarkupExtension
    {
        public Brush True { get; set; } = Brushes.Transparent;

        public Brush False { get; set; } = Brushes.Transparent;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BoolToBrushConverter { TrueValue = True, FalseValue = False };
        }
    }

    [MarkupExtensionReturnType(typeof(BoolToColorConverter))]
    public sealed class BoolToColorExtension : MarkupExtension
    {
        public Color True { get; set; } = Colors.Transparent;

        public Color False { get; set; } = Colors.Transparent;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new BoolToColorConverter { TrueValue = True, FalseValue = False };
        }
    }
}
