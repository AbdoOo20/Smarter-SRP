using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class SupplyPermitDetailEntityConfiguration : BaseEntityConfiguration<TSupplyPermitDetail>
{
    public override void Configure(EntityTypeBuilder<TSupplyPermitDetail> builder)
    {


        builder
            .HasOne(x => x.SupplyPermit)
            .WithMany(x => x.SupplyPermitDetails)
            .HasForeignKey(p => p.SupplyPermitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);


        builder
            .HasOne(x => x.Unit)
            .WithMany()
            .HasForeignKey(p => p.UnitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(p => p.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);


        base.Configure(builder);
    }
}