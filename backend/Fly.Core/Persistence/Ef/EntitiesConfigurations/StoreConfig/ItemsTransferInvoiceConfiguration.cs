using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class ItemsTransferInvoiceConfiguration : BaseEntityConfiguration<TItemsTransferInvoice>
{
    public override void Configure(EntityTypeBuilder<TItemsTransferInvoice> x)
    {
        x.HasOne(t => t.FromStorage)
            .WithMany()
            .HasForeignKey(f => f.FromStorageId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        x.HasOne(t => t.ToStorage)
            .WithMany()
            .HasForeignKey(f => f.ToStorageId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        base.Configure(x);
    }
}