using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public class CreateUpdateItemCompositionRequest : CreateUpdateRequestBase
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public Identifier CategoryId { get; set; }
    
    public IEnumerable<CreateUpdateItemCompositionDetailRequest> Details { get; set; } = [];
}

public class DeleteItemCompositionRequest : DeleteRequestBase
{

}