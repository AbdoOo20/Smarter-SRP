using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class SalesInvoiceDetailEntityConfiguration : BaseEntityConfiguration<TSalesInvoiceDetail>
{
    public override void Configure(EntityTypeBuilder<TSalesInvoiceDetail> builder)
    {
        builder
            .HasOne(x => x.SalesInvoice)
            .WithMany(x => x.Details)
            .HasForeignKey(p => p.SalesInvoiceId)
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