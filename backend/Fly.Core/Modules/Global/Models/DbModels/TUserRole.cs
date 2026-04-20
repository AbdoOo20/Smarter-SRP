using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.Global.Models.DbModels;

[MapTo]
public class TUserRole : TEntity
{

    public TUser? User { get; set; }
    public Identifier UserId { get; set; }

    public TRole? Role { get; set; }
    public Identifier RoleId { get; set; }

}