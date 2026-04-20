using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.EntitiesConfigurations;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;

[MapTo]
public class TSellReturnInvoiceDetail : TEntity

{
    public Identifier ReturnInvoiceId { get; set; }
    public TSellReturnInvoice? ReturnInvoice { get; set; }

    public TUnit? Unit { get; set; }
    public Identifier UnitId { get; set; }

    public TItem? Item { get; set; }
    public Identifier ItemId { get; set; }

    public decimal Price { get; set; }
    public decimal Quantity { get; set; }

    public DateTime? ExpireDate { get; set; }

    internal sealed class SellReturnInvoiceDetailEntityConfiguration : BaseEntityConfiguration<TSellReturnInvoiceDetail>
    {
        public override void Configure(EntityTypeBuilder<TSellReturnInvoiceDetail> builder)
        {

            builder
                .HasOne(s => s.Unit)
                .WithMany()
                .HasForeignKey(f => f.UnitId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            builder
                .HasOne(s => s.Item)
                .WithMany()
                .HasForeignKey(f => f.ItemId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            builder
                .HasOne(s => s.ReturnInvoice)
                .WithMany(s => s.Details)
                .HasForeignKey(f => f.ReturnInvoiceId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }

}