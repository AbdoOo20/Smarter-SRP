using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public partial class CreateUpdateItemRequest : CreateUpdateRequestBase
{
    public Identifier CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public string? Barcode { get; set; }
    public DateTime? ExpireDate { get; set; }
    public decimal Price { get; set; }

    [ObservableProperty]
    public partial byte[] Image { get; set; } = [];
    
    public bool IsActive { get; set; }

    public bool Expirable { get; set; }

    public decimal BulkPrice { get; set; }
    
    public decimal RegionalPrice { get; set; }
    
    public decimal MinPrice { get; set; }
    
    public double UpdateCounter { get; set; }
    
    public IEnumerable<CreateUpdateItemUnitRequest> ItemUnits { get; set; } = [];
    
    public virtual IEnumerable<CreateUpdateItemBarcodeRequest> ItemBarcodes { get; set; } = [];
}

public class DeleteItemRequest : DeleteRequestBase
{

}