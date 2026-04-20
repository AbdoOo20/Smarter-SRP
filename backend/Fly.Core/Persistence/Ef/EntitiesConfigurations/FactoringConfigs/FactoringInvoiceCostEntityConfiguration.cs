using Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.FactoringConfigs;

internal sealed class FactoringInvoiceCostEntityConfiguration : BaseEntityConfiguration<TFactoringInvoiceCost>
{
    public override void Configure(EntityTypeBuilder<TFactoringInvoiceCost> x)
    {
        x.HasOne(t => t.FactoringInvoice)
            .WithMany(h => h.Costs)
            .HasForeignKey(f => f.FactoringInvoiceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        base.Configure(x);
    }
}