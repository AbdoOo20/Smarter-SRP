using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.EntitiesConfigurations;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;


[MapTo]
public class TSellReturnInvoice : TEntity
{
    public DateTime Date { get; set; }
    public long SerialNumber { get; set; }
    public long Number { get; set; }


    public TClient? Client { get; set; }
    public Identifier? ClientId { get; set; }

    public Identifier? InvoiceId { get; set; }
    public TSalesInvoice? Invoice { get; set; }
    
    public TStore? Store { get; set; }
    public Identifier? StoreId { get; set; }
    public PeymentType PeymentType { get; set; }

    public TSafe? Safe { get; set; }
    public Identifier? SafeId { get; set; }

    public virtual ICollection<TSellReturnInvoiceDetail> Details { get; set; } = [];
    public string? Notes { get; set; }

    internal sealed class SellReturnInvoiceEntityConfiguration : BaseEntityConfiguration<TSellReturnInvoice>
    {
        public override void Configure(EntityTypeBuilder<TSellReturnInvoice> builder)
        {
            builder
                .HasOne(s => s.Client)
                .WithMany()
                .HasForeignKey(f => f.ClientId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
        
            builder
                .HasOne(s => s.Safe)
                .WithMany()
                .HasForeignKey(f => f.SafeId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            builder
                .HasOne(s => s.Store)
                .WithMany()
                .HasForeignKey(f => f.StoreId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
        
        
            builder
                .HasOne(s => s.Invoice)
                .WithMany()
                .HasForeignKey(f => f.InvoiceId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }

}