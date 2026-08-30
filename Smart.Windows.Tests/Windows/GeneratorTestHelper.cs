namespace Smart.Windows;

using System.Collections.Generic;

using Microsoft.CodeAnalysis;

using Smart.Windows.Generator;

using SourceGenerateHelper.Testing;

internal static class GeneratorTestHelper
{
    private static GeneratorTestRunner Runner => GeneratorTestRunner
        .For<DependencyPropertyGenerator>()
        .WithReference(typeof(DependencyPropertyAttribute).Assembly)
        .WithReference(typeof(System.Windows.DependencyObject).Assembly)
        .WithReference(typeof(System.Windows.FrameworkPropertyMetadataOptions).Assembly)
        .WithDiagnosticPrefix("SWD")
        .VerifyCompiles();

    public static IReadOnlyList<Diagnostic> GetDiagnostics(string source) => Runner.GetDiagnostics(source);

    public static IReadOnlyList<Diagnostic> GetDiagnosticsAll(string source) => Runner.GetDiagnosticsAll(source);

    // Used when the generated code can not compile by design, such as a type with no known base type
    public static IReadOnlyList<Diagnostic> GetDiagnosticsWithoutVerify(string source) =>
        Runner.VerifyCompiles(false).GetDiagnostics(source);

    public static string GetGeneratedSource(string source) => Runner.GetGeneratedSource(source);

    public static IncrementalRunResult RunIncremental(string source, string addedSource) =>
        Runner.WithTracking().RunIncremental(source, addedSource);
}
