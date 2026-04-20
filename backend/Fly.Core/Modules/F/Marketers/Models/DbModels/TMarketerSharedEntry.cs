using Fly.Core.Common.Attributes;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.Marketers.Models.DbModels;

[MapTo]
public class TMarketerSharedEntry : TEntity
{
    public Identifier MarketerId { get; set; }
    public TMarketer? Marketer { get; set; }

    public Identifier EntryId { get; set; }
    public TEntry? Entry { get; set; }

    public decimal SharedAmount { get; set; }
}