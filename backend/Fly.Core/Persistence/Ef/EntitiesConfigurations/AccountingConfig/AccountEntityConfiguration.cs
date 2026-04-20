using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.SeededData;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.AccountingConfig;

internal sealed class AccountEntityConfiguration : BaseEntityConfiguration<TAccount>
{
    public override void Configure(EntityTypeBuilder<TAccount> builder)
    {
        builder
            .HasOne(acc => acc.Parent)
            .WithMany()
            .HasForeignKey(c => c.ParentId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        var data = AccountSeed.Accounts.Adapt<List<TAccount>>();
        data.ForEach(d =>
        {
            d.Guid = Identifier.Parse("EEBB6ED7-85EF-40D4-AD79-8F9D4BBABBD5");
            d.TenantId = Identifier.Parse("EEBB6ED7-85EF-40D4-AD79-8F9D4BBABBD8");
            d.CurrencyId = Identifier.Parse("FC75D0D6-B4F0-4D5D-953C-CE6F44324A4F");
        });
        builder.HasData(data);

        base.Configure(builder);
    }
}