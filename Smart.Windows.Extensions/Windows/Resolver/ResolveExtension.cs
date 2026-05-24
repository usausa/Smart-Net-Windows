namespace Smart.Windows.Resolver;

using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;

using Smart.Mvvm.Resolver;

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

    public override object? ProvideValue(IServiceProvider serviceProvider) => ResolveProvider.Default.GetService(Type);
}
