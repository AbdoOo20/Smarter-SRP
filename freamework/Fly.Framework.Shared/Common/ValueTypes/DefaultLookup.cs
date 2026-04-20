namespace Fly.Framework.Shared.Common.ValueTypes;

public class DefaultLookup : LookupOf<string, Guid>
{
}

public class StaticLookup : LookupOf<string, string>
{
}