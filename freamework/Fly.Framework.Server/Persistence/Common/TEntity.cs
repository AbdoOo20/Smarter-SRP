using Fly.Framework.Shared.Common;

namespace Fly.Framework.Server.Persistence.Common;

public class TEntity
{
    public Guid Guid { get; set; }
    public DateTime CreateDate { get; set; }
    

    public Identifier Id { get; set; }
    public bool IsDeleted { get; set; }
    public Identifier TenantId { get; set; }
}
