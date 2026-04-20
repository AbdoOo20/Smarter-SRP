using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Shared.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.StoreConfig;

internal sealed class CategoryEntityConfiguration : BaseEntityConfiguration<TCategory>
{
    public override void Configure(EntityTypeBuilder<TCategory> builder)
    {
        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(p => p.CateguryId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete)
            .IsRequired(false);

        builder.HasData(
            new TCategory
            {
                Name = "الفئة الأساسية",
                Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4CCB016"),
                Guid = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4FFB016"),
                Description = "الفئة الأساسية التي تندرج عنها باقي الفئات.", // locale
            }
        );
        base.Configure(builder);
    }
}