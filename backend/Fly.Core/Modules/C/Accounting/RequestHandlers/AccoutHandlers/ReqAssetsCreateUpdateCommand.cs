using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Accounting.Dtos;
using MediatR;

namespace Fly.Core.Modules.C.Accounting.RequestHandlers.AccoutHandlers;

public class ReqAssetsCreateUpdateCommand : CreateUpdateNamedRequestBase, IRequest<DtoAssets>
{

}