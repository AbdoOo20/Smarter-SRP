using Fly.Core.Persistence.Ef.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.AccountingConfig;

internal sealed class EntryDetailEntityConfiguration : BaseEntityConfiguration<TEntryDetail>
{
    public override void Configure(EntityTypeBuilder<TEntryDetail> builder)
    {
        builder
           .HasOne(acc => acc.Entry)
           .WithMany(x => x.Details)
           .HasForeignKey(c => c.EntryId)
           .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
           .HasOne(acc => acc.Account)
           .WithMany()
           .HasForeignKey(c => c.AccountId)
           .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}


