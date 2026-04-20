using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Factoring.Dtos;

public class DtoFactoringInvoiceCost : DtoBase, IDetailsModel
{
    public decimal Price { get; set; }

    public string CostName { get; set; } = string.Empty;
    
    [ClientInvisible]
    public decimal Discount { get; set; }
    
    public Identifier FactoringInvoiceId { get; set; }

    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    public string? Notes { get; set; }
}