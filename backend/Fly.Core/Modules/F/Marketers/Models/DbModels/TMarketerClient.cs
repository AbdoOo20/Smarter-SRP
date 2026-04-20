using Fly.Core.Common.Attributes;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;

namespace Fly.Core.Modules.F.Marketers.Models.DbModels;

[MapTo]
public class TMarketerClient: TEntity
{
    //  
    public Identifier MarketerId { get; set; }
    public TMarketer? Marketer { get; set; }

    public Identifier ClientId { get; set; }
    public TClient? Client { get; set; }
}