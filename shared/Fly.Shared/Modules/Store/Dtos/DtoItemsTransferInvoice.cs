using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Store.Enums;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoItemsTransferInvoice : DtoNamesBase
{
    public long InvoiceNumber { get; set; }
    
    public Identifier FromStorageId { get; set; }
    
    public string? FromStorageName { get; set; }

    public Identifier ToStorageId { get; set; }
    
    public string? ToStorageName { get; set; }
    
    public ItemsTransferRequestStatus TransferStatus { get; set; }

    [ClientInvisible]
    public IEnumerable<DtoItemsTransferInvoiceDetail> Details { get; set; } = [];
    
    public string DetailsString
    {
        get => string.Join(", ", Details.Select(x => x.ItemName));
        set { } // This setter is just to avoid serialization issues
    }
}