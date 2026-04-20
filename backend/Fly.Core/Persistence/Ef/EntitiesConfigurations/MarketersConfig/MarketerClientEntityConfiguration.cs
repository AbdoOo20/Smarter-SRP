using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MarketersConfig;

internal sealed class MarketerClientEntityConfiguration : BaseEntityConfiguration<TMarketerClient>
{
    public override void Configure(EntityTypeBuilder<TMarketerClient> builder)
    {

        builder
            .HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(p => p.ClientId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Marketer)
            .WithMany(x => x.MarketerClients)
            .HasForeignKey(p => p.MarketerId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}