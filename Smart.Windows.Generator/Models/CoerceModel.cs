namespace Smart.Windows.Generator.Models;

internal sealed record CoerceModel(
    string MethodName,
    bool IsStatic,
    string ParameterType);
