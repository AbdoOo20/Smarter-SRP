using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public class CreateUpdateSupplyPermitRequest : CreateUpdateRequestBase
{
    public Identifier SupplierId { get; set; }
    public Identifier StoreId { get; set; }
    public DateTime Date { get; set; }

    public IEnumerable<CreateUpdateSupplyPermitDetailRequest>? SupplyPermitDetails { get; set; }
}