using MediatR;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.C.Accounting.RequestHandlers.AccoutHandlers;

public class AssetsCreatedUpdatedCommandNotification : INotificationHandler<AssetsCreatedUpdatedCommand>
{
    private readonly ILogger<AssetsCreatedUpdatedCommandNotification> _logger;

    public AssetsCreatedUpdatedCommandNotification(ILogger<AssetsCreatedUpdatedCommandNotification> logger)
    {
        _logger = logger;
    }

    public async Task Handle(AssetsCreatedUpdatedCommand notification, CancellationToken cancellationToken)
    {
        await Task.Delay(1 * 1000 * 60);
        _logger.LogWarning("{0} Sent {1}", "Asset created", notification.IsNew ? "Creared" : "Updated");
    }
}