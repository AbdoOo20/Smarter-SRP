using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MarketersConfig;

internal sealed class MarketerInventoryEntityConfiguration : BaseEntityConfiguration<TMarketerInventory>
{
    public override void Configure(EntityTypeBuilder<TMarketerInventory> builder)
    {

        builder
            .HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(p => p.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        base.Configure(builder);

        builder
            .HasOne(x => x.Marketer)
            .WithMany()
            .HasForeignKey(p => p.MarketerId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        base.Configure(builder);

        builder
            .HasOne(x => x.Unit)
            .WithMany()
            .HasForeignKey(p => p.UnitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}