using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fly.Core.Persistence.Ef.Contexts;

public class DbApplicationContext : DbCustomeContext
{
    //  Infrastructure.MongoDB
    /**/
    /**/
    public DbApplicationContext(DbContextOptions<DbApplicationContext> options) : base(options)
    {
        base.ChangeTracker.LazyLoadingEnabled = false;
        base.ChangeTracker.AutoDetectChangesEnabled = false;
        base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    /**/
    /**/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbApplicationContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
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
    internal DbSet<TAccount> Accounts { get; set; }
    //internal DbSet<TCurrency> Currencies { get; set; }
    /**/
    /**/
}