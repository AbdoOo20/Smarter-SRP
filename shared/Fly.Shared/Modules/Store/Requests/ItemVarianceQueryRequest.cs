using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public class ItemVarianceQueryRequest : QueryRequestBase
{
    public Identifier? ItemId { get; set; }
}