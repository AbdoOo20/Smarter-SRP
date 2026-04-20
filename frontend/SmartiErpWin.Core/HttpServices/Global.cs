using SmartErpWin.Core.Exceptions;

namespace SmartErpWin.Core.HttpServices;

public static class Global
{
    public static event EventHandler<LoggedExceptionEventArgs>? OnHttpError;
    
    public static void RaiseHttpError(LoggedException exception)
    {
        throw exception;
    }
}