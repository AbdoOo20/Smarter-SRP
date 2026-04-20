using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MarketersConfig;

internal sealed class MarketerSharedEntryEntityConfiguration : BaseEntityConfiguration<TMarketerSharedEntry>
{
    public override void Configure(EntityTypeBuilder<TMarketerSharedEntry> builder)
    {

        builder
            .HasOne(x => x.Marketer)
            .WithMany()
            .HasForeignKey(p => p.MarketerId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Entry)
            .WithMany()
            .HasForeignKey(p => p.EntryId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}