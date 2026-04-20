using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Store.Enums;

namespace Fly.Shared.Modules.Store.Requests;

public class ItemsTransferInvoiceCreateUpdateRequest : CreateUpdateRequestBase
{
    public long InvoiceNumber { get; set; }
    
    public Identifier FromStorageId { get; set; }
    
    public string? FromStorageName { get; set; }

    public Identifier ToStorageId { get; set; }
    
    public string? ToStorageName { get; set; }
    
    public ItemsTransferRequestStatus TransferStatus { get; set; }
    
    public IEnumerable<ItemsTransferInvoiceCreateUpdateDetailRequest> Details { get; set; } = [];
}

public class ItemsTransferInvoiceDeleteRequest : DeleteRequestBase
{

}