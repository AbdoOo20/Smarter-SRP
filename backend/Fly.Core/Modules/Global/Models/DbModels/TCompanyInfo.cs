using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.EntitiesConfigurations;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Modules.Global.Models.DbModels;

[MapTo]
public class TCompanyInfo : TEntity
{

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber1 { get; set; }

    public string? PhoneNumber2 { get; set; }

    public string? Email { get; set; }

    public string? InvoiceNotice { get; set; }

    public byte[] ImageData { get; set; } = [];

    internal sealed class EntityConfiguration : BaseEntityConfiguration<TCompanyInfo>
    {
        public override void Configure(EntityTypeBuilder<TCompanyInfo> builder)
        {

            builder.HasData([
                new TCompanyInfo(){

                    Id = Identifier.Parse("81E1B645-A306-404D-AFF0-361FE9FDE041"),
                    Guid = Identifier.Parse("81E1B645-A306-404D-AFF0-361FE9FDE042"),
                    TenantId = Identifier.Parse("81E1B645-A306-404D-AFF0-361FE9FDE043"),

                    Name = "المنظومة المتجددة",
                    Type = "تقنية المعلومات",
                    Email = "contact@mmtech.ly",
                    Address = "بلعون، بنغازي",
                    PhoneNumber1 = "+218 92 023 0018",
                    PhoneNumber2 = "+218 91 521 0018",
                    InvoiceNotice = "تنبيهات الفواتير...",
                }
            ]);
            base.Configure(builder);
        }
    }
}
