using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo]
public class TItemComposition : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Identifier CategoryId { get; set; }
    public TCategory Category { get; set; }
    
    public int CompositionIndex { get; set; } = 1;
    
    public virtual ICollection<TItemCompositionDetail> Details { get; set; } = [];
}