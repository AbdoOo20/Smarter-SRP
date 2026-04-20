using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class SupplyPermitEntityConfiguration : BaseEntityConfiguration<TSupplyPermit>
{
    public override void Configure(EntityTypeBuilder<TSupplyPermit> builder)
    {

        builder
            .Property(w => w.Number)
            .HasValueGenerator<NextNumberGenerator<TSupplyPermit>>();

        builder
            .HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Store)
            .WithMany()
            .HasForeignKey(p => p.StoreId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}