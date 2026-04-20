using Fly.Core.Modules.Global.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.GlobalConfig;

internal sealed class RoleGroupDetailEntityConfiguration : BaseEntityConfiguration<TRoleGroupDetail>
{
    public override void Configure(EntityTypeBuilder<TRoleGroupDetail> builder)
    {
        builder
            .HasOne(x => x.RoleGroup)
            .WithMany()
            .HasForeignKey(x => x.RoleGroupId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}