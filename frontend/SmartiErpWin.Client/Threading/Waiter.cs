using MahApps.Metro.Controls;

namespace SmartiErpWin.Client.Threading;

public class Waiter(Action action)
{
    public void After(double seconds)
    { 
        Task.Delay(Convert.ToInt32(seconds * 1000))
            .ContinueWith(_ => Application.Current.Invoke(action));
    }
}