namespace Smart.Windows.Resolver;

using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;

public sealed class ResolveExtension : MarkupExtension
{
    [ConstructorArgument("type")]
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public Type Type { get; set; } = default!;

    public ResolveExtension()
    {
    }

    public ResolveExtension([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type)
    {
        Type = type;
    }

    public override object? ProvideValue(IServiceProvider serviceProvider) => ResolveHelper.Resolve(Type);
}
