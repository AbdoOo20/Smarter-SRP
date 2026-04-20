using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Common.Markers;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo]
public class TSupplyPermit : TEntity, INumberEntity
{

    public Identifier SupplierId { get; set; }
    public TSupplier? Supplier { get; set; }
    public Identifier StoreId { get; set; }
    public TStore Store { get; set; }
    public DateTime Date { get; set; }
    public long Number { get; set; }

    public virtual ICollection<TSupplyPermitDetail>? SupplyPermitDetails { get; set; }

}