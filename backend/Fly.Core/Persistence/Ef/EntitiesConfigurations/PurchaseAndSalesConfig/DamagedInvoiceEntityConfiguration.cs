using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class DamagedInvoiceEntityConfiguration : BaseEntityConfiguration<TDamagedInvoice>
{
    public override void Configure(EntityTypeBuilder<TDamagedInvoice> builder)
    {

        builder
            .Property(w => w.Number)
            .HasValueGenerator<NextNumberGenerator<TDamagedInvoice>>();

        builder
            .HasOne(x => x.MarketerStore)
            .WithMany()
            .HasForeignKey(p => p.MarketerStoreId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Store)
            .WithMany()
            .HasForeignKey(p => p.StoreId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);

    }
}