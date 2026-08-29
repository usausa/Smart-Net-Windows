namespace Smart.Windows.Generator.Models;

using SourceGenerateHelper;

internal sealed record TypeModel(
    string Namespace,
    string ClassName,
    EquatableArray<ContainingTypeModel> ContainingTypes,
    EquatableArray<PropertyModel> Properties);
