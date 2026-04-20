using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class StoreEntityConfiguration : BaseEntityConfiguration<TStore>
{
    public override void Configure(EntityTypeBuilder<TStore> builder)
    {



        base.Configure(builder);
    }
}