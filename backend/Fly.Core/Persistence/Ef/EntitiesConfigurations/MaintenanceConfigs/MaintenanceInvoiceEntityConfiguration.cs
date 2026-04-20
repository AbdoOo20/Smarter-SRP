using Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;
using Fly.Framework.Server.Persistence.ValueConverters;
using Fly.Framework.Server.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MaintenanceConfigs;

internal sealed class MaintenanceInvoiceEntityConfiguration : BaseEntityConfiguration<TMaintenanceInvoice>
{
    public override void Configure(EntityTypeBuilder<TMaintenanceInvoice> x)
    {
        x.Property(w => w.Number)
            .HasValueGenerator<NextNumberGenerator<TMaintenanceInvoice>>();
        
        x.HasOne(t => t.Store)
            .WithMany()
            .HasForeignKey(f => f.StoreId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete)
            .IsRequired(false);

        x.Property(p => p.StoreId)
            .HasConversion<IdentifierConverter>();
        
        x.Property(p => p.ClientId)
            .HasConversion<IdentifierConverter>();
        
        x.Property(p => p.SafeId)
            .HasConversion<IdentifierConverter>();
        
        x.Property(p => p.BankId)
            .HasConversion<IdentifierConverter>();
        
        x.HasOne(t => t.Client)
            .WithMany()
            .HasForeignKey(f => f.ClientId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        
        x.HasOne(t => t.Safe)
            .WithMany()
            .HasForeignKey(f => f.SafeId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete)
            .IsRequired(false);

        x.HasOne(t => t.Bank)
            .WithMany()
            .HasForeignKey(f => f.BankId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete)
            .IsRequired(false);
        
        x.HasOne(t => t.MaintenanceSector)
            .WithMany()
            .HasForeignKey(f => f.MaintenanceSectorId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        base.Configure(x);
    }
}