using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Personnel.Enums;

namespace Fly.Shared.Modules.Personnel.Dtos;

public class DtoEmployeeAssignment : DtoBase
{

    public string EmployeeName { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier EmployeeAccountId { get; set; } 

    [ClientInvisible]
    public Identifier EmployeeId { get; set; }

    public decimal Amount { get; set; }

    [ClientInvisible]
    public AssignmentsEnum AssignmentsType { get; set; }

    public string AssignmentsTypeString => AssignmentsType switch
    {
        AssignmentsEnum.FullTime => "دوام كامل",
        AssignmentsEnum.SpecificWork => "عمل محدد",
    };

    [ClientInvisible]
    public AssignmentsMoneyEnum AssignmentsMoney { get; set; }
    public string AssignmentsMoneyString => AssignmentsMoney switch
    {
        AssignmentsMoneyEnum.Period => "فترة",
        AssignmentsMoneyEnum.Cutoff => "قطع",
    };

    [ClientInvisible]
    public int? PeriodTime { get; set; }
    [ClientInvisible]
    public DateTime? EndDate { get; set; }
    [ClientInvisible]
    public DateTime? StartDate { get; set; }
    [ClientInvisible]
    public string Note { get; set; } = string.Empty;
}