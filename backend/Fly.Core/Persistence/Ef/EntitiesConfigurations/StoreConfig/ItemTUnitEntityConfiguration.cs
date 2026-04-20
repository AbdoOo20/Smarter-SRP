using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class ItemTUnitEntityConfiguration : BaseEntityConfiguration<TItemUnit>
{
    public override void Configure(EntityTypeBuilder<TItemUnit> x)
    {
        x
            .HasOne(t => t.Unit)
            .WithMany()
            .HasForeignKey(t => t.UnitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);


        x
            .HasOne(t => t.Item)
            .WithMany(x => x.ItemUnits)
            .HasForeignKey(t => t.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);


        base.Configure(x);
    }
}

internal sealed class ItemTBarcodeEntityConfiguration : BaseEntityConfiguration<TItemBarcode>
{
    public override void Configure(EntityTypeBuilder<TItemBarcode> x)
    {
        x
            .HasOne(t => t.Item)
            .WithMany(x => x.ItemBarcodes)
            .HasForeignKey(t => t.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(x);
    }
}