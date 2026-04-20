using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public class ItemsSaleQueryRequest : QueryRequestBase
{
    public Identifier? StorageId { get; set; }
}

public class ItemsSaleDetailsQueryResponse: ObservableObject
{
    public Identifier? ItemId { get; set; }
    
    public string? ItemName { get; set; }

    public string? ItemBarcode { get; set; }
    
    public decimal ItemRegionalPrice { get; set; }
    
    public decimal ItemBulkPrice { get; set; }

    public decimal ItemCustomPrice { get; set; }
    
    public double ItemUpdateCounter { get; set; }

    public List<UnitSaleDetailsQueryResponse> Units { get; set; } = [];
}

public class UnitSaleDetailsQueryResponse
{
    public Identifier? UnitId { get; set; }
    
    public string? UnitName { get; set; }

    public decimal QuantityInStorage { get; set; }
    
    public bool IsPrimary { get; set; }
}

public class ItemsSaleQueryResponse
{
    public List<ItemsSaleDetailsQueryResponse> Items { get; set; } = [];
}