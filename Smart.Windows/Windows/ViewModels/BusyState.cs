namespace Smart.Windows.ViewModels;

using Smart.ComponentModel;

public class BusyState : NotificationObject, IBusyState
{
    private int counter;

    public bool IsBusy => counter > 0;

    public void Require()
    {
        var current = IsBusy;
        counter++;
        if (current != IsBusy)
        {
            RaisePropertyChanged(nameof(IsBusy));
        }
    }

    public void Release()
    {
        var current = IsBusy;
        counter--;
        if (current != IsBusy)
        {
            RaisePropertyChanged(nameof(IsBusy));
        }
    }

    public void Reset()
    {
        var current = IsBusy;
        counter = 0;
        if (current != IsBusy)
        {
            RaisePropertyChanged(nameof(IsBusy));
        }
    }
}
