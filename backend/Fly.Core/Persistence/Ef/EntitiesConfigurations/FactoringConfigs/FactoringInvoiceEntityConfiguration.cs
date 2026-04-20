using Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;
using Fly.Framework.Server.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations.FactoringConfigs;

internal sealed class FactoringInvoiceEntityConfiguration : BaseEntityConfiguration<TFactoringInvoice>
{
    public override void Configure(EntityTypeBuilder<TFactoringInvoice> x)
    {
        x.Property(w => w.Number)
            .HasValueGenerator<NextNumberGenerator<TFactoringInvoice>>();
        
        x.HasOne(t => t.Store)
            .WithMany()
            .HasForeignKey(f => f.StoreId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        x.HasOne(t => t.Client)
            .WithMany()
            .HasForeignKey(f => f.ClientId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);

        x.HasOne(t => t.Safe)
            .WithMany()
            .HasForeignKey(f => f.SafeId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        x.HasOne(t => t.Bank)
            .WithMany()
            .HasForeignKey(f => f.BankId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        x.HasOne(t => t.ManufacturingFacility)
            .WithMany()
            .HasForeignKey(f => f.ManufacturingFacilityId)
            .OnDelete(GlobalEnityConfig.DefaultOnDelete);
        
        base.Configure(x);
    }
}