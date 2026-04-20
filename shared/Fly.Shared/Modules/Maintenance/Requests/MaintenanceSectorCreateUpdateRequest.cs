using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Maintenance.Requests;

public class MaintenanceSectorCreateUpdateRequest : CreateUpdateRequestBase
{
    public decimal Percentage { get; set; }
    
    public bool IsRepresentative { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public string Identifier { get; set; } = string.Empty;

    public Identifier AccountId { get; set; }
        
    public string Address { get; set; } = string.Empty;

    public decimal AccountOpeningDebtor { get; set; }
    
    public decimal AccountOpeningCreditor { get; set; }
}

public class MaintenanceSectorCreateRequest
{
        
}

public class MaintenanceSectorQueryRequest : QueryRequestBase
{ }

public class MaintenanceSectorLookupRequest : MaintenanceSectorQueryRequest
{ }

public class MaintenanceSectorDeleteRequest : DeleteRequestBase
{ }