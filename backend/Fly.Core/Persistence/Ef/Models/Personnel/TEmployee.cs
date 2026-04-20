using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Persistence.Ef.Models.Personnel;

[MapTo]
public class TEmployee : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;

    public TAccount Account { get; set; }
    public Identifier AccountId { get; set; }
    public TDepartment Department { get; set; } 
    public Identifier DepartmentId { get; set; }

    //public decimal AccountOpeningDebtor { get; set; }
    //public decimal AccountOpeningCreditor { get; set; }
}