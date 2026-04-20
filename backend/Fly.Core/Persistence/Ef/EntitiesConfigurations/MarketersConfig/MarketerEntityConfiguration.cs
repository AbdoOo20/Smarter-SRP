using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MarketersConfig;

internal sealed class MarketerEntityConfiguration : BaseEntityConfiguration<TMarketer>
{
    public override void Configure(EntityTypeBuilder<TMarketer> builder)
    {

        builder
            .HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        //builder
        //    .HasMany(x => x.Clients)
        //    .WithMany()
        //    .UsingEntity<TMarketerClient>();

        base.Configure(builder);
    }
}