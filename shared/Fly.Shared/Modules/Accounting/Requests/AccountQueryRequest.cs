using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using MediatR;

namespace Fly.Shared.Modules.Accounting.Requests;


public class AccountQueryRequest : QueryRequestBase/*, IRequest<Dtos.DtoAssets>*/
{
    public Identifier? ChildrenOf { get; set; }
}
public class GetAccountBalanceResponse
{
    public decimal Balance { get; set; }
}

public class GetAccountBalance : IRequest<GetAccountBalanceResponse>
{
    public Identifier AccountId { get; set; }
}