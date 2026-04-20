using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Global.Requests;

public class RoleGroupCreateUpdate : CreateUpdateRequestBase
{
    public string Name { get; set; } = string.Empty;
    public List<Identifier> Roles { get; set; }
}

public class RoleGroupDelete : DeleteRequestBase
{

}

public class RoleGroupQuery : QueryRequestBase
{

}