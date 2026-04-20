using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo]
public class TStore : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}