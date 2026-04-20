using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public class CreateUpdateStoreRequest : CreateUpdateRequestBase
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}