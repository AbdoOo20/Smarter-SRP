using Fly.Framework.Shared.Modules.Identity.Dtos;

namespace Fly.Shared.Modules.Global.Dtos;

public class DtoRole : DtoRoleBase
{
    public string WindowName { get; set; } = string.Empty;
    public bool IsSecret { get; set; }
}