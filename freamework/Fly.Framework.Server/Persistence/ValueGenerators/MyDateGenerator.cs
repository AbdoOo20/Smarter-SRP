using Fly.Framework.Server.Common.Markers;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Helpers;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Fly.Framework.Server.Persistence.ValueGenerators;

public class MyDateGenerator : ValueGenerator<DateTime>
{
    public override bool GeneratesTemporaryValues => false;

    public override DateTime Next(EntityEntry entry)
    {
        return DatetimeHelpers.GetUTC();
    }
}

public class NextNumberGenerator<T> : ValueGenerator<long>
    where T : TEntity, INumberEntity
{
    public override bool GeneratesTemporaryValues => false;

    public override long Next(EntityEntry entry)
    {
        var max = 0L;
        if (entry.Context.Set<T>().Any())
        {
            max = entry.Context.Set<T>().Max(w => w.Number);
        }
        return max + 1;
    }
}
