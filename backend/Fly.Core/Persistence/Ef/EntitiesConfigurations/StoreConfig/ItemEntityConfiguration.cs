using Fly.Core.Persistence.Ef.Models.Store;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class ItemEntityConfiguration : BaseEntityConfiguration<TItem>
{
    public override void Configure(EntityTypeBuilder<TItem> builder)
    {

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasMany(x => x.Units)
            .WithMany()
            .UsingEntity<TItemUnit>();
        
        //builder
        //    .HasMany(x => x.Units)
        //    .WithMany()
        //    .UsingEntity<TItemUnit>(
        //    cc =>
        //    {
        //        cc
        //        .HasOne(c => c.Unit)
        //        .WithMany()
        //        .HasForeignKey(c => c.UnitId)
        //        .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        //        cc
        //        .HasOne(c => c.Item)
        //        .WithMany()
        //        .HasForeignKey(c => c.ItemId)
        //        .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        //    });

        //.HasForeignKey<TItem>(p => p.CategoryId)
        //.OnDelete(GlobalEnityConfig.DefaultOnDelete);
        ;

        base.Configure(builder);
    }
}