using Fly.Core.Persistence.Ef.Contexts;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Fly.Core.Persistence.Ef;

internal class MySqlDbContextDesignTime : IDesignTimeDbContextFactory<DbApplicationContext>
{
    public DbApplicationContext CreateDbContext(string[] args)
    {
        // This is your local database server FOR MIGRATIONS.
        string conStr = "Server=localhost;port=3308;Database=erp_mmtech_20032025;Uid=Admin;Pwd=NaZe3X5V";
        var optionsBuilder = new DbContextOptionsBuilder<DbApplicationContext>();
        var ver = ServerVersion.AutoDetect(conStr);
        optionsBuilder.UseMySql(conStr, ver, x => x.MigrationsAssembly(typeof(MySqlDbContextDesignTime).Assembly.FullName));
        return new DbApplicationContext(optionsBuilder.Options);
    }
}
