using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Maintenance.Dtos;

public class DtoMaintenanceInvoiceCost : DtoBase, IDetailsModel
{
    public decimal Price { get; set; }

    public string CostName { get; set; } = string.Empty;
    
    public decimal Discount { get; set; }
    
    public Identifier MaintenanceInvoiceId { get; set; }

    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    public string? Notes { get; set; }
}