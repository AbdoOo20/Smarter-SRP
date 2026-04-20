using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.AccountingConfig;

internal sealed class EntryEntityConfiguration : BaseEntityConfiguration<TEntry>
{
    public override void Configure(EntityTypeBuilder<TEntry> builder)
    {
        builder
            .Property(w => w.Number)
            .HasValueGenerator<NextNumberGenerator<TEntry>>();

        base.Configure(builder);
    }
}


