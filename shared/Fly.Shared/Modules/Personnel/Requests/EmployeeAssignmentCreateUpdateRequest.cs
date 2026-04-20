using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Personnel.Enums;

namespace Fly.Shared.Modules.Personnel.Requests;

public class EmployeeAssignmentCreateUpdateRequest : CreateUpdateRequestBase
{
    public Identifier EmployeeId { get; set; }
    public int? PeriodTime { get; set; }
    public string Note { get; set; } = string.Empty;
    public AssignmentsEnum AssignmentsType { get; set; }
    public AssignmentsMoneyEnum AssignmentsMoney { get; set; }
    public decimal Amount { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}