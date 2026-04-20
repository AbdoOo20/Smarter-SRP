using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class SupplierEntityConfiguration : BaseEntityConfiguration<TSupplier>
{
    public override void Configure(EntityTypeBuilder<TSupplier> builder)
    {

        builder
            .HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        base.Configure(builder);
    }
}