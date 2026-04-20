using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Store.Requests;

public class CreateUpdateItemUnitRequest: IDetailsModel
{
    public Identifier UnitId { get; set; }
    public string UnitName { get; set; }
    public bool IsPrimary { get; set; }
    public decimal TransferRate { get; set; } = 1;
    [ClientInvisible]
    public decimal? CurrentQuantity { get; set; }
    public Identifier? TemporaryId { get; set; }
    
    public string? Barcode { get; set; }
}