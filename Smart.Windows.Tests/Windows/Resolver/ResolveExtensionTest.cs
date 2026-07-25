namespace Smart.Windows.Resolver;

using Smart.Mvvm.Resolver;

public sealed class ResolveExtensionTest : IDisposable
{
    private readonly IServiceProvider original = ResolveProvider.Default.Provider;

    public void Dispose() => ResolveProvider.Default.Provider = original;

    private sealed class StubProvider : IServiceProvider
    {
        private readonly object? service;

        public StubProvider(object? service) => this.service = service;

        public object? GetService(Type serviceType) => service;
    }

    private sealed class Service
    {
    }

    //------------------------------------------------------------------
    // Resolver
    //------------------------------------------------------------------

    [Fact]
    public void ThrowsWhenTypeIsNotRegistered()
    {
        // Arrange
        ResolveProvider.Default.Provider = new StubProvider(null);
        var extension = new ResolveExtension(typeof(Service));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => extension.ProvideValue(null!));
        Assert.Contains(nameof(Service), exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ReturnsResolvedInstance()
    {
        // Arrange
        var service = new Service();
        ResolveProvider.Default.Provider = new StubProvider(service);
        var extension = new ResolveExtension(typeof(Service));

        // Act
        var result = extension.ProvideValue(null!);

        // Assert
        Assert.Same(service, result);
    }
}
