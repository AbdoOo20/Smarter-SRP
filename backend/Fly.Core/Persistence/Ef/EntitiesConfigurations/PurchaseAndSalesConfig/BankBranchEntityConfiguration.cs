using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PurchaseAndSalesConfig;

internal sealed class BankBranchEntityConfiguration : BaseEntityConfiguration<TBankBranch>
{
    public override void Configure(EntityTypeBuilder<TBankBranch> builder)
    {
        builder
            .HasOne(x => x.Bank)
            .WithMany()
            .HasForeignKey(p => p.BankId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        builder
            .HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        base.Configure(builder);
    }
}