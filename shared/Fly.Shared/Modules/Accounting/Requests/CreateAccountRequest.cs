using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Accounting.Dtos;

namespace Fly.Shared.Modules.Accounting.Requests;

public class CreateAccountRequest : CreateUpdateRequestBase
{
    public long Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal OpeningDebtor { get; set; }
    public decimal OpeningCreditor { get; set; }
    public Identifier? ParentId { get; set; }
    public Identifier? CurrencyId { get; set; }
    public AccountType Natural { get; set; } // 0=debit, 1=vrefit
    public AccountCat AccountCat { get; set; }    // 0=debit, 1=vrefit
}