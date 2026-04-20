using Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.FactoringConfigs;

internal sealed class FactoringInvoiceCompositionEntityConfiguration : BaseEntityConfiguration<TFactoringInvoiceComposition>
{
    public override void Configure(EntityTypeBuilder<TFactoringInvoiceComposition> builder)
    {
        builder
            .HasOne(t => t.ItemComposition)
            .WithMany()
            .HasForeignKey(a => a.ItemCompositionId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        builder
            .HasOne(t => t.FactoringInvoice)
            .WithMany(h => h.Compositions)
            .HasForeignKey(a => a.FactoringInvoiceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}