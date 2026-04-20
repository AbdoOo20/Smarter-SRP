namespace Fly.Framework.Shared.Exceptions;

public class CallAdminException : Exception
{
    public CallAdminException(string errorNumber) : base( /*PagesLocalization.ResourceManager.GetString(nameof(CallAdminException)) +*/ $" [{errorNumber}]")
    {
    }

    public CallAdminException(string errorNumber, Exception exception)
        : base( /*PagesLocalization.ResourceManager.GetString(nameof(CallAdminException)) +*/ $" [{errorNumber}]", exception)
    {
    }
}