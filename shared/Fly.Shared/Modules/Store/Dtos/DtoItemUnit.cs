using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using System.Text.Json.Serialization;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoItemUnit : DtoBase, IDetailsModel
{
    [JsonIgnore]
    [ClientInvisible]
    public DtoUnit? Unit { get; set; }
    public string? UnitName { get; set; }
    [ClientInvisible]
    public Identifier UnitId { get; set; }

    [ClientInvisible]
    public string? ItemName { get; set; }
    [ClientInvisible]
    public Identifier ItemId { get; set; }

    [ClientInvisible]
    public bool IsPrimary { get; set; }

    public decimal TransferRate { get; set; } = 1;
    
    public string? Barcode { get; set; }

    public string IsPrimaryText => IsPrimary ? "نعم" : "لا";
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}