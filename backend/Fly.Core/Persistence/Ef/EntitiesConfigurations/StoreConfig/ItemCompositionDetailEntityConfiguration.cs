using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class ItemCompositionDetailEntityConfiguration : BaseEntityConfiguration<TItemCompositionDetail>
{
    public override void Configure(EntityTypeBuilder<TItemCompositionDetail> builder)
    {
        builder
            .HasOne(t => t.Unit)
            .WithMany()
            .HasForeignKey(a => a.UnitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        builder
            .HasOne(t => t.Item)
            .WithMany()
            .HasForeignKey(a => a.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(t => t.ItemVariance)
            .WithMany()
            .HasForeignKey(a => a.ItemVarianceId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        builder.HasOne(t => t.ItemComposition)
            .WithMany(h => h.Details)
            .HasForeignKey(a => a.ItemCompositionId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}