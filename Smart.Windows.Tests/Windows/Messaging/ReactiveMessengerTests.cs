namespace Smart.Windows.Messaging;

public sealed class ReactiveMessengerTests
{
    private sealed class TestMessage
    {
        public int Value { get; set; }
    }

    //------------------------------------------------------------------
    // Instance
    //------------------------------------------------------------------

    [Fact]
    public void InstancesAreIsolated()
    {
        // Arrange
        using var messenger1 = new ReactiveMessenger();
        using var messenger2 = new ReactiveMessenger();

        var received1 = 0;
        var received2 = 0;
        using var subscription1 = messenger1.Observe<TestMessage>().Subscribe(_ => received1++);
        using var subscription2 = messenger2.Observe<TestMessage>().Subscribe(_ => received2++);

        // Act
        messenger1.Send(new TestMessage());

        // Assert
        Assert.Equal(1, received1);
        Assert.Equal(0, received2);
    }

    [Fact]
    public void MessageIsDeliveredToAllObservers()
    {
        // Arrange
        using var messenger = new ReactiveMessenger();
        var received1 = 0;
        var received2 = 0;
        using var subscription1 = messenger.Observe<TestMessage>().Subscribe(_ => received1++);
        using var subscription2 = messenger.Observe<TestMessage>().Subscribe(_ => received2++);

        // Act
        messenger.Send(new TestMessage { Value = 1 });

        // Assert
        Assert.Equal(1, received1);
        Assert.Equal(1, received2);
    }

    [Fact]
    public void MessageIsDeliveredByType()
    {
        // Arrange
        using var messenger = new ReactiveMessenger();
        var received = 0;
        using var subscription = messenger.Observe<TestMessage>().Subscribe(_ => received++);

        // Act
        messenger.Send("other type");

        // Assert
        Assert.Equal(0, received);
    }

    [Fact]
    public void UnsubscribedObserverDoesNotReceive()
    {
        // Arrange
        using var messenger = new ReactiveMessenger();
        var received = 0;
        var subscription = messenger.Observe<TestMessage>().Subscribe(_ => received++);

        // Act
        subscription.Dispose();
        messenger.Send(new TestMessage());

        // Assert
        Assert.Equal(0, received);
    }

    //------------------------------------------------------------------
    // State
    //------------------------------------------------------------------

    [Fact]
    public void HasObserversReflectsSubscription()
    {
        // Arrange
        using var messenger = new ReactiveMessenger();

        // Assert
        Assert.False(messenger.HasObservers<TestMessage>());

        using (messenger.Observe<TestMessage>().Subscribe(static _ => { }))
        {
            Assert.True(messenger.HasObservers<TestMessage>());
        }

        Assert.False(messenger.HasObservers<TestMessage>());
    }

    //------------------------------------------------------------------
    // Lifetime
    //------------------------------------------------------------------

    [Fact]
    public void DisposeCompletesObservers()
    {
        // Arrange
        var messenger = new ReactiveMessenger();
        var completed = false;
        using var subscription = messenger.Observe<TestMessage>().Subscribe(static _ => { }, () => completed = true);

        // Act
        messenger.Dispose();

        // Assert
        Assert.True(completed);
    }

    [Fact]
    public void UseAfterDisposeThrows()
    {
        // Arrange
        var messenger = new ReactiveMessenger();
        messenger.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => messenger.Send(new TestMessage()));
    }

    //------------------------------------------------------------------
    // Concurrency
    //------------------------------------------------------------------

    [Fact]
    public void ConcurrentSendDeliversAllMessages()
    {
        // Arrange
        using var messenger = new ReactiveMessenger();
        const int count = 1000;
        var received = 0;
        using var subscription = messenger.Observe<TestMessage>().Subscribe(_ => Interlocked.Increment(ref received));

        // Act
        // ReSharper disable once AccessToDisposedClosure
        Parallel.For(0, count, i => messenger.Send(new TestMessage { Value = i }));

        // Assert
        Assert.Equal(count, received);
    }
}
