using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class PurchaseInvoiceEntityConfiguration : BaseEntityConfiguration<TPurchaseInvoice>
{
    public override void Configure(EntityTypeBuilder<TPurchaseInvoice> builder)
    {
        builder
            .Property(w => w.Number)
            .HasValueGenerator<NextNumberGenerator<TPurchaseInvoice>>();

        builder
            .HasOne(x => x.Safe)
            .WithMany()
            .HasForeignKey(p => p.SafeId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Bank)
            .WithMany()
            .HasForeignKey(p => p.BankId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

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

        //builder
        //    .HasMany(x => x.Details)
        //    .WithOne(x => x.PurchaseInvoice)
        //    .HasForeignKey(p => p.PurchaseInvoiceId)
        //    .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        //builder
        //    .HasMany(x => x.Costs)
        //    .WithOne(x => x.PurchaseInvoice)
        //    .HasForeignKey(p => p.PurchaseInvoiceId)
        //    .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);

    }
}