using Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.FactoringConfigs;

internal sealed class FactoringInvoiceDetailEntityConfiguration : BaseEntityConfiguration<TFactoringInvoiceDetail>
{
    public override void Configure(EntityTypeBuilder<TFactoringInvoiceDetail> builder)
    {
        builder
            .HasOne(t => t.Item)
            .WithMany()
            .HasForeignKey(a => a.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        builder
            .HasOne(t => t.Unit)
            .WithMany()
            .HasForeignKey(a => a.UnitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(t => t.FactoringInvoice)
            .WithMany(h => h.Details)
            .HasForeignKey(a => a.FactoringInvoiceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}