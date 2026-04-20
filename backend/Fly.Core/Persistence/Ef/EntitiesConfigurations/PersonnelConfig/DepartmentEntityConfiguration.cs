using Fly.Core.Persistence.Ef.Models.Personnel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.PersonnelConfig;

internal sealed class DepartmentEntityConfiguration : BaseEntityConfiguration<TDepartment>
{
    public override void Configure(EntityTypeBuilder<TDepartment> builder)
    {


        base.Configure(builder);
    }
}