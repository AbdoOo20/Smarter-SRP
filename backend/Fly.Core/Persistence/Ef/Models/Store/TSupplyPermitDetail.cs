using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo]
public class TSupplyPermitDetail : TEntity
{

    public Identifier SupplyPermitId { get; set; }
    public TSupplyPermit SupplyPermit { get; set; }
    public Identifier ItemId { get; set; }
    public TItem? Item { get; set; }
    public Identifier UnitId { get; set; }
    public TUnit? Unit { get; set; }
    public decimal Quantity { get; set; }
    public DateTime ExpireDate { get; set; }
}