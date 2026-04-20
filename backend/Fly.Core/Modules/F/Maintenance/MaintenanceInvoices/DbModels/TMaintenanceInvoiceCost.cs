using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;

[MapTo]
public class TMaintenanceInvoiceCost : TEntity
{
    public Identifier MaintenanceInvoiceId { get; set; }
    
    public TMaintenanceInvoice? MaintenanceInvoice { get; set; }
    
    public decimal Price { get; set; }

    public string CostName { get; set; } = string.Empty;
    
    public decimal Discount { get; set; }
    
    public string? Notes { get; set; }
}