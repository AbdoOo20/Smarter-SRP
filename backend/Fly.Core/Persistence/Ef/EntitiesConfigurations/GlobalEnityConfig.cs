using Microsoft.EntityFrameworkCore;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations;

internal static class GlobalEnityConfig
{
    public static DeleteBehavior DefaultOnDelete = DeleteBehavior.Restrict;
}