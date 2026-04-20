using Fly.Core.Common.Attributes;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Persistence.Ef.Models.Store;

[MapTo]
public class TSupplier : TEntity
{

    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;

    public TAccount? Account { get; set; }
    public Identifier AccountId { get; set; }

    //public decimal OpeningDebtor { get; set; }
    //public decimal OpeningCreditor { get; set; }
}