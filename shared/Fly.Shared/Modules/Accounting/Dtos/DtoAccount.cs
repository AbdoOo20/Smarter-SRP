using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Financial.Enums;

namespace Fly.Shared.Modules.Accounting.Dtos;

public class DtoCurrency : DtoBase
{
    public bool IsPrimary { get; set; }
    public decimal TransfereRate { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Letter { get; set; } = string.Empty;
}



public enum AccountType : int
{
    /// <summary>
    /// ����
    /// </summary>
    Debit = 0,
    /// <summary>
    /// ����
    /// </summary>
    Credit = 1,
}
public enum AccountCat : int
{
    Budget_Assets,
    Budget_Opponents,
    ProfitLoss_Income,
    ProfitLoss_Cost,
    Adjustment,
}

public class DtoAccount : DtoBase
{
    public long Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal OpeningDebtor { get; set; }
    public decimal OpeningCreditor { get; set; }
    [ClientInvisible]
    public Identifier? ParentId { get; set; }
    public string? ParentName { get; set; }
    [ClientInvisible]
    public Identifier CurrencyId { get; set; }
    public string CurrencyName { get; set; } = string.Empty;
    public AccountType Natural { get; set; }    // 0=debit, 1=vrefit
    public AccountCat AccountCat { get; set; }    // 0=debit, 1=vrefit

    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public decimal TotalBalance { get; set; }
    public StatementAccountType StatementAccountType { get; set; }
}