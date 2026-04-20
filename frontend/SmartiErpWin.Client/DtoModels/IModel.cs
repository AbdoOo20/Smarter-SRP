using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;

namespace SmartiErpWin.Client.DtoModels;

public interface IModel
{
    [ClientInvisible]
    public Identifier? TemporaryId { get; set; }
}