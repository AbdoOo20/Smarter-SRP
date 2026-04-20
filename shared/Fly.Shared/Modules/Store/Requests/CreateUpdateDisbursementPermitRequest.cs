using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public class CreateDisbursementPermitRequest : CreateUpdateRequestBase
{
    public Identifier CostCenterId { get; set; }
    public Identifier StoreId { get; set; }
    public DateTime? Date { get; set; }

    public IEnumerable<CreateUpdateDisbursementPermitDetailRequest>? DisbursementPermitDetails { get; set; }
}

public class UpdateDisbursementPermitRequest : UpdateRequestBase
{
    public Identifier CostCenterId { get; set; }
    public Identifier StoreId { get; set; }
    public DateTime? Date { get; set; }

    public IEnumerable<CreateUpdateDisbursementPermitDetailRequest>? DisbursementPermitDetails { get; set; }
}