using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MarketersConfig;

internal sealed class MarketerDisbursementPermitDetailEntityConfiguration : BaseEntityConfiguration<TMarketerDisbursementPermitDetail>
{
    public override void Configure(EntityTypeBuilder<TMarketerDisbursementPermitDetail> builder)
    {


        builder
            .HasOne(x => x.MarketerDisbursementPermit)
            .WithMany(x => x.Details)
            .HasForeignKey(p => p.MarketerDisbursementPermitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);


        builder
            .HasOne(x => x.Unit)
            .WithMany()
            .HasForeignKey(p => p.UnitId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        builder
            .HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(p => p.ItemId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);


        base.Configure(builder);
    }
}