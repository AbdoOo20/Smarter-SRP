using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Marketers.Requests;

public class MarketerCreateUpdateRequest : CreateUpdateRequestBase
{
    public decimal Percentage { get; set; }
        
    public bool IsRepresentative { get; set; }
        
    public string Name { get; set; } = string.Empty;
        
    public string Phone { get; set; } = string.Empty;
        
    public string Email { get; set; } = string.Empty;
        
    public string Identifier { get; set; } = string.Empty;
        
    public Identifier AccountId { get; set; }

    public decimal AccountOpeningDebtor { get; set; }
        
    public decimal AccountOpeningCreditor { get; set; }

    public ICollection<MarketerClientCreateRequest> MarketerClients { get; set; } = [];

    public decimal Salary { get; set; }
}

public class MarketerClientCreateRequest
{
    public Identifier ClientId { get; set; }
}

public class MarketerQueryRequest : QueryRequestBase
{ }

public class MarketerLookupRequest : MarketerQueryRequest
{ }

public class MarketerDeleteRequest : DeleteRequestBase
{ }