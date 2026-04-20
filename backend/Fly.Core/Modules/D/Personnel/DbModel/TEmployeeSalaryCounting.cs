using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.EntitiesConfigurations;
using Fly.Core.Persistence.Ef.Models.Personnel;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Modules.D.Personnel.DbModel;

[MapTo]
internal class TEmployeeSalaryCounting : TEntity
{
    public Identifier EmployeeSalaryCountingPermitId { get; set; }
    public TEmployeeSalaryCountingPermit? EmployeeSalaryCountingPermit { get; set; }

    public decimal Total { get; set; }
    public decimal Salaty { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }

    public Identifier EmployeeId { get; set; }
    public TEmployee? Employee { get; set; }

    internal sealed class EntityConfiguration : BaseEntityConfiguration<TEmployeeSalaryCounting>
    {
        public override void Configure(EntityTypeBuilder<TEmployeeSalaryCounting> builder)
        {

            builder
                .HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.EmployeeSalaryCountingPermit)
                .WithMany()
                .HasForeignKey(x => x.EmployeeSalaryCountingPermitId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}