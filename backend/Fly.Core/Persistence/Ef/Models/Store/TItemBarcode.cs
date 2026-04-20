using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo(typeof(DtoItemBarcode))]
public class TItemBarcode : TEntity
{
    public TItem? Item { get; set; }
    
    public Identifier ItemId { get; set; }
    
    public string? ItemTypeName { get; set; }

    public string? Barcode { get; set; }
    
    public string? Notes { get; set; }
}