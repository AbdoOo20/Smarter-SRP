using Fly.Shared.Modules.Global.Dtos;

namespace Fly.Core.Modules.Global.Seeded;

public static class GroupSeed
{
    public static List<DtoRoleGroup> Groups { get; set; }

    public static void SetSecreit()
    {
        Groups = new List<DtoRoleGroup>()
        {
            new DtoRoleGroup()
            {
                Name = Guid.NewGuid().ToString(),
            }
        };
    }
}