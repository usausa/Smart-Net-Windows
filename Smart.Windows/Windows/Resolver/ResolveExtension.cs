namespace Smart.Windows.Resolver
{
    using System;
    using System.Windows.Markup;

    /// <summary>
    ///
    /// </summary>
    public sealed class ResolveExtension : MarkupExtension
    {
        /// <summary>
        ///
        /// </summary>
        [ConstructorArgument("type")]
        public Type Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public ResolveExtension()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        public ResolveExtension(Type type)
        {
            Type = type;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ResolveProvider.Default.Resolve(Type);
        }
    }
}
