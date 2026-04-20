using Fly.Core.Persistence.Ef.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.AccountingConfig;

internal sealed class ExpenseTypesEntityConfiguration : BaseEntityConfiguration<TExpenseTypes>
{
    public override void Configure(EntityTypeBuilder<TExpenseTypes> builder)
    {
        base.Configure(builder);
    }
}