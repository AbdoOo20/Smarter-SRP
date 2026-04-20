using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

[MapTo]
public class TClient : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;

    public Identifier AccountId { get; set; }
    public TAccount? Account { get; set; }

    //public decimal OpeningDebtor { get; set; }
    //public decimal OpeningCreditor { get; set; }


    public decimal MaxDebt { get; set; }
    public string? Address { get; set; }
}