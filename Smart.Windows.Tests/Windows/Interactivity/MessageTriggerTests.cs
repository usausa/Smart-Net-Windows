namespace Smart.Windows.Interactivity;

using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

using Smart.Mvvm.Messaging;

public sealed class MessageTriggerTests
{
    private sealed class RecordingAction : TriggerAction<DependencyObject>
    {
        public int Count { get; private set; }

        public object? LastParameter { get; private set; }

        protected override void Invoke(object parameter)
        {
            Count++;
            LastParameter = parameter;
        }
    }

    private sealed class CustomMessenger : IMessenger
    {
        public event EventHandler<MessengerEventArgs>? Received;

        public void Send(string label) => Received?.Invoke(this, new MessengerEventArgs(label, typeof(object), null));

        public void Send<T>(T message) => Received?.Invoke(this, new MessengerEventArgs(string.Empty, typeof(T), message));

        public void Send<T>(string label, T parameter) => Received?.Invoke(this, new MessengerEventArgs(label, typeof(T), parameter));
    }

    private static void RunSta(Action action)
    {
        ExceptionDispatchInfo? dispatchInfo = null;
        var thread = new Thread(() =>
        {
#pragma warning disable CA1031
            try
            {
                action();
            }
            catch (Exception ex)
            {
                dispatchInfo = ExceptionDispatchInfo.Capture(ex);
            }
#pragma warning restore CA1031
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        dispatchInfo?.Throw();
    }

    private static void RaiseLoaded(FrameworkElement element) =>
        element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent, element));

    private static void RaiseUnloaded(FrameworkElement element) =>
        element.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent, element));

    //------------------------------------------------------------------
    // Messenger
    //------------------------------------------------------------------

    [Fact]
    public void ReactsAfterUnloadAndReload() => RunSta(() =>
    {
        // Arrange
        var element = new Border();
        var messenger = new Messenger();
        var action = new RecordingAction();
        var trigger = new MessageTrigger { Messenger = messenger };
        trigger.Actions.Add(action);
        Interaction.GetTriggers(element).Add(trigger);

        // Act
        RaiseLoaded(element);
        RaiseUnloaded(element);
        RaiseLoaded(element);
        messenger.Send("test");

        // Assert
        Assert.Equal(1, action.Count);
    });

    [Fact]
    public void DoesNotReactWhileUnloaded() => RunSta(() =>
    {
        // Arrange
        var element = new Border();
        var messenger = new Messenger();
        var action = new RecordingAction();
        var trigger = new MessageTrigger { Messenger = messenger };
        trigger.Actions.Add(action);
        Interaction.GetTriggers(element).Add(trigger);

        // Act
        RaiseLoaded(element);
        RaiseUnloaded(element);
        messenger.Send("test");

        // Assert
        Assert.Equal(0, action.Count);
    });

    [Fact]
    public void ReactsWithCustomMessengerImplementation() => RunSta(() =>
    {
        // Arrange
        var element = new Border();
        var messenger = new CustomMessenger();
        var action = new RecordingAction();
        var trigger = new MessageTrigger { Messenger = messenger };
        trigger.Actions.Add(action);
        Interaction.GetTriggers(element).Add(trigger);

        // Act
        RaiseLoaded(element);
        messenger.Send("test");

        // Assert
        Assert.Equal(1, action.Count);
    });
}
