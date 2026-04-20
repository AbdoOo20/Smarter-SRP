using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class CostCenterEntityConfiguration : BaseEntityConfiguration<TCostCenter>
{
    public override void Configure(EntityTypeBuilder<TCostCenter> builder)
    {
        base.Configure(builder);
    }
}