using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.Marketers.Models.DbModels;

[MapTo]
public class TMarketerDisbursementPermit : TEntity
{
    public DateTime Date { get; set; }
        
    public decimal TotalCost { get; set; }
        
    public long Number { get; set; }
        
    public long SerialNumber { get; set; }

    public Identifier MarketerId { get; set; }
    public TMarketer? Marketer { get; set; }

    public Identifier StoreId { get; set; }
    public TStore? Store { get; set; }
        
    public virtual ICollection<TMarketerDisbursementPermitDetail>? Details { get; set; }

    public string? Notes { get; set; }
}