using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class PurchaseInvoiceCostEntityConfiguration : BaseEntityConfiguration<TPurchaseInvoiceCost>
{
    public override void Configure(EntityTypeBuilder<TPurchaseInvoiceCost> builder)
    {

        builder
            .HasOne(x => x.PurchaseInvoice)
            .WithMany(x => x.Costs)
            .HasForeignKey(p => p.PurchaseInvoiceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}