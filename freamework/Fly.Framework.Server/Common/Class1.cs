using Microsoft.Extensions.Logging;

namespace Fly.Framework.Server.Common;

public class CustomLoggingService
{
    private readonly ILogger _logger;

    public CustomLoggingService(ILogger logger)
    {
        _logger = logger;
    }

    public void TraceGettingServices(params Type[] types)
    {
        var names = types.Any() ? string.Join(", ", types.Select(s => s.Name)) : "";
        _logger.LogTrace("Getting services [{0}]", names);
    }
}