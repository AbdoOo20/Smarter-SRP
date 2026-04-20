using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoDisbursementPermitDetail : DtoBase, IDetailsModel
{
    [ClientInvisible]
    public Identifier DisbursementPermitId { get; set; }
    [ClientInvisible]
    public Identifier ItemId { get; set; }
    [ClientInvisible]
    public Identifier UnitId { get; set; }

    public string UnitName { get; set; }
    public string ItemName { get; set; }
    public decimal Quantity { get; set; }
    
    public DateTime ExpireDate { get; set; } = DateTime.Now;
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}