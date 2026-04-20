using Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MaintenanceConfigs;

internal sealed class MaintenanceInvoiceCostEntityConfiguration : BaseEntityConfiguration<TMaintenanceInvoiceCost>
{
    public override void Configure(EntityTypeBuilder<TMaintenanceInvoiceCost> x)
    {
        x.HasOne(t => t.MaintenanceInvoice)
            .WithMany(h => h.Costs)
            .HasForeignKey(f => f.MaintenanceInvoiceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        base.Configure(x);
    }
}