using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;

namespace Fly.Core.Persistence.Ef.Models.Personnel;

[MapTo]
public class TDepartment : TEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}