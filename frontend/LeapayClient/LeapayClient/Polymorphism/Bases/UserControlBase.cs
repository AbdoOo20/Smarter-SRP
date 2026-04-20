using Leapay.Client.Polymorphism.Interfaces;

namespace Leapay.Client.Polymorphism.Bases;

public class UserControlBase: UserControl, IDisposable
{
    public ISynchronisationAware? SynchronisationModel { get; set; }

    public void Dispose()
    {
        if (SynchronisationModel is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}