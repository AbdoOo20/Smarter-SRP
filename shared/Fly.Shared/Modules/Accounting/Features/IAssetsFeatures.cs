using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.Features;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Framework.Shared.Common;

namespace Fly.Shared.Modules.Accounting.Features;


public class AssetCreateUpdateCommand : CreateUpdateNamedRequestBase
{
    public decimal DepreciationRate { get; set; }
    public Identifier AccountId { get; set; }
}


public interface IAssetsFeatures : IFeatureBase<DtoAssets, AssetsQueryRequest, DeleteRequestBase, AssetCreateUpdateCommand>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts(AccountQueryRequest request);
}