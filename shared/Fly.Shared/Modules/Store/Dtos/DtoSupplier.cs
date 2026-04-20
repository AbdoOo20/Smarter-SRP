using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoSupplier : DtoBase
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    [ClientInvisible]
    public string Identifier { get; set; } = string.Empty;

    [ClientInvisible]
    public Identifier AccountId { get; set; }
    public long AccountNumber { get; set; }

    public decimal AccountOpeningDebtor { get; set; }
    public decimal AccountOpeningCreditor { get; set; }
}