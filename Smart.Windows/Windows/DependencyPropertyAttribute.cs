namespace Smart.Windows;

using System;
using System.Windows;

[AttributeUsage(AttributeTargets.Property)]
public sealed class DependencyPropertyAttribute : Attribute
{
    public object? DefaultValue { get; set; }

    public string? DefaultValueExpression { get; set; }

    public FrameworkPropertyMetadataOptions Options { get; set; }

    public string? PropertyChanged { get; set; }

    public string? Coerce { get; set; }

    public string? Validate { get; set; }
}
