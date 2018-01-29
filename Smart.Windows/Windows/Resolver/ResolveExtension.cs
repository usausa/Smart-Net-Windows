namespace Smart.Windows.Resolver
{
    using System;
    using System.Windows.Markup;

    public class ResolveExtension : MarkupExtension
    {
        [ConstructorArgument("type")]
        public Type Type { get; set; }

        public ResolveExtension()
        {
        }

        public ResolveExtension(Type type)
        {
            Type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ResolveProvider.Default.Resolve(Type);
        }
    }
}
