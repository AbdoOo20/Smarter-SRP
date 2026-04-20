using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public class BankCreateUpdateRequest : CreateUpdateRequestBase
{
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; }
}

public class BankDeleteRequest : DeleteRequestBase
{

}

public class BankQueryRequest : QueryRequestBase
{

}