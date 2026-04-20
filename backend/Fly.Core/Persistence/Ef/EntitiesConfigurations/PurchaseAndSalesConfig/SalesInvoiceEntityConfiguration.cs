using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class SalesInvoiceEntityConfiguration : BaseEntityConfiguration<TSalesInvoice>
{
    public override void Configure(EntityTypeBuilder<TSalesInvoice> builder)
    {
        builder
            .Property(w => w.Number)
            .HasValueGenerator<NextNumberGenerator<TSalesInvoice>>();

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
            .HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(p => p.ClientId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Store)
            .WithMany()
            .HasForeignKey(p => p.StoreId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Representative)
            .WithMany()
            .HasForeignKey(p => p.RepresentativeId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);

    }
}