using Fly.Framework.Server.Persistence.ValueConverters;
using Fly.Framework.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace Fly.Framework.Server.Persistence.Contexts;

public abstract class DbCustomeContext : DbContext
{
    //  Infrastructure.MongoDB
    /**/
    public Guid ContextUniqeId { get; }
    /**/
    /**/
    protected DbCustomeContext()
    {
        ContextUniqeId = Guid.NewGuid();
        base.ChangeTracker.LazyLoadingEnabled = false;
        base.ChangeTracker.AutoDetectChangesEnabled = false;
        base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    protected DbCustomeContext(DbContextOptions options) : base(options)
    {
        ContextUniqeId = Guid.NewGuid();
        base.ChangeTracker.LazyLoadingEnabled = false;
        base.ChangeTracker.AutoDetectChangesEnabled = false;
        base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    /**/
    /**/
    //protected new virtual void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    //modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbCustomeContext).Assembly);
    //    base.OnModelCreating(modelBuilder);
    //}
    /**/
    /**/
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
#endif
        base.OnConfiguring(optionsBuilder);
    }
    /**/
    /**/
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Identifier>()
            .HaveConversion<IdentifierConverter>();

        configurationBuilder.Properties<Identifier?>()
            .HaveConversion<NullableIdentifierConverter>();
        
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(12, 2);
        
        base.ConfigureConventions(configurationBuilder);
    }
    /**/
    /**/
    public override int SaveChanges()
    {
        //  Log & Sync
        return base.SaveChanges();
    }
    /**/
    /**/
}