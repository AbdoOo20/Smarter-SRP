using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Store.Requests;

public class CreateUpdateItemBarcodeRequest: IDetailsModel
{
    public string? ItemTypeName { get; set; }

    public string? Barcode { get; set; }
    
    public string? Notes { get; set; }

    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}