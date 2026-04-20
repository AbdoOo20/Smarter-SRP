using System.Diagnostics.CodeAnalysis;

namespace SmartErpWin.Core.Exceptions;

public class LoggedException(string message, int code) : Exception(message)
{
    public int ErrorCode { get; } = code;

    public Exception? SourceException { get; private set; }
    
    public LoggedException(string message, int code, Exception sourceException) : this(message, code)
    {
        SourceException = sourceException;
    }
    
    public static void ThrowIfNull([NotNull] object? obj, string message, int code)
    {
        if (obj is not null)
            return;

        throw new LoggedException(message, code);
    }
    
    public static void ThrowIfNotNull(object? obj, string message, int code)
    {
        if (obj is not null)
            throw new LoggedException(message, code);
    }
}