using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.Marketers.Models.DbModels;

[MapTo]
public class TMarketer : TEntity
{

    public decimal Percentage { get; set; }
    public bool IsRepresentative { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;

    public TAccount? Account { get; set; }
    public Identifier AccountId { get; set; }

    //public IEnumerable<TClient> Clients { get; set; } = [];
    public ICollection<TMarketerClient> MarketerClients { get; set; } = [];

    public decimal OpeningDebtor { get; set; }
    public decimal OpeningCreditor { get; set; }

    public decimal Salary { get; set; }
}