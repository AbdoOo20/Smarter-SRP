using Fly.Core.Modules.E.Store.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class InventoryEntityConfiguration : BaseEntityConfiguration<TInventory>
{
    public override void Configure(EntityTypeBuilder<TInventory> builder)
    {

        builder
            .HasOne(t => t.Item)
            .WithMany()
            .HasForeignKey(t => t.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(t => t.Unit)
            .WithMany()
            .HasForeignKey(t => t.UnitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(t => t.Store)
            .WithMany()
            .HasForeignKey(t => t.StoreId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);


        base.Configure(builder);
    }
}