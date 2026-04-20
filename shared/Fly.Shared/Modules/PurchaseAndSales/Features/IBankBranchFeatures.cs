using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Features;

public interface IBankBranchFeatures : IFeatureBase<DtoBankBranch, BankBranchQueryRequest, BankBranchDeleteRequest, BankBranchCreateUpdateRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupBanks(BankQueryRequest request);
}