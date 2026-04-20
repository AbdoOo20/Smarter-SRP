using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Extentions;

namespace Fly.Shared.Modules.Store.Dtos;

public partial class DtoItem : DtoBase
{
    [ClientInvisible]
    public Identifier CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    [ClientInvisible]
    public string? Photo { get; set; }
    public string? Barcode { get; set; }
    
    [ClientInvisible]
    public DateTime? ExpireDate { get; set; }
    public string? ExpireDateString => ExpireDate?.ToDottedDate();
    public decimal Price { get; set; }
    public decimal Cost { get; set; }

    [ClientInvisible]
    public virtual IEnumerable<DtoUnit> Units { get; set; } = [];
    [ClientInvisible]
    public virtual IEnumerable<DtoItemUnit> ItemUnits { get; set; } = [];

    [ClientInvisible]
    public virtual IEnumerable<DtoItemBarcode> ItemBarcodes { get; set; } = [];
    
    public string? Unit
    {
        get
        {
            var def = "";

            if (ItemUnits?.Any() == true)
            {
                def = ItemUnits.FirstOrDefault(u => u.IsPrimary)?.UnitName;
            }

            return def;
        }
    }
    
    [ClientInvisible]
    public string ImageLink => $"/Item/Image/?id={Id.ToString()}&counter={UpdateCounter}";

    [DisplayName("IsItemActive")]
    public bool IsActive { get; set; } = true;

    [ObservableProperty]
    public partial bool Expirable { get; set; }

    public decimal BulkPrice { get; set; }
    
    public decimal RegionalPrice { get; set; }
    
    public decimal MinPrice { get; set; }
    
    public string? OtherUnits
    {
        get
        {
            var def = "";

            if (ItemUnits?.Any() == true)
            {
                def = string.Join(" ,", ItemUnits.Where(s => !s.IsPrimary).Select(s => s.UnitName));
            }

            return def;
        }
    }
    
    [ClientInvisible]
    public double UpdateCounter { get; set; }
}