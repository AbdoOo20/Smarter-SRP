using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using System.Text.Json.Serialization;
using Fly.Framework.Shared.Common.Interfaces;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoSupplyPermit : DtoBase
{
    [ClientInvisible]
    public Identifier SupplierId { get; set; }
    public string SupplierName { get; set; }
    [ClientInvisible]
    public Identifier StoreId { get; set; }
    public string StoreName { get; set; }
    [ClientInvisible]
    public DateTime Date { get; set; }

    public string TimwFormated => Date.ToString("HH:mm") ?? "";
    [JsonIgnore]
    public string DateFormated => Date.ToString("yyyy.MM.dd") ?? "";

    [ClientInvisible]
    public IEnumerable<DtoSupplyPermitDetail>? SupplyPermitDetails { get; set; } = [];

    public string DetailsString
    {
        get
        {
            var def = "";


            if (SupplyPermitDetails?.Any() == true)
            {
                var tt = new List<string>();

                foreach (var item in SupplyPermitDetails)
                {
                    tt.Add($"[{item.Quantity} {item.UnitName} {item.ItemName}]");
                }
                def = string.Join(", ", tt);
            }


            return def;
        }
    }
}

public class DtoSupplyPermitDetail : DtoBase, IDetailsModel
{
    [ClientInvisible]
    public Identifier SupplyPermitId { get; set; }
    [ClientInvisible]
    public Identifier ItemId { get; set; }
    [ClientInvisible]
    public Identifier UnitId { get; set; }

    public string? UnitName { get; set; }
    public string? ItemName { get; set; }

    public decimal Quantity { get; set; }

    public DateTime ExpireDate { get; set; } = DateTime.Now;
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
}