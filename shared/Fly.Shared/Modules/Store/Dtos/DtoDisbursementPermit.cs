using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Store.Dtos;

public class DtoDisbursementPermit : DtoBase
{
    [ClientInvisible]
    public Identifier CostCenterId { get; set; }
    public string CostCenterName { get; set; }
    [ClientInvisible]
    public Identifier StoreId { get; set; }
    [ClientInvisible]
    public DateTime? Date { get; set; }

    public long Number { get; set; }

    public string TimwFormated => Date?.ToString("HH:mm") ?? "";
    public string DateFormated => Date?.ToString("yyyy.MM.dd") ?? "";

    [ClientInvisible]
    public IEnumerable<DtoDisbursementPermitDetail> DisbursementPermitDetails { get; set; } = [];

    public string DetailsString
    {
        get
        {
            var def = "";


            if (DisbursementPermitDetails?.Any() == true)
            {
                var tt = new List<string>();

                foreach (var item in DisbursementPermitDetails)
                {
                    tt.Add($"[{item.Quantity} {item.UnitName} {item.ItemName}]");
                }
                def = string.Join(", ", tt);
            }


            return def;
        }
    }
}