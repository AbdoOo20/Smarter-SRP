using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public class SafeCreateUpdateRequest : CreateUpdateRequestBase
{
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; } 

    public PeymentMode PeymentMode { get; set; }

    public decimal LossPercent { get; set; }

    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }

}
public class SafeDeleteRequest : DeleteRequestBase
{

}
public class SafeQueryRequest : QueryRequestBase
{
    public PeymentMode? PeymentMode { get; set; } = null;
}
public class GetSafesRequest : SafeQueryRequest
{
}