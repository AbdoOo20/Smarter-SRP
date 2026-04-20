using Fly.Core.Persistence.Ef.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

internal sealed class QuantityCreditingInvoiceDetailEntityConfiguration : BaseEntityConfiguration<TQuantityCreditingInvoiceDetail>
{
    public override void Configure(EntityTypeBuilder<TQuantityCreditingInvoiceDetail> builder)
    {

        builder
            .HasOne(s => s.Unit)
            .WithMany()
            .HasForeignKey(f => f.UnitId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

        builder
            .HasOne(s => s.Item)
            .WithMany()
            .HasForeignKey(f => f.ItemId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

        builder
            .HasOne(s => s.QuantityCreditingInvoice)
            .WithMany(s => s.Details)
            .HasForeignKey(f => f.QuantityCreditingInvoiceId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

        base.Configure(builder);
    }
}