using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Maintenance.Dtos;

public class DtoMaintenanceSector : DtoBase
{
    public decimal Percentage { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    
    public string Identifier { get; set; } = string.Empty;
    
    public Identifier AccountId { get; set; }
    
    public long AccountNumber { get; set; }

    public decimal OpeningDebtor { get; set; }
    
    public decimal OpeningCreditor { get; set; }
}