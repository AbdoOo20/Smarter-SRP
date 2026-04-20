using Fly.Core.Persistence.Ef.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

internal sealed class QuantityCreditingInvoiceEntityConfiguration : BaseEntityConfiguration<TQuantityCreditingInvoice>
{
    public override void Configure(EntityTypeBuilder<TQuantityCreditingInvoice> builder)
    {
        builder
            .HasOne(s => s.Store)
            .WithMany()
            .HasForeignKey(f => f.StoreId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

        base.Configure(builder);
    }
}