using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoStore : DtoBase
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}