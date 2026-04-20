using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Persistence.Ef.Models;

[MapTo]
public class TAssets : TEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal DepreciationRate { get; set; }

    public Identifier AccountId { get; set; }
    public TAccount? Account { get; set; }
}