using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public  class UnitQueryRequest : QueryRequestBase
{
    public Guid? ItemId { get; set; }
}