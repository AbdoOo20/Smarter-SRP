using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Accounting.Dtos;
using Mapster;
using MediatR;

namespace Fly.Core.Modules.C.Accounting.RequestHandlers.AccoutHandlers;

//
//
//
//  
//  
//  
//  
public class CreateCommandHandler : IRequestHandler<ReqAssetsCreateUpdateCommand, DtoAssets>, IDisposable
{
    //
    //
    private readonly IMediator _mediator;
    private readonly IServiceBase<DtoAssets, TAssets> _assetsService;
    //
    public CreateCommandHandler(IServiceBase<DtoAssets, TAssets> assetsService, IMediator mediator)
    {
        _mediator = mediator;
        _assetsService = assetsService;
    }
    //
    public void Dispose()
    {
        _assetsService.Added -= AssestAdded;
    }
    //
    public async Task<DtoAssets> Handle(ReqAssetsCreateUpdateCommand request, CancellationToken cancellationToken)
    {
        _assetsService.Added += AssestAdded;
        var dto = request.Adapt<DtoAssets>();
        await _assetsService.Add(dto);
        return dto;
    }
    //
    private void AssestAdded(object? sender, DtoAssets e)
    {
        _mediator.Publish(new AssetsCreatedUpdatedCommand(true));
    }
    //
    //
}