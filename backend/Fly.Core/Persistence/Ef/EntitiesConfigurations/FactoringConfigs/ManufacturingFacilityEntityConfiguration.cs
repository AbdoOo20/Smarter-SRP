using Fly.Core.Modules.Factoring.ManufacturingFacilities.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.FactoringConfigs;

internal sealed class ManufacturingFacilityEntityConfiguration : BaseEntityConfiguration<TManufacturingFacility>
{
    public override void Configure(EntityTypeBuilder<TManufacturingFacility> builder)
    {
        builder
            .HasOne(x => x.Account)
            .WithOne()
            .HasForeignKey<TManufacturingFacility>(c => c.AccountId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        base.Configure(builder);
    }
}