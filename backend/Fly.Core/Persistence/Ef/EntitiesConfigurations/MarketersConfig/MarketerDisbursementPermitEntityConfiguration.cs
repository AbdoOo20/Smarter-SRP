using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MarketersConfig;

internal sealed class MarketerDisbursementPermitEntityConfiguration : BaseEntityConfiguration<TMarketerDisbursementPermit>
{
    public override void Configure(EntityTypeBuilder<TMarketerDisbursementPermit> builder)
    {


        builder
            .HasOne(x => x.Marketer)
            .WithMany()
            .HasForeignKey(p => p.MarketerId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Store)
            .WithMany()
            .HasForeignKey(p => p.StoreId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        // builder
        //     .HasMany(x => x.DisbursementPermitDetails)
        //     .WithOne()
        //     .HasForeignKey(x=> x.DisbursementPermitId)
        //     .OnDelete(GlobalEnityConfig.DefaultOnDelete);


        base.Configure(builder);
    }
}