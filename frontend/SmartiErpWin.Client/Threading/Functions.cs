namespace SmartiErpWin.Client.Threading;

public static class Functions
{
    public static Waiter Do(Action action)
    {
        return new Waiter(action);
    }
}