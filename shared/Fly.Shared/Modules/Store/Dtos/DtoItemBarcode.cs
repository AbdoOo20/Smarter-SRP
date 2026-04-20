using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoItemBarcode : DtoBase, IDetailsModel
{
    public string? ItemTypeName { get; set; }

    public string? Barcode { get; set; }
    
    public string? Notes { get; set; }
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    [ClientInvisible]
    public string? ItemName { get; set; }
    
    [ClientInvisible]
    public Identifier ItemId { get; set; }
}