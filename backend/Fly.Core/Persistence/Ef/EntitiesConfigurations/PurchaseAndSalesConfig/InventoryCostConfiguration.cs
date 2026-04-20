using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;


//internal sealed class DiscountingItemConfiguration : BaseEntityConfiguration<TDiscountingItem>
//{
//    public override void Configure(EntityTypeBuilder<TDiscountingItem> builder)
//    {

//        builder
//            .HasOne(x => x.Unit)
//            .WithMany()
//            .HasForeignKey(x => x.UnitId)
//            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
//        builder
//            .HasOne(x => x.Item)
//            .WithMany()
//            .HasForeignKey(x => x.ItemId)
//            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
//        builder
//            .HasOne(x => x.Unit)
//            .WithMany()
//            .HasForeignKey(x => x.UnitId)
//            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

//        base.Configure(builder);
//    }
//}

internal sealed class InventoryCostConfiguration : BaseEntityConfiguration<TInventoryCost>
{
    public override void Configure(EntityTypeBuilder<TInventoryCost> builder)
    {

        builder
            .HasOne(x => x.Inventory)
            .WithOne()
            .HasForeignKey<TInventoryCost>(x => x.TInventoryId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}