namespace Smart.Windows.Internal;

internal static class Functions
{
    public static Func<bool> True { get; } = static () => true;
}

internal static class Functions<T>
{
    public static Func<T, bool> True { get; } = static _ => true;
}
