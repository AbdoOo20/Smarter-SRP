using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo(typeof(DtoItemUnit))]
public class TItemUnit : TEntity
{
    public TUnit? Unit { get; set; }
    public Identifier UnitId { get; set; }
    public TItem? Item { get; set; }
    public Identifier ItemId { get; set; }
    public bool IsPrimary { get; set; }
    public decimal TransferRate { get; set; }
    public string? Barcode { get; set; }
}