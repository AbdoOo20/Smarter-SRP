using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.SeededData;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.GlobalConfig;



//  DtoConfig - TConfig
internal sealed class ConfigEntityConfiguration : BaseEntityConfiguration<TConfig>
{
    public override void Configure(EntityTypeBuilder<TConfig> builder)
    {

        var data = ConfigSeed.Configs.Adapt<List<TConfig>>();
        data.ForEach(d =>
        {
            d.Guid = Identifier.Parse("EEBB6ED7-85EF-40D4-AD79-8F9D4BBABBA0");
            d.TenantId = Identifier.Parse("EEBB6ED7-85EF-40D4-AD79-8F9D4BBABBA1");
        });
        builder.HasData(data);
        base.Configure(builder);
    }
}


//  DtoUser - TUser

//  DtoRole- TRole

// DtoRole- TRole
//internal sealed class UserRoleEntityConfiguration : BaseEntityConfiguration<TUserRole>
//{
//    public override void Configure(EntityTypeBuilder<TUserRole> builder)
//    {
//        //builder
//        //    .HasOne(x => x.User)
//        //    .WithMany(x => x.UserRoles)
//        //    .HasForeignKey(x => x.UserId)
//        //    .OnDelete(GlobalEnityConfig.DefaultOnDelete);
//        builder
//            .HasOne(x => x.Role)
//            .WithMany()
//            .HasForeignKey(x => x.RoleId)
//            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
//        base.Configure(builder);
//    }
//}




//  DtoRole- TRole

//  DtoRole- TRole