using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Enums;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo]
public class TItemsTransferInvoice : TEntity
{
    public long InvoiceNumber { get; set; }
    
    public TStore FromStorage { get; set; }
    public Identifier FromStorageId { get; set; }

    public TStore ToStorage { get; set; }
    public Identifier ToStorageId { get; set; }
    
    public ItemsTransferRequestStatus TransferStatus { get; set; }

    public virtual ICollection<TItemsTransferInvoiceDetail> Details { get; set; } = [];
}