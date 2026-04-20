using Fly.Framework.Shared.Common.Attributes;

namespace Fly.Framework.Shared.Common.Interfaces;

public interface IDetailsModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; }
}