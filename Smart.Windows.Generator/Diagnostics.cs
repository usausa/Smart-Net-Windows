namespace Smart.Windows.Generator;

using Microsoft.CodeAnalysis;

internal static class Diagnostics
{
    public static DiagnosticDescriptor InvalidPropertyDefinition { get; } = new(
        id: "SWD0001",
        title: "Invalid property definition",
        messageFormat: "[DependencyProperty] property must be partial. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor StaticPropertyNotSupported { get; } = new(
        id: "SWD0002",
        title: "Static property not supported",
        messageFormat: "[DependencyProperty] static property is not supported. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidPropertyAccessor { get; } = new(
        id: "SWD0003",
        title: "Invalid property accessor",
        messageFormat: "[DependencyProperty] property must have get/set without modifiers. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor ContainingTypeNotPartial { get; } = new(
        id: "SWD0004",
        title: "Containing type not partial",
        messageFormat: "[DependencyProperty] containing type must be partial. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidContainingType { get; } = new(
        id: "SWD0005",
        title: "Invalid containing type",
        messageFormat: "[DependencyProperty] containing type is not DependencyObject. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor GenericTypeNotSupported { get; } = new(
        id: "SWD0006",
        title: "Generic type not supported",
        messageFormat: "[DependencyProperty] generic containing type is not supported. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor DefaultValueConflict { get; } = new(
        id: "SWD0007",
        title: "DefaultValue conflict",
        messageFormat: "[DependencyProperty] DefaultValue and DefaultValueExpression conflict. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor CallbackMethodNotFound { get; } = new(
        id: "SWD0008",
        title: "Callback method not found",
        messageFormat: "[DependencyProperty] callback method is not found. method=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidCallbackMethod { get; } = new(
        id: "SWD0009",
        title: "Invalid callback method",
        messageFormat: "[DependencyProperty] callback method signature is invalid. method=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidDefaultValue { get; } = new(
        id: "SWD0010",
        title: "Invalid default value",
        messageFormat: "[DependencyProperty] DefaultValue is not a supported constant. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}
