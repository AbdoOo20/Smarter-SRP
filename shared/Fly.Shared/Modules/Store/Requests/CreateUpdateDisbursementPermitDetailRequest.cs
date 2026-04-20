using Fly.Framework.Shared.Common;

namespace Fly.Shared.Modules.Store.Requests;

public class CreateUpdateDisbursementPermitDetailRequest
{
    public Identifier ItemId { get; set; }
    public Identifier UnitId { get; set; }

    public decimal Quantity { get; set; }

    //  TODO: Custom properties like expire date must be loaded dynamically!
    public DateTime ExpireDate { get; set; }
}
