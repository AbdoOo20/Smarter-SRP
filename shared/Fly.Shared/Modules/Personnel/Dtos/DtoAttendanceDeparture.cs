using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Personnel.Enums;

namespace Fly.Shared.Modules.Personnel.Dtos;

public class DtoAttendanceDeparture : DtoBase
{
    [ClientInvisible]
    public Identifier EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Note { get; set; } = string.Empty;
    public AttendanceDepartureMode Mode { get; set; }
    public DateTime? Date { get; set; }
}