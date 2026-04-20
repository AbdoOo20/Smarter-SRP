using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Core.Modules.Global.Seeded;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.GlobalConfig;

internal sealed class RoleEntityConfiguration : BaseEntityConfiguration<TRole>
{
    public override void Configure(EntityTypeBuilder<TRole> builder)
    {
        RoleSeed.Generate();
        var all = RoleSeed.Roles.Adapt<List<TRole>>();
        all.ForEach(f =>
        {
            f.Guid = f.Id.Value;
        });

        builder.HasData(all);

        //RoleSeed.SetAll();
        //var all = RoleSeed.Roles.Adapt<List<TRole>>();
        //foreach (var item in all)
        //    item.Guid = Guid.Parse("EEBB6ED7-85EF-40D4-AD79-8F9D4BBABBA0");
        //builder.HasData(all[0]);

        base.Configure(builder);
    }
}