using Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MaintenanceConfigs;

internal sealed class MaintenanceInvoiceDetailEntityConfiguration : BaseEntityConfiguration<TMaintenanceInvoiceDetail>
{
    public override void Configure(EntityTypeBuilder<TMaintenanceInvoiceDetail> builder)
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
            .HasOne(t => t.ItemVariance)
            .WithMany()
            .HasForeignKey(a => a.ItemVarianceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(t => t.MaintenanceInvoice)
            .WithMany(h => h.Details)
            .HasForeignKey(a => a.MaintenanceInvoiceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}