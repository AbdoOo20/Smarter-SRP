using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;

[MapTo]
public class TMaintenanceInvoiceDetail : TEntity
{
    public Identifier MaintenanceInvoiceId { get; set; }
    
    public TMaintenanceInvoice? MaintenanceInvoice { get; set; }
    
    public Identifier ItemId { get; set; }
    
    public TItem? Item { get; set; }
    
    public TUnit? Unit { get; set; }
    
    public Identifier UnitId { get; set; }

    public TItemVariance? ItemVariance { get; set; }

    public Identifier? ItemVarianceId { get; set; }

    public string? UnitName { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal Discount { get; set; }
}