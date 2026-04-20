using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class TDisbursementPermitDetailEntityConfiguration : BaseEntityConfiguration<TDisbursementPermitDetail>
{
    public override void Configure(EntityTypeBuilder<TDisbursementPermitDetail> builder)
    {


        builder
            .HasOne(x => x.DisbursementPermit)
            .WithMany(x => x.DisbursementPermitDetails)
            .HasForeignKey(p => p.DisbursementPermitId)
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