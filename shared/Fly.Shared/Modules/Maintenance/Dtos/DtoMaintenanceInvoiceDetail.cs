using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.PurchaseAndSales.Enums;

namespace Fly.Shared.Modules.Maintenance.Dtos;

public partial class DtoMaintenanceInvoiceDetail : DtoBase, IDetailsModel
{
    public Identifier ItemId { get; set; }

    public string? ItemName { get; set; }
    
    public Identifier UnitId { get; set; }

    public string? UnitName { get; set; }
    
    public Identifier? ItemVarianceId { get; set; }

    public string? ItemVarianceName { get; set; }
    
    [ObservableProperty]
    public partial decimal Price { get; set; }

    public decimal Quantity { get; set; } = 1;
    
    public decimal Discount { get; set; }
    
    public Identifier MaintenanceInvoiceId { get; set; }
    
    public Identifier? TemporaryId { get; set; } = Identifier.Create(Guid.NewGuid());
    
    /// <summary>
    /// Line total. mode -100 = before the line discount; any other value = after it.
    /// As on the sales invoice and the printed invoice, the line discount is a fixed
    /// amount for the whole line (it used to turn into a percentage whenever the
    /// invoice discount was a percentage).
    /// </summary>
    public decimal CountPrice(int mode)
    {
        var realPrice = Math.Round(Price * Quantity, 3);
        var final = mode == -100 ? realPrice : realPrice - Discount;
        return Math.Round(final, 3);
    }
}