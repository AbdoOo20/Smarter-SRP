using Fly.Framework.Shared.Common;

namespace Fly.Shared.Modules.Store.Requests;

public class ItemQuntityQueryRequest
{
    public bool CollecttTheQuntitiesBeforTime { get; set; }

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Identifier ItemId { get; set; }
    public Identifier UnitVemf { get; set; }
    public Identifier StoreId { get; set; }

}