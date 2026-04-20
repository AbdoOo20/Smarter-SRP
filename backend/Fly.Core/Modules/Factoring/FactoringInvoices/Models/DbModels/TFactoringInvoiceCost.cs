using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;

[MapTo]
public class TFactoringInvoiceCost : TEntity
{
    public Identifier FactoringInvoiceId { get; set; }
    
    public TFactoringInvoice? FactoringInvoice { get; set; }
    
    public decimal Price { get; set; }

    public string CostName { get; set; } = string.Empty;
    
    public decimal Discount { get; set; }
    
    public string? Notes { get; set; }
}