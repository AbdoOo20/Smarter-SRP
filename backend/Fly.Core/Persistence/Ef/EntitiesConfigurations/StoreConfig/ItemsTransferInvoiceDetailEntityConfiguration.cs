using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class TItemsTransferInvoiceDetailDetailEntityConfiguration : BaseEntityConfiguration<TItemsTransferInvoiceDetail>
{
    public override void Configure(EntityTypeBuilder<TItemsTransferInvoiceDetail> builder)
    {
        builder
            .HasOne(t => t.Unit)
            .WithMany()
            .HasForeignKey(a => a.UnitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        builder
            .HasOne(t => t.Item)
            .WithMany()
            .HasForeignKey(a => a.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder.HasOne(t => t.ItemsTransferInvoice)
            .WithMany(h => h.Details)
            .HasForeignKey(f => f.ItemsTransferInvoiceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        base.Configure(builder);
    }
}