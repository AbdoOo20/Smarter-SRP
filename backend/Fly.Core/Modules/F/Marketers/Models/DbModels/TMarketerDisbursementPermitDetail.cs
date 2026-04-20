using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.Marketers.Models.DbModels;

[MapTo]
public class TMarketerDisbursementPermitDetail : TEntity
{

    public Identifier MarketerDisbursementPermitId { get; set; }
    public TMarketerDisbursementPermit? MarketerDisbursementPermit { get; set; }
    public Identifier ItemId { get; set; }
    public TItem? Item { get; set; }
    public Identifier UnitId { get; set; }
    public TUnit? Unit { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpireDate { get; set; }
}