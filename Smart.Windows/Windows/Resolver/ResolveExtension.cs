namespace Smart.Windows.Resolver;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;

public sealed class ResolveExtension : MarkupExtension
{
    [ConstructorArgument("type")]
    [AllowNull]
    public Type Type { get; set; }

    public ResolveExtension()
    {
    }

    public ResolveExtension(Type type)
    {
        Type = type;
    }

    public override object? ProvideValue(IServiceProvider serviceProvider) => ResolveProvider.Default.Resolve(Type);
}
