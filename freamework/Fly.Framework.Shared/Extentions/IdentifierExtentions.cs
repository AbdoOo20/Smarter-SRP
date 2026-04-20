using Fly.Framework.Shared.Common;

namespace Fly.Framework.Shared.Extentions;

public static class IdentifierExtentions
{
    public static bool IsNullOrEmpty(this Identifier? id)
    {
        return id is null || id.Value.IsNullOrEmpty();
    }
}