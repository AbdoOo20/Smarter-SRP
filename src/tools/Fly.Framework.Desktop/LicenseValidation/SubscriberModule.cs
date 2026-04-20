using Fly.Framework.Desktop.Toos;

namespace Fly.Framework.Desktop.LicenseValidation;

public class SubscriberModule : ISubscriberModule
{
    private readonly BasicSubscriber _basicSubscriber;

    public SubscriberModule(BasicSubscriber basicSubscriber)
    {
        this._basicSubscriber = basicSubscriber;
    }

    public bool SyncSubscription()
    {
        return _basicSubscriber.SyncSub();
    }
}
