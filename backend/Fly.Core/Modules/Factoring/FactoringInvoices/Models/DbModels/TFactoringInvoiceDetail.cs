using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;

[MapTo]
public class TFactoringInvoiceDetail : TEntity
{
    public Identifier FactoringInvoiceId { get; set; }
    
    public TFactoringInvoice? FactoringInvoice { get; set; }
    
    public Identifier ItemId { get; set; }

    public TItem? Item { get; set; }
    
    public TUnit? Unit { get; set; }
    
    public Identifier UnitId { get; set; }

    public string? UnitName { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal Discount { get; set; }
    
    public Identifier? SourceCompositionId { get; set; }

    public string? SourceCompositionName { get; set; }

    public int SourceCompositionIndex { get; set; } = 0;
    
    public Identifier CompositionDetailBindingId { get; set; }
}