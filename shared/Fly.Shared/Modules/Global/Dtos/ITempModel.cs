using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;

namespace Fly.Shared.Modules.Global.Dtos;

[Obsolete("", error: true)]
public interface ITempModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; }
}