using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Store.Dtos;

public partial class DtoItemsTransferInvoiceDetail : DtoBase, IDetailsModel
{
    public string? ItemName { get; set; }
    
    public Identifier ItemId { get; set; }
    
    public string? UnitName { get; set; }
    
    public Identifier UnitId { get; set; }
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    public Identifier ItemsTransferInvoiceId { get; set; }

    public int Quantity { get; set; } = 1;
}