using MediatR;

namespace Fly.Core.Modules.C.Accounting.RequestHandlers.AccoutHandlers;

public record AssetsCreatedUpdatedCommand(bool IsNew) : INotification;