using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

[MapTo]
public class TSafe : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; }

    public PeymentMode PeymentMode { get; set; }

    public decimal LossPercent { get; set; }

    public Identifier AccountId { get; set; }
    public TAccount? Account { get; set; }

    //public decimal OpeningDebtor { get; set; }
    //public decimal OpeningCreditor { get; set; }
}