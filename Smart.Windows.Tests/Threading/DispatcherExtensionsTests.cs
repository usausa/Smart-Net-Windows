namespace Smart.Threading;

using System.Windows;
using System.Windows.Threading;

using Smart.Windows.Interactivity;

public sealed class DispatcherExtensionsTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

    [Fact]
    public void AsyncInvokePutsExceptionOfAsyncLambdaIntoTask()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var element = new DependencyObject();
            var unhandled = false;
            Dispatcher.CurrentDispatcher.UnhandledException += (_, e) =>
            {
                unhandled = true;
                e.Handled = true;
            };

            // Act
            var task = element.AsyncInvoke(static async () =>
            {
                await Task.Yield();
                throw new InvalidOperationException();
            });
            TestDispatcher.ProcessUntil(() => task.IsCompleted, Timeout);

            // Assert
            Assert.IsType<InvalidOperationException>(task.Exception?.InnerException);
            Assert.False(unhandled);
        });
    }

    [Fact]
    public void AsyncInvokeReturnsResultOfAsyncLambda()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var element = new DependencyObject();

            // Act
            var task = element.AsyncInvoke(static async () =>
            {
                await Task.Yield();
                return 42;
            });
            TestDispatcher.ProcessUntil(() => task.IsCompleted, Timeout);

            // Assert
            Assert.Equal(42, task.Result);
        });
    }

    [Fact]
    public void AsyncInvokeRunsSyncLambdaAsAction()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var element = new DependencyObject();
            var called = false;

            // Act
            var operation = element.AsyncInvoke(() => called = true);
            TestDispatcher.ProcessUntil(() => operation.Status == DispatcherOperationStatus.Completed, Timeout);

            // Assert
            Assert.True(called);
        });
    }

    [Fact]
    public void AsyncInvokeDoesNotRunAsyncLambdaWhenCanceledBeforeRun()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var element = new DependencyObject();
            var called = false;
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            // Act
            var task = element.AsyncInvoke(
                async () =>
                {
                    called = true;
                    await Task.Yield();
                },
                cancellation.Token);
            TestDispatcher.ProcessUntil(() => task.IsCompleted, Timeout);

            // Assert
            Assert.True(task.IsCanceled);
            Assert.False(called);
        });
    }

    [Fact]
    public void AsyncInvokeRunsAsyncLambdaOnDispatcherThreadFromOtherThread()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var element = new DependencyObject();
            var dispatcherThread = Environment.CurrentManagedThreadId;

            // Act
            var task = Task.Run(() => element.AsyncInvoke(static async () =>
            {
                await Task.Yield();
                return Environment.CurrentManagedThreadId;
            }));
            TestDispatcher.ProcessUntil(() => task.IsCompleted, Timeout);

            // Assert
            Assert.Equal(dispatcherThread, task.Result);
        });
    }
}
