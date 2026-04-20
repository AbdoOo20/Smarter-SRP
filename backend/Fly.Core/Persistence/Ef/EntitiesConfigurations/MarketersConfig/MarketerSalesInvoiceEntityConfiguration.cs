using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.MarketersConfig;


internal sealed class MarketerSalesInvoiceEntityConfiguration : BaseEntityConfiguration<TMarketerSalesInvoice>
{
    public override void Configure(EntityTypeBuilder<TMarketerSalesInvoice> builder)
    {
        builder
            .HasOne(x => x.Marketer)
            .WithMany()
            .HasForeignKey(p => p.MarketerId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);


        builder
            .HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(p => p.ClientId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);

    }
}