namespace Smart.Windows.Generator.Models;

internal sealed record PropertyChangedModel(
    string MethodName,
    bool HasParameters,
    string OldParameterType,
    string NewParameterType);
