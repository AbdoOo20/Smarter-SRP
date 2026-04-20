using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;

namespace Fly.Core.Modules.Global.Models.DbModels;

[MapTo]
public class TRoleGroup : TEntity
{
    public string Name { get; set; } = string.Empty;
}