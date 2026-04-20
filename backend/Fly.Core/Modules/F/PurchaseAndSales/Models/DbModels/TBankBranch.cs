using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

[MapTo]
public class TBankBranch : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; }

    public Identifier BankId { get; set; }
    public TBank? Bank { get; set; }

    public Identifier AccountId { get; set; }
    public TAccount? Account { get; set; }

    //public decimal OpeningDebtor { get; set; }
    //public decimal OpeningCreditor { get; set; }
}