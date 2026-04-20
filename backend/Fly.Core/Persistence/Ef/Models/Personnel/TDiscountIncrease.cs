using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Personnel.Enums;

namespace Fly.Core.Persistence.Ef.Models.Personnel;

[MapTo]
public class TDiscountIncrease : TEntity
{
    public decimal Amount { get; set; }
    public string Note { get; set; } = string.Empty;
    public TEmployee Employee { get; set; }
    public Identifier EmployeeId { get; set; }
    public DiscountIncreaseMode Mode { get; set; }
    public DateTime? Date { get; set; }
}