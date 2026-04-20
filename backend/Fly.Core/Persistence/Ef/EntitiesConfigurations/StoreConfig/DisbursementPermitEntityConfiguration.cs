using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class DisbursementPermitEntityConfiguration : BaseEntityConfiguration<TDisbursementPermit>
{
    public override void Configure(EntityTypeBuilder<TDisbursementPermit> builder)
    {

        builder
            .Property(w => w.Number)
            .HasValueGenerator<NextNumberGenerator<TDisbursementPermit>>();

        builder
            .HasOne(x => x.CostCenter)
            .WithMany()
            .HasForeignKey(p => p.CostCenterId)
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