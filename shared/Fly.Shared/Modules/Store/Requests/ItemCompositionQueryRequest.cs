using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public class ItemCompositionQueryRequest : QueryRequestBase
{
    public Identifier? ItemCompositionId { get; set; }
}