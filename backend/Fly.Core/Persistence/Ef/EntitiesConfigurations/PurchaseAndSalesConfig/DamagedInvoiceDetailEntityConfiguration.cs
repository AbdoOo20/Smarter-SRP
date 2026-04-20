using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class DamagedInvoiceDetailEntityConfiguration : BaseEntityConfiguration<TDamagedInvoiceDetail>
{
    public override void Configure(EntityTypeBuilder<TDamagedInvoiceDetail> builder)
    {
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
            
        builder
            .HasOne(x => x.DamagedInvoice)
            .WithMany(x => x.Details)
            .HasForeignKey(p => p.DamagedInvoiceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);

    }
}