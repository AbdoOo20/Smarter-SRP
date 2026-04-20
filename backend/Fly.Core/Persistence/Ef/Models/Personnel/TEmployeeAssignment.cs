using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Personnel.Enums;

namespace Fly.Core.Persistence.Ef.Models.Personnel;

[MapTo]
public class TEmployeeAssignment : TEntity
{
    public decimal Amount { get; set; }
    public int? PeriodTime { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public string Note { get; set; } = string.Empty;
    public AssignmentsEnum AssignmentsType { get; set; }
    public TEmployee Employee { get; set; }
    public Identifier EmployeeId{ get; set; }
    public AssignmentsMoneyEnum AssignmentsMoney { get; set; }
}