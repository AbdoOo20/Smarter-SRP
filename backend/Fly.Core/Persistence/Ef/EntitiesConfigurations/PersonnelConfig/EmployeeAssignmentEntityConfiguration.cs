using Fly.Core.Persistence.Ef.Models.Personnel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PersonnelConfig;

internal sealed class EmployeeAssignmentEntityConfiguration : BaseEntityConfiguration<TEmployeeAssignment>
{
    public override void Configure(EntityTypeBuilder<TEmployeeAssignment> builder)
    {
        //  .
        builder
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        //  .
        base.Configure(builder);
    }
}