using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Factoring.Requests;

public class ManufacturingFacilityCreateUpdateRequest : CreateUpdateRequestBase
{
    public string Name { get; set; } = string.Empty;
        
    public string Phone { get; set; } = string.Empty;
        
    public string Email { get; set; } = string.Empty;
        
    public string Identifier { get; set; } = string.Empty;

    public Identifier AccountId { get; set; }
        
    public string Address { get; set; } = string.Empty;

    public decimal AccountOpeningDebtor { get; set; }
        
    public decimal AccountOpeningCreditor { get; set; }
}

public class ManufacturingFacilityCreateRequest
{
        
}

public class ManufacturingFacilityQueryRequest : QueryRequestBase
{ }

public class ManufacturingFacilityLookupRequest : ManufacturingFacilityQueryRequest
{ }

public class ManufacturingFacilityDeleteRequest : DeleteRequestBase
{ }