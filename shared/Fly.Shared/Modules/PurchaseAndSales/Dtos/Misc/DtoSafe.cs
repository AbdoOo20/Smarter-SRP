using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

public class DtoSafe : DtoBase
{
    public string Name { get; set; } = string.Empty;

    public PeymentMode PeymentMode { get; set; }

    public decimal LossPercent { get; set; }

    [ClientInvisible]
    public Identifier AccountId { get; set; }
    public long AccountNumber { get; set; }

    public string? Note { get; set; }

    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }
}
