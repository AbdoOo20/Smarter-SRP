using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class ItemCompositionEntityConfiguration : BaseEntityConfiguration<TItemComposition>
{
    public override void Configure(EntityTypeBuilder<TItemComposition> x)
    {
        x.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(f => f.CategoryId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(x);
    }
}