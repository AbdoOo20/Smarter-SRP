using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class SafeEntityConfiguration : BaseEntityConfiguration<TSafe>
{
    public override void Configure(EntityTypeBuilder<TSafe> builder)
    {
        builder
            .HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}