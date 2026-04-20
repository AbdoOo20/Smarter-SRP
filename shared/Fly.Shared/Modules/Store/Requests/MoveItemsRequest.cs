using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Store.Requests;

public class MoveItemsRequest: DtoBase
{
    public Identifier SourceLocationId { get; set; }
    
    public Identifier DestinationLocationId { get; set; }

    public IEnumerable<StorageItemsRequest> Details { get; set; } = [];
}

public class DamageItemsRequest: DtoBase
{
    public Identifier SourceLocationId { get; set; }

    public IEnumerable<StorageItemsRequest> Details { get; set; } = [];
}

public class StorageItemsRequest: DtoBase, IDetailsModel
{
    public Identifier ItemId { get; set; }
    public string ItemName { get; set; }

    public string UnitName { get; set; }
    public Identifier UnitId { get; set; }

    public decimal Quantity { get; set; }
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}