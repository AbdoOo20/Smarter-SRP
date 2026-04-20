using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common;

namespace Fly.Shared.Modules.PurchaseAndSales.Requests;

public partial class SuspendedInvoiceModel: ObservableObject
{
    [ObservableProperty]
    public partial Identifier InvoiceId { get; set; }

    [ObservableProperty]
    public partial string? ClientName { get; set; }
    
    [ObservableProperty]
    public partial string? ClientPhone { get; set; }
    
    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }
    
    [ObservableProperty]
    public partial string? InvoiceDate { get; set; }
}