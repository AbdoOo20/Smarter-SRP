using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Common.Markers;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo]
public class TDisbursementPermit : TEntity, INumberEntity
{

    public Identifier CostCenterId { get; set; }
    public TCostCenter? CostCenter { get; set; }
    public Identifier StoreId { get; set; }
    public TStore? Store { get; set; }
    public DateTime? Date { get; set; }

    public virtual ICollection<TDisbursementPermitDetail>? DisbursementPermitDetails { get; set; }
    public long Number { get; set; }
}