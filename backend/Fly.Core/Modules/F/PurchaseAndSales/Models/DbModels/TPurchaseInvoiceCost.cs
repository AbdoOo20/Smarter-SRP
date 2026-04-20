using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

[MapTo]
public class TPurchaseInvoiceCost : TEntity
{
    public Identifier PurchaseInvoiceId { get; set; }
    public TPurchaseInvoice? PurchaseInvoice { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public decimal Ammount { get; set; }
}