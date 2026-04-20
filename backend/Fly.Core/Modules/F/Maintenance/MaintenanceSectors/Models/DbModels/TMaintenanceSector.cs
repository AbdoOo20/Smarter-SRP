using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.Maintenance.MaintenanceSectors.Models.DbModels;

[MapTo]
public class TMaintenanceSector: TEntity
{
    public decimal Percentage { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    
    public string Identifier { get; set; } = string.Empty;

    public TAccount? Account { get; set; }
    
    public Identifier AccountId { get; set; }
    
    public decimal OpeningDebtor { get; set; }
    
    public decimal OpeningCreditor { get; set; }
}