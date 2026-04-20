using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class ClientEntityConfiguration : BaseEntityConfiguration<TClient>
{
    public override void Configure(EntityTypeBuilder<TClient> builder)
    {
        builder
            .HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}