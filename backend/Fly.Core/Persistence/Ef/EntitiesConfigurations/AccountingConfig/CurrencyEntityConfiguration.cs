using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Shared.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.AccountingConfig;

internal sealed class CurrencyEntityConfiguration : BaseEntityConfiguration<TCurrency>
{
    public override void Configure(EntityTypeBuilder<TCurrency> builder)
    {
        var date = new DateTime(2024, 7, 6);

        builder.HasData(new TCurrency[] {
            new TCurrency
            {
                Letter = "LYD",
                IsPrimary = true,
                TransfereRate = 0,
                Name = "دينار ليبي",
                TenantId = Identifier.Parse("FC75D0D6-B4F0-4D5D-953C-CE6F44324A8F"), // 

                IsDeleted = false,
                CreateDate = date,
                Id = Identifier.Parse("FC75D0D6-B4F0-4D5D-953C-CE6F44324A4F"),
                Guid = Guid.Parse("14EBDB67-1D7D-41E2-B766-DEE66415B487"),
            }
        });

        base.Configure(builder);
    }
}


