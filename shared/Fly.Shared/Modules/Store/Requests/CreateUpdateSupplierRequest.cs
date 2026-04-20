using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Store.Requests;

public class CreateUpdateSupplierRequest : CreateUpdateRequestBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;

    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }

}