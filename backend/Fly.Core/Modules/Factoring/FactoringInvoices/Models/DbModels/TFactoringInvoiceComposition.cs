using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;

[MapTo]
public class TFactoringInvoiceComposition : TEntity
{
    public Identifier FactoringInvoiceId { get; set; }
    
    public TFactoringInvoice? FactoringInvoice { get; set; }
    
    public Identifier ItemCompositionId { get; set; }
    
    public Identifier CompositionDetailBindingId { get; set; }

    public TItemComposition? ItemComposition { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal Discount { get; set; }
    
    public int CompositionIndex { get; set; }
    
    
}