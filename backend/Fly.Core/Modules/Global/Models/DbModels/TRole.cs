using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;

namespace Fly.Core.Modules.Global.Models.DbModels;

[MapTo]
public class TRole : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Windowname { get; set; } = string.Empty;
    public bool IsSecret { get; set; }

}