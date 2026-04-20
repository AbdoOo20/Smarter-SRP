using Fly.Core.Persistence.Ef.Models.Personnel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PersonnelConfig;

internal sealed class EmployeeEntityConfiguration : BaseEntityConfiguration<TEmployee>
{
    public override void Configure(EntityTypeBuilder<TEmployee> builder)
    {

        //  .
        //  .
        builder
            .HasOne(x => x.Account)
            .WithOne()
            .HasForeignKey<TEmployee>(c => c.AccountId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        //  .
        //  .
        builder
            .HasOne(x => x.Department)
            .WithMany()
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        //  .
        base.Configure(builder);
    }
}