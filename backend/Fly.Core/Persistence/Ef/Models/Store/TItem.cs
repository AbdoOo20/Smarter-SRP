using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo]
public class TItem : TEntity
{

    public Identifier CategoryId { get; set; }
    public TCategory Category { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public string? Barcode { get; set; }
    public DateTime? ExpireDate { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }

    public byte[] Image { get; set; } = [];
    
    public bool IsActive { get; set; }

    public bool Expirable { get; set; }

    public decimal BulkPrice { get; set; }
    
    public decimal RegionalPrice { get; set; }
    
    public decimal MinPrice { get; set; }
    
    public double UpdateCounter { get; set; }

    public virtual ICollection<TUnit> Units { get; set; } = [];
    public virtual ICollection<TItemUnit> ItemUnits { get; set; } = [];
    
    public virtual ICollection<TItemBarcode> ItemBarcodes { get; set; } = [];
}