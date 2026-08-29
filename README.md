# Smart.Windows .NET - MVVM helper library for WPF

[![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Windows.svg)](https://www.nuget.org/packages/Usa.Smart.Windows/)

## Features

* Basic converters.
* Observable commands.
* Actions, Behaviors and Triggers.
* Markup extensions.
* Messenger.
* Resolver(DI Container) integration.
* Base class for ViewModel.
* DependencyProperty source generator.

## DependencyProperty generator

Add `[DependencyProperty]` to a partial property, and the `DependencyProperty` field and the property implementation are generated.

```csharp
public partial class GaugeControl : FrameworkElement
{
    [DependencyProperty(DefaultValue = 0d, Options = FrameworkPropertyMetadataOptions.AffectsRender, PropertyChanged = nameof(OnLevelChanged), Coerce = nameof(CoerceLevel))]
    public partial double Level { get; set; }

    [DependencyProperty(DefaultValueExpression = "global::System.Windows.Media.Brushes.SteelBlue")]
    public partial Brush? BarBrush { get; set; }

    private void OnLevelChanged(double oldValue, double newValue)
    {
    }

    private double CoerceLevel(double value) => Math.Clamp(value, 0d, 100d);
}
```

| Option | Note |
|-|-|
| `DefaultValue` | Default value of the property |
| `DefaultValueExpression` | Default value as an expression, for values that can not be written as a constant |
| `Options` | `FrameworkPropertyMetadataOptions` |
| `PropertyChanged` | Name of a `void` method with no parameters, or with `(T oldValue, T newValue)` |
| `Coerce` | Name of a `T` method with `(T value)` |
| `Validate` | Name of a `static bool` method with `(T value)` |

Requires C# 13 or later, because partial properties are used.

## NuGet

| Package | Note  |
|-|-|
| [![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Windows.svg)](https://www.nuget.org/packages/Usa.Smart.Windows/) | Core libyrary |
| [![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Windows.Behaviors.svg)](https://www.nuget.org/packages/Usa.Smart.Windows.Behaviors/) | Behaivors |
| [![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Windows.Extensions.svg)](https://www.nuget.org/packages/Usa.Smart.Windows.Extensions/) | Extension helpers |
| [![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Windows.Hosting.svg)](https://www.nuget.org/packages/Usa.Smart.Windows.Hosting/) | Hosting helpers |

## Link

* [Smart.Mvvm](https://github.com/usausa/Smart-Net-Mvvm)
* [Smart.Resolver](https://github.com/usausa/Smart-Net-Resolver)
* [Smart.Navigation](https://github.com/usausa/Smart-Net-Navigation)
