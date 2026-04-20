using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Personnel.Enums;

namespace Fly.Shared.Modules.Personnel.Dtos;

public class DtoDiscountIncrease : DtoBase
{

    [ClientInvisible]
    public Identifier EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public string Note { get; set; } = string.Empty;

    [ClientInvisible]
    public DiscountIncreaseMode Mode { get; set; }
    public string DiscountIncreaseModeString => Mode switch
    {
        DiscountIncreaseMode.Discount => "تخفيض",
        DiscountIncreaseMode.Increase => "زيادة",
        _ => ""
    };

    [ClientInvisible]
    public DateTime? Date { get; set; }
}