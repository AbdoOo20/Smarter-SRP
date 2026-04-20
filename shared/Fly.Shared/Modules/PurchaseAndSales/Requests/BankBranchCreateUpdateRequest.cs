using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public class BankBranchCreateUpdateRequest : CreateUpdateRequestBase
{
    public Identifier BankId { get; set; }
    public string? Note { get; set; }
    public string Name { get; set; } = string.Empty;

    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }

}
public class BankBranchDeleteRequest : DeleteRequestBase
{

}
public class BankBranchQueryRequest : QueryRequestBase
{

}


public class InventoryCostQueryRequest : QueryRequestBase
{
    public Identifier ItemId { get; set; }
    public Identifier UnitId { get; set; }
}