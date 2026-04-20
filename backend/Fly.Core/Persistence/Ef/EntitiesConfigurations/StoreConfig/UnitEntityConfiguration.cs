using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class UnitEntityConfiguration : BaseEntityConfiguration<TUnit>
{
    public override void Configure(EntityTypeBuilder<TUnit> builder)
    {

        base.Configure(builder);
    }
}