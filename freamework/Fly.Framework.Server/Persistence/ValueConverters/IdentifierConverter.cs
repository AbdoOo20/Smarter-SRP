using Fly.Framework.Shared.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fly.Framework.Server.Persistence.ValueConverters;

public sealed class IdentifierConverter() : ValueConverter<Identifier, Guid>(id => id.Value,
    value => value);

public sealed class NullableIdentifierConverter : ValueConverter<Identifier?, Guid>
{
    public NullableIdentifierConverter()
        : base(id => id.GetValueOrDefault(),
            value => value)
    {
    }
}