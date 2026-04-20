using Fly.Framework.Shared.Common;

namespace Fly.Shared.Modules.Store.Requests;

public class ItemsTransferInvoiceCreateUpdateDetailRequest
{
    public Identifier UnitId { get; set; }
    
    public Identifier ItemId { get; set; }
    
    public int Quantity { get; set; }
    
    public Identifier ItemsTransferInvoiceId { get; set; }
}