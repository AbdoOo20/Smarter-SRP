using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Personnel.Enums;

namespace Fly.Shared.Modules.Personnel.Requests;

public class CreateUpdateAttendanceDepartureRequest : CreateUpdateRequestBase
{
    public Identifier EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public string Note { get; set; } = string.Empty;
    public AttendanceDepartureMode Mode { get; set; }
    public DateTime? Date { get; set; }
}