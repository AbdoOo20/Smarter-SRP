using Fly.Core.Modules.Global.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.GlobalConfig;

internal sealed class RoleGroupEntityConfiguration : BaseEntityConfiguration<TRoleGroup>
{
    public override void Configure(EntityTypeBuilder<TRoleGroup> builder)
    {

        base.Configure(builder);
    }
}