using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;

namespace Fly.Core.Persistence.Ef.Models;

[MapTo]
public class TCurrency : TEntity
{
    public bool IsPrimary { get; set; }
    public decimal TransfereRate { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Letter { get; set; } = string.Empty;
}
