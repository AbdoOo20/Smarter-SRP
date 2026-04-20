using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public class DamagedInvoiceCreateUpdate : CreateUpdateRequestBase
{
    public long Number { get; set; }
    public DateTime Date { get; set; }
    public long SerialNumber { get; set; }
    public Identifier StoreId { get; set; }
    public Identifier? MarketerStoreId { get; set; }

    public virtual ICollection<DamagedInvoiceDetailCreateUpdate> Details { get; set; } = [];

    public string? Notes { get; set; }
}
public class DamagedInvoiceDetailCreateUpdate : CreateUpdateRequestBase
{
    public Identifier DamagedInvoiceId { get; set; }
    public Identifier UnitId { get; set; }
    public Identifier ItemId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime? ExpireDate { get; set; }
}

public class DamagedInvoiceDelete : DeleteRequestBase
{ }
public class DamagedInvoiceQueryRequest : QueryRequestBase
{ }