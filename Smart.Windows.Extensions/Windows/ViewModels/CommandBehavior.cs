namespace Smart.Windows.ViewModels;

[Flags]
#pragma warning disable CA2217
public enum CommandBehavior
{
    None = 0,

    ControlByBusyState = 1 << 0,

    AllowBusyExecution = 1 << 1,

    Default = 1 << 31
}
#pragma warning restore CA2217
