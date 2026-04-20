namespace Leapay.Client.Polymorphism.Interfaces;

public interface IRefreshable: IWrappable
{
    public void SetInfo(string title, string info = "");

    public void SetRefreshState(bool isRefreshing);

    public void RefreshState(Func<Task> action);

    void IWrappable.PostWrap()
    {
        SetRefreshState(false);
    }

    void IWrappable.PreWrap()
    {
        SetRefreshState(true);
    }
}