using Fly.Core.Persistence.Ef.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.AccountingConfig;

internal sealed class AssetsEntityConfiguration : BaseEntityConfiguration<TAssets>
{
    public override void Configure(EntityTypeBuilder<TAssets> builder)
    {
        base.Configure(builder);
    }
}