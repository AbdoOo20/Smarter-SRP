using Fly.Core.Modules.F.Maintenance.MaintenanceSectors.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MaintenanceConfigs;

internal sealed class MaintenanceSectorEntityConfiguration : BaseEntityConfiguration<TMaintenanceSector>
{
    public override void Configure(EntityTypeBuilder<TMaintenanceSector> builder)
    {
        builder
            .HasOne(x => x.Account)
            .WithOne()
            .HasForeignKey<TMaintenanceSector>(c => c.AccountId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        base.Configure(builder);
    }
}