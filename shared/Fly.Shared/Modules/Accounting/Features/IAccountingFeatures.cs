using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;

namespace Fly.Shared.Modules.Accounting.Features;

public interface IAccountingFeatures
//  : IFeatureBase<DtoAccount>
    : IFeatureBase<DtoAccount, AccountQueryRequest, DeleteRequestBase, CreateAccountRequest>
{
    ValueTask<HttpResponseWrapperList<DtoAccount>> GetAccountTreeAsync(Guid parentId);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts(AccountQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupCurrencies(CurrencyQueryRequest request);
    ValueTask<HttpResponseWrapper<GetAccountBalanceResponse>> GetAccountBalanc(GetAccountBalance request);
}