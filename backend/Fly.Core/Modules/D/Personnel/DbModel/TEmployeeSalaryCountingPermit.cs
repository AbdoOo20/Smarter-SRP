using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.EntitiesConfigurations;
using Fly.Framework.Server.Persistence.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Modules.D.Personnel.DbModel;

[MapTo]
internal class TEmployeeSalaryCountingPermit : TEntity
{
    public int Number { get; set; }
    public decimal Total { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public DateTime WorkTime { get; set; }
    public int WorkMounth { get; set; }

    internal sealed class EntityConfiguration : BaseEntityConfiguration<TEmployeeSalaryCountingPermit>
    {
        public override void Configure(EntityTypeBuilder<TEmployeeSalaryCountingPermit> builder)
        {


            base.Configure(builder);
        }
    }
}