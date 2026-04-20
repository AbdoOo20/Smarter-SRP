using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class BankEntityConfiguration : BaseEntityConfiguration<TBank>
{
    public override void Configure(EntityTypeBuilder<TBank> builder)
    {
        base.Configure(builder);
    }
}