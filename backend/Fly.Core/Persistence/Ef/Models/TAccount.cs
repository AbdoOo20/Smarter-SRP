using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Financial.Enums;

namespace Fly.Core.Persistence.Ef.Models;

[MapTo]
public class TAccount : TEntity
{
    public long Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal OpeningDebtor { get; set; }
    public decimal OpeningCreditor { get; set; }
    public Identifier? ParentId { get; set; }
    public TAccount? Parent { get; set; }
    public Identifier CurrencyId { get; set; }
    public AccountType Natural { get; set; }
    public AccountCat AccountCat { get; set; }
    public StatementAccountType StatementAccountType { get; set; }
}