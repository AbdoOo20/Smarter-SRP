using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class DiscountitedItemEntityConfiguration : BaseEntityConfiguration<TDiscountitedItem>
{
    public override void Configure(EntityTypeBuilder<TDiscountitedItem> builder)
    {

        builder
            .HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(p => p.ClientId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(p => p.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);

    }
}