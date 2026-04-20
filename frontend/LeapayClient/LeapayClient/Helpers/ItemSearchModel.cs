using Fly.Framework.Shared.Common;

namespace Leapay.Client.Helpers;

public class ItemSearchModel
{
    public string? ItemName { get; set; }
        
    public string? ItemPrice { get; set; }

    public Identifier ItemId { get; set; }
    
    public Identifier? UnitId { get; set; }

    public string? PhotoLink { get; set; }

    public string? Barcode { get; set; }
    
    public List<string?>? OtherBarcodes { get; set; }
    
    public List<Tuple<Identifier, string>>? ItemUnitBarcodes { get; set; }

    public override string ToString()
    {
        return ItemName ?? "";
    }
}