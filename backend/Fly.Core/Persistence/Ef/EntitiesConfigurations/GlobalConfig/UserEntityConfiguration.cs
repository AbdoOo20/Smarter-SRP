using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Core.Modules.Global.Seeded;
using Fly.Framework.Shared.Common;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.GlobalConfig;

internal sealed class UserEntityConfiguration : BaseEntityConfiguration<TUser>
{
    public override void Configure(EntityTypeBuilder<TUser> builder)
    {

        var data = UserSeed.Users.Adapt<List<TUser>>();
        data.ForEach(d =>
        {
            d.Guid = Identifier.Parse("EEBB6ED7-85EF-40D4-AD79-8F9D4BBABBA0");
            d.TenantId = Identifier.Parse("EEBB6ED7-85EF-40D4-AD79-8F9D4BBABBA1");
        });

        //builder
        //    .HasMany(w => w.Roles)
        //    .WithMany()
        //    .UsingEntity<TUserRole>();

        builder
            .HasOne(x => x.RoleGroup)
            .WithMany()
            .HasForeignKey(x => x.RoleGroupId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        builder.Property(u => u.VisibleStorages)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );
        
        builder.Property(u => u.VisibleSafes)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );
        
        builder.Property(u => u.VisibleBanks)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );
        
        builder.HasData(data);
        base.Configure(builder);
    }
}