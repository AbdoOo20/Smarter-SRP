using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Personnel.Dtos;

public class DtoEmployeeSalaryCountingPermit : DtoBase
{
    public int Number { get; set; }
    public decimal Total { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public DateTime WorkTime { get; set; }
    public int WorkMounth { get; set; }
}
public class DtoEmployeeSalaryCounting : DtoBase
{
    public Identifier EmployeeSalaryCountingPermitId { get; set; }
    public decimal Total { get; set; }
    public decimal Salaty { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
}

public class DtoEmployee : DtoBase
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    [ClientInvisible]
    public string Identifier { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    [ClientInvisible]
    public Identifier DepartmentId { get; set; }

    [ClientInvisible]
    public Identifier AccountId { get; set; }
    public long AccountNumber { get; set; }

    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }
}